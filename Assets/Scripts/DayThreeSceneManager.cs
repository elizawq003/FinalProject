using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;
using Ink.Runtime;

public class DayThreeSceneManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;


    // OPENING ANIMATION UI
    [SerializeField] private GameObject openingCanvas;
    [SerializeField] private RectTransform openingDialogPanel;
    [SerializeField] private GameObject dialogLinePrefab;

    [SerializeField] private float openingTypeSpeed = 0.04f;
    [SerializeField] private float pauseBetweenLines = 0.8f;
    [SerializeField] private float openingFadeOutDuration = 1.0f;

    [SerializeField] private Color openingDefaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color openingDimColor = new Color(0.78f, 0.78f, 0.78f, 0.45f);

    /*
    [SerializeField] private AudioClip typeSFX;
    [SerializeField] [Range(0f, 1f)] private float typeVolume = 0.3f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMin = 0.9f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMax = 1.1f;
    [SerializeField] [Range(1, 6)] private int playSoundEveryNChars = 3;
    */

    // NORMAL DIALOG UI
    [SerializeField] private GameObject dialogCanvas;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Transform choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private EmotionBarController emotionBar;

    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color blueColor = new Color(0.36f, 0.61f, 0.84f, 1f);
    [SerializeField] private Color purpleColor = new Color(0.7f, 0.5f, 0.85f, 1f);


    [SerializeField] private Sprite adultDefault;
    [SerializeField] private Sprite adultSmile;
    [SerializeField] private Sprite adultSad;
    [SerializeField] private Sprite adultAngry;
    [SerializeField] private Sprite adultSpeechless;

    [SerializeField] private Image backgroundImage;

    [SerializeField] private RawImage videoRawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private Sprite bgRoom;
    [SerializeField] private Sprite bgRoom2;
    [SerializeField] private Sprite bgCityLight;
    [SerializeField] private Sprite bgStarryNight;
    [SerializeField] private Sprite bgStarrySky;

    /*
    [SerializeField] private Sprite bgDesertedStation;
    [SerializeField] private Sprite bgStation;
    [SerializeField] private Sprite bgPromotion;
    [SerializeField] private Sprite bgTraveller;
    [SerializeField] private Sprite bgFriend;
    [SerializeField] private Sprite bgWork;
    */


    [SerializeField] private VideoClip bgRoom1Video;

    //music
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicRecession;
    //[SerializeField] private AudioClip musicPressTheButton;
    [SerializeField] private AudioClip musicTrip;
    //[SerializeField] private AudioClip musicIntrovertInTheParty;
    

    [SerializeField] private float typewriterSpeed = 0.03f;

    private Story inkStory;
    private List<GameObject> spawnedChoices = new List<GameObject>();
    private List<TextMeshProUGUI> spawnedOpeningLines = new List<TextMeshProUGUI>();
    private bool isWaitingForChoice = false;
    private bool isTyping = false;
    private bool isInOpeningPhase = true;
    //private bool canPlayTypeSound = true;
    private bool reachedEnding = false;
    private string endingName = "";

    private Dictionary<string, Sprite> portraitMap;
    private Dictionary<string, Sprite> pictureMap;
    private Dictionary<string, AudioClip> musicMap;
    private HashSet<string> videoTags;

    private CanvasGroup openingCanvasGroup;
    private AudioSource sfxAudioSource;

    // Stores a line consumed during opening that belongs to normal dialog
    private string _pendingLine = null;
    private List<string> _pendingTags = null;


    // Start is called before the first frame update
    void Start()
    {
        portraitMap = new Dictionary<string, Sprite>
        {
            { "Adult_default", adultDefault },
            { "Adult_smile", adultSmile },
            { "Adult_sad", adultSad },
            { "Adult_angry", adultAngry },
            { "Adult_speechless", adultSpeechless },
        };

        pictureMap = new Dictionary<string, Sprite>
        {
            { "Picture_Room2", bgRoom2 },
            { "Picture_CityLight", bgCityLight },
            { "Picture:StarryNight", bgStarryNight },
            { "Picture:StarrySky", bgStarrySky },
            //{ "Picture:DesertedStation", bgDesertedStation },
            //{ "Picture:Station", bgStation },
            //{ "Picture:Promotion", bgPromotion },
            //{ "Picture:Traveller", bgTraveller },
            //{ "Picture:Friend", bgFriend },
            //{ "Picture:Work", bgWork },
            { "Picture:Room", bgRoom },
        };

        videoTags = new HashSet<string>
        {
            "Picture_Room1",
        };
        
        musicMap = new Dictionary<string, AudioClip>
        {
            { "Music:Recession", musicRecession },
            //{ "Music:PressTheButton", musicPressTheButton },
            { "Music:Trip", musicTrip },
            //{ "Music:IntrovertInTheParty", musicIntrovertInTheParty },
        };
        
        if (videoRawImage != null)
            videoRawImage.gameObject.SetActive(false);

        /*
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.playOnAwake = false;
        */

        openingCanvasGroup = openingCanvas.GetComponent<CanvasGroup>();
        if (openingCanvasGroup == null)
            openingCanvasGroup = openingCanvas.AddComponent<CanvasGroup>();

        openingCanvas.SetActive(true);
        dialogCanvas.SetActive(false);

        InitializeInk();

        StartCoroutine(PlayFullSequence());

    }

    private void InitializeInk()
    {
        inkStory = new Story(inkJSONAsset.text);

        // Load carried-over values from Day 1 & 2
        GameStateManager.Instance.SyncToInk(inkStory);

        inkStory.BindExternalFunction("EasterEggTrigger", (int id) =>
        {
            Debug.Log($"Easter Egg triggered: {id}");
        });

        inkStory.ObserveVariable("TeenAffinity", (string n, object v) =>
            Debug.Log($"TeenAffinity = {v}"));
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
        inkStory.ObserveVariable("DOL", (string n, object v) =>
            Debug.Log($"DOL = {v}"));
        inkStory.ObserveVariable("DOS", (string n, object v) =>
            Debug.Log($"DOS = {v}"));
        inkStory.ObserveVariable("DOT", (string n, object v) =>
            Debug.Log($"DOT = {v}"));
        inkStory.ObserveVariable("DOR", (string n, object v) =>
            Debug.Log($"DOR = {v}"));

        Debug.Log("All Day 3 observers registered.");

        // Log carried-over values
        Debug.Log($"=== Day 3 Start Values ===");
        Debug.Log($"Dream={inkStory.variablesState["Dream"]}, Achievement={inkStory.variablesState["Achievement"]}, Stability={inkStory.variablesState["Stability"]}, Friend={inkStory.variablesState["Friend"]}");
        Debug.Log($"DOL={inkStory.variablesState["DOL"]}, DOR={inkStory.variablesState["DOR"]}, DOT={inkStory.variablesState["DOT"]}, DOS={inkStory.variablesState["DOS"]}");

        inkStory.ChoosePathString("Day3");
    }

    // MAIN SEQUENCE
    private IEnumerator PlayFullSequence()
    {
        // Phase 1: Opening animation
        yield return StartCoroutine(PlayOpeningPhase());

        // Transition
        yield return StartCoroutine(TransitionToDialog());

        // Phase 2: Normal dialog + endings
        isInOpeningPhase = false;
        yield return StartCoroutine(PlayDialogSequence());
    }

    // PHASE 1 — OPENING ANIMATION
    private IEnumerator PlayOpeningPhase()
    {
        while (inkStory.canContinue)
        {
            string line = inkStory.Continue().Trim();
            List<string> tags = inkStory.currentTags;

            if (string.IsNullOrEmpty(line)) continue;

            bool isTypeAnimation = false;
            Color lineColor = openingDefaultColor;

            foreach (string tag in tags)
            {
                string t = tag.Trim();
                if (t == "type_animation") isTypeAnimation = true;
                if (t == "Class:Blue") lineColor = blueColor;
                if (t == "Class:Purple" || t == "Class\uFF1APurple") lineColor = purpleColor;
            }

            if (!isTypeAnimation)
            {
                _pendingLine = line;
                _pendingTags = tags;
                break;
            }

            foreach (var prev in spawnedOpeningLines)
            {
                StartCoroutine(FadeTextColor(prev, openingDimColor, 0.4f));
            }

            TextMeshProUGUI newLineText = SpawnOpeningLine(lineColor);
            yield return StartCoroutine(TypewriteOpeningText(newLineText, line, lineColor));
            yield return new WaitForSeconds(pauseBetweenLines);
        }

        //canPlayTypeSound = false;
        //sfxAudioSource.Stop();
    }

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
        //int charsSinceLastSound = 0;

        for (int i = 0; i <= fullText.Length; i++)
        {
            string visibleText = fullText.Substring(0, i);
            textComponent.text = visibleText + cursor;

            /*
            if (i < fullText.Length && fullText[i] != ' ' && typeSFX != null && canPlayTypeSound)
            {
                charsSinceLastSound++;
                if (charsSinceLastSound >= playSoundEveryNChars)
                {
                    sfxAudioSource.pitch = Random.Range(pitchMin, pitchMax);
                    sfxAudioSource.PlayOneShot(typeSFX, typeVolume);
                    charsSinceLastSound = 0;
                }
            }*/

            yield return new WaitForSeconds(openingTypeSpeed);
        }
        textComponent.text = fullText;
    }

    // TRANSITION
    private IEnumerator TransitionToDialog()
    {
        yield return new WaitForSeconds(1.0f);

        float elapsed = 0f;
        while (elapsed < openingFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            openingCanvasGroup.alpha = 1f - (elapsed / openingFadeOutDuration);
            yield return null;
        }
        openingCanvasGroup.alpha = 0f;
        openingCanvas.SetActive(false);

        dialogCanvas.SetActive(true);

        if (emotionBar != null)
            emotionBar.Initialize(inkStory);


    }

    // PHASE 2 — NORMAL DIALOG (runs until an Ending knot)
    private IEnumerator PlayDialogSequence()
    {
        // Handle pending line from opening phase
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

                

                if (string.IsNullOrEmpty(line)) continue;

                // Check tags for ending title (first line of an ending)
                bool hasEndingTitle = false;
                foreach (string tag in tags)
                {
                    if (tag.Trim() == "EndingTitle")
                    {
                        hasEndingTitle = true;
                        endingName = line; // The line itself is the ending title
                    }
                }

                // If this is an ending title line, store it and trigger ending
                if (hasEndingTitle)
                {
                    // Process remaining tags for music/picture on this line
                    ProcessEndingTags(tags);

                    GameStateManager.Instance.SyncFromInk(inkStory);
                    GameStateManager.Instance.EndingReached = endingName;

                    // Store remaining story for ending scene to continue
                    GameStateManager.Instance.EndingStoryState = inkStory.state.ToJson();
                    GameStateManager.Instance.InkJSONText = inkJSONAsset.text;

                    Debug.Log($"=== Ending: {endingName} — loading Ending scene ===");
                    MusicManager.Instance?.FadeOutAndStop();
                    SceneManager.LoadScene("Ending");
                    yield break;
                }

                ProcessAndDisplayLine(line, tags);

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
                Debug.Log("End of story reached.");
                MusicManager.Instance?.FadeOutAndStop();
                GameStateManager.Instance.SyncFromInk(inkStory);
                SceneManager.LoadScene("Ending");
                yield break;
            }
        }
    }

    

    // Processes music and picture tags from the ending title line and stores them in GameStateManager for the ending scene.
    private void ProcessEndingTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string t = tag.Trim();

            // Store music tag for ending scene
            if (musicMap.ContainsKey(t))
            {
                GameStateManager.Instance.EndingMusic = t;
            }

            // Store picture tag for ending scene
            if (pictureMap.ContainsKey(t))
            {
                GameStateManager.Instance.EndingPicture = t;
            }
        }
    }

    // DISPLAY LINE
    private void ProcessAndDisplayLine(string line, List<string> tags)
    {
        dialogText.color = defaultColor;
        bool hasPortraitTag = false;

        foreach (string tag in tags)
        {
            string t = tag.Trim();

            if (t == "Class:Blue")
                dialogText.color = blueColor;
            else if (t == "Class:Purple" || t == "Class\uFF1APurple")
                dialogText.color = purpleColor;
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
            else if (musicMap.ContainsKey(t))
            {
                PlayMusic(t);
            }
        }

        string speaker = ParseSpeaker(ref line);
        speakerNameText.text = speaker;
        speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));

        if (!hasPortraitTag && string.IsNullOrEmpty(speaker))
            characterPortrait.gameObject.SetActive(false);

        choicesPanel.gameObject.SetActive(false);
        dialogText.text = line;
    }

    //helper functions
    
    private void PlayMusic(string musicTag)
    {
        if (musicSource == null || !musicMap.ContainsKey(musicTag)) return;

        AudioClip clip = musicMap[musicTag];
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

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
        string[] speakers = { "Maggie", "Agent X", "Margaret" };
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
