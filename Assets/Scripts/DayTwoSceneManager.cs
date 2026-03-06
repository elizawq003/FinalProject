using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Ink.Runtime;
using UnityEngine.Video;

public class DayTwoSceneManager : MonoBehaviour
{

    [SerializeField] private TextAsset inkJSONAsset;

    //for opening animation
    [SerializeField] private GameObject openingCanvas;
    [SerializeField] private RectTransform openingDialogPanel;
    [SerializeField] private GameObject dialogLinePrefab;

    //for typing animation
    [SerializeField] private float openingTypeSpeed = 0.04f;
    [SerializeField] private float pauseBetweenLines = 0.8f;
    [SerializeField] private float openingFadeOutDuration = 1.0f;
    //[SerializeField] private float titleFadeInDuration = 1.5f;

    //colors
    [SerializeField] private Color openingDefaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color openingDimColor = new Color(0.78f, 0.78f, 0.78f, 0.45f);


    //audio setup
    [SerializeField] private AudioClip typeSFX;
    [SerializeField] [Range(0f, 1f)] private float typeVolume = 0.3f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMin = 0.9f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMax = 1.1f;
    // play sound every N characters
    [SerializeField] [Range(1, 6)] private int playSoundEveryNChars = 3;

    //day 2 dialog canvas
    [SerializeField] private GameObject dialogCanvas;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Transform choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Image characterPortrait;

    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color blueColor = new Color(0.36f, 0.61f, 0.84f, 1f);
    [SerializeField] private Color purpleColor = new Color(0.7f, 0.5f, 0.85f, 1f);


    [SerializeField] private Sprite kidDefault;
    [SerializeField] private Sprite kidSmile;
    [SerializeField] private Sprite kidSad;


    [SerializeField] private Image backgroundImage;
    [SerializeField] private RawImage videoRawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private Sprite bgRoom2;
    [SerializeField] private Sprite bgCityLight;
    [SerializeField] private Sprite bgStarryNight;

    [SerializeField] private VideoClip bgRoom1Video;
    [SerializeField] private float typewriterSpeed = 0.03f;

    private Story inkStory;
    private List<GameObject> spawnedChoices = new List<GameObject>();
    private List<TextMeshProUGUI> spawnedOpeningLines = new List<TextMeshProUGUI>();
    private bool isWaitingForChoice = false;
    private bool isTyping = false;
    private bool isInOpeningPhase = true;
    private bool canPlayTypeSound = true;

    private Dictionary<string, Sprite> portraitMap;
    private Dictionary<string, Sprite> pictureMap;
    private HashSet<string> videoTags;

    private CanvasGroup openingCanvasGroup;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        // Portrait lookup
        portraitMap = new Dictionary<string, Sprite>
        {
            { "Kid_default", kidDefault },
            { "Kid_smile", kidSmile },
            { "Kid_sad", kidSad },
        };

        // Background lookup
        pictureMap = new Dictionary<string, Sprite>
        {
            { "Picture_Room2", bgRoom2 },
            { "Picture_CityLight", bgCityLight },
            { "Picture:StarryNight", bgStarryNight },
        };

        videoTags = new HashSet<string>
        {
            "Picture_Room1",
        };

        // Hide video layer at start
        if (videoRawImage != null)
            videoRawImage.gameObject.SetActive(false);

        // Setup audio source for opening typing SFX
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Setup canvas group on opening canvas for fading
        openingCanvasGroup = openingCanvas.GetComponent<CanvasGroup>();
        if (openingCanvasGroup == null)
            openingCanvasGroup = openingCanvas.AddComponent<CanvasGroup>();

        // Start with opening canvas visible, dialog canvas hidden
        openingCanvas.SetActive(true);
        dialogCanvas.SetActive(false);

        InitializeInk();

        // Connect emotion bars
        EmotionBarController emotionBar = FindObjectOfType<EmotionBarController>();
        if (emotionBar != null)
            emotionBar.Initialize(inkStory);

        StartCoroutine(PlayFullSequence());


    }


    private void InitializeInk()
    {
        inkStory = new Story(inkJSONAsset.text);

        // Load carried-over values from Day 1
        GameStateManager.Instance.SyncToInk(inkStory);

        // Register external function
        inkStory.BindExternalFunction("EasterEggTrigger", (int id) =>
        {
            Debug.Log($"Easter Egg triggered: {id}");
        });

        // Debug observers
        inkStory.ObserveVariable("KidAffinity", (string n, object v) =>
            Debug.Log($"KidAffinity = {v}"));
        inkStory.ObserveVariable("Dream", (string n, object v) =>
            Debug.Log($"Dream = {v}"));
        inkStory.ObserveVariable("Achievement", (string n, object v) =>
            Debug.Log($"Achievement = {v}"));
        inkStory.ObserveVariable("Stability", (string n, object v) =>
            Debug.Log($"Stability = {v}"));
        inkStory.ObserveVariable("Friend", (string n, object v) =>
            Debug.Log($"Friend = {v}"));

        // Start at Day2 knot
        inkStory.ChoosePathString("Day2");

    }

    private IEnumerator PlayFullSequence()
    {
        // Phase 1: Opening animation (type_animation lines)
        yield return StartCoroutine(PlayOpeningPhase());

        // Transition: fade out opening, show dialog canvas
        yield return StartCoroutine(TransitionToDialog());

        // Phase 2: Normal dialog
        isInOpeningPhase = false;
        yield return StartCoroutine(PlayDialogSequence());
    }

    //PHASE 1 — OPENING ANIMATION
    private IEnumerator PlayOpeningPhase()
    {
        while (inkStory.canContinue)
        {
            string line = inkStory.Continue().Trim();
            List<string> tags = inkStory.currentTags;

            if (string.IsNullOrEmpty(line)) continue;

            // Check if this line has type_animation tag
            bool isTypeAnimation = false;
            Color lineColor = openingDefaultColor;

            foreach (string tag in tags)
            {
                string t = tag.Trim();
                if (t == "type_animation") isTypeAnimation = true;
                if (t == "Class:Blue") lineColor = blueColor;
                if (t == "Class:Purple" || t == "Class\uFF1APurple") lineColor = purpleColor;
            }

            // If this line is NOT a type_animation line, we've passed
            // the opening section — stop and let normal dialog handle it
            if (!isTypeAnimation)
            {
                // We already consumed this line from Ink, so we need to
                // display it as the first line in normal dialog phase.
                // Store it for the dialog phase to pick up.
                _pendingLine = line;
                _pendingTags = tags;
                break;
            }

            // Dim previous lines
            foreach (var prev in spawnedOpeningLines)
            {
                StartCoroutine(FadeTextColor(prev, openingDimColor, 0.4f));
            }

            // Spawn and type the new line
            TextMeshProUGUI newLineText = SpawnOpeningLine(lineColor);
            yield return StartCoroutine(TypewriteOpeningText(newLineText, line, lineColor));
            yield return new WaitForSeconds(pauseBetweenLines);


        }

        // Stop typing audio
        canPlayTypeSound = false;
        audioSource.Stop();
    }

    // Stores a line consumed during opening that belongs to normal dialog
    private string _pendingLine = null;
    private List<string> _pendingTags = null;

    private TextMeshProUGUI SpawnOpeningLine(Color color)
    {
        GameObject lineObj = Instantiate(dialogLinePrefab, openingDialogPanel);
        TextMeshProUGUI tmp = lineObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.color = color;
        spawnedOpeningLines.Add(tmp);
        return tmp;
    }

    private IEnumerator TypewriteOpeningText(TextMeshProUGUI textComponent, string fullText, Color color)
    {
        string cursor = "<color=#5BFF5B>▌</color>";
        int charsSinceLastSound = 0;

        for (int i = 0; i <= fullText.Length; i++)
        {
            string visibleText = fullText.Substring(0, i);
            textComponent.text = visibleText + cursor;

            if (i < fullText.Length && fullText[i] != ' ' && typeSFX != null && canPlayTypeSound)
            {
                charsSinceLastSound++;
                if (charsSinceLastSound >= playSoundEveryNChars)
                {
                    audioSource.pitch = Random.Range(pitchMin, pitchMax);
                    audioSource.PlayOneShot(typeSFX, typeVolume);
                    charsSinceLastSound = 0;
                }
            }

            yield return new WaitForSeconds(openingTypeSpeed);
        }

        textComponent.text = fullText;
    }

    // TRANSITION — fade opening out, show dialog canvas
    private IEnumerator TransitionToDialog()
    {
        yield return new WaitForSeconds(1.0f);

        // Fade out opening canvas
        float elapsed = 0f;
        while (elapsed < openingFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            openingCanvasGroup.alpha = 1f - (elapsed / openingFadeOutDuration);
            yield return null;
        }
        openingCanvasGroup.alpha = 0f;
        openingCanvas.SetActive(false);

        // Show dialog canvas
        dialogCanvas.SetActive(true);
    }

    // PHASE 2 — NORMAL DIALOG
    private IEnumerator PlayDialogSequence()
    {
        // If we have a pending line from the opening phase, display it first
        if (_pendingLine != null)
        {
            ProcessAndDisplayLine(_pendingLine, _pendingTags);
            _pendingLine = null;
            _pendingTags = null;

            yield return new WaitUntil(() =>
                Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
            yield return null;
        }

        while (true)
        {
            if (inkStory.canContinue)
            {
                string line = inkStory.Continue().Trim();
                List<string> tags = inkStory.currentTags;

                // --- DAY 3 TRANSITION CHECK ---
                if (inkStory.state.currentPathString != null &&
                     inkStory.state.currentPathString.StartsWith("Day3"))
                {
                    Debug.Log("=== Reached Day 3 — returning to Main Menu ===");
                    GameStateManager.Instance.SyncFromInk(inkStory);
                    SceneManager.LoadScene("MainMenu");
                    yield break;
                }

                if (string.IsNullOrEmpty(line)) continue;

                ProcessAndDisplayLine(line, tags);

                // Wait for Enter key
                yield return new WaitUntil(() =>
                    Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
                yield return null;
            }
            else if (inkStory.currentChoices.Count > 0)
            {
                SpawnChoices();
                isWaitingForChoice = true;
                yield return new WaitUntil(() => !isWaitingForChoice);
            }
            else
            {
                Debug.Log("End of story reached — returning to Main Menu.");
                GameStateManager.Instance.SyncFromInk(inkStory);
                SceneManager.LoadScene("MainMenu");
                yield break;
            }
        }

    }

    // Processes tags and displays a line in the normal dialog UI.
    private void ProcessAndDisplayLine(string line, List<string> tags)
    {
        bool useTypewriter = false;
        dialogText.color = defaultColor;
        bool hasPortraitTag = false;

        foreach (string tag in tags)
        {
            string t = tag.Trim();

            if (t == "Class:Blue")
                dialogText.color = blueColor;
            else if (t == "Class:Purple" || t == "Class\uFF1APurple")
                dialogText.color = purpleColor;
            else if (t == "type_animation")
                useTypewriter = true;
            else if (portraitMap.ContainsKey(t))
            {
                characterPortrait.sprite = portraitMap[t];
                characterPortrait.gameObject.SetActive(true);
                hasPortraitTag = true;
            }
            else if (videoTags.Contains(t))
            {
                if (t == "Picture_Room1")
                    ShowVideoBackground(bgRoom1Video);
            }
            else if (pictureMap.ContainsKey(t))
            {
                ShowSpriteBackground(pictureMap[t]);
            }
        }
        // Parse speaker name
        string speaker = ParseSpeaker(ref line);
        speakerNameText.text = speaker;
        speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));

        if (!hasPortraitTag && string.IsNullOrEmpty(speaker))
            characterPortrait.gameObject.SetActive(false);

        // Hide choices during normal dialog
        choicesPanel.gameObject.SetActive(false);

        // Display text
        dialogText.text = line;
    }

    //helper method
    private void ShowSpriteBackground(Sprite sprite)
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Stop();
        if (videoRawImage != null)
            videoRawImage.gameObject.SetActive(false);

        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.sprite = sprite;
        }
    }

    private void ShowVideoBackground(VideoClip clip)
    {
        if (videoPlayer == null || videoRawImage == null) return;

        if (videoPlayer.targetTexture == null)
        {
            RenderTexture rt = new RenderTexture(1920, 1080, 0);
            videoPlayer.targetTexture = rt;
            videoRawImage.texture = rt;
        }

        videoRawImage.gameObject.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    private string ParseSpeaker(ref string line)
    {
        string[] speakers = { "Maggie", "Agent X" };
        foreach (string speaker in speakers)
        {
            string prefix = speaker + ":";
            if (line.StartsWith(prefix))
            {
                line = line.Substring(prefix.Length).Trim();
                return speaker;
            }
        }
        return "";
    }

    private IEnumerator TypewriterEffect(string fullText)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in fullText)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                dialogText.text = fullText;
                break;
            }
            dialogText.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
    }

    private IEnumerator FadeTextColor(TextMeshProUGUI text, Color targetColor, float duration)
    {
        Color startColor = text.color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }
        text.color = targetColor;
    }

    private void SpawnChoices()
    {
        choicesPanel.gameObject.SetActive(true);

        foreach (Choice choice in inkStory.currentChoices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesPanel);
            spawnedChoices.Add(buttonObj);

            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

            int idx = choice.index;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnChoiceSelected(idx));
        }
    }

    private void OnChoiceSelected(int index)
    {
        inkStory.ChooseChoiceIndex(index);

        foreach (GameObject btn in spawnedChoices)
        {
            Destroy(btn);
        }

        spawnedChoices.Clear();
        choicesPanel.gameObject.SetActive(false);
        isWaitingForChoice = false;
    }

}
