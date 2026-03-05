using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Ink.Runtime;
using UnityEngine.Video;


public class DayOneManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;

    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Transform choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Image characterPortrait;

    //default color for normal text.
    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);


    //color used when the 'Class:Blue' tag is active.
    [SerializeField] private Color blueColor = new Color(0.36f, 0.61f, 0.84f, 1f);
    //color used when the 'Class:Purple' tag is active
    [SerializeField] private Color purpleColor = new Color(0.7f, 0.5f, 0.85f, 1f);

    private Story inkStory;
    private List<GameObject> spawnedChoices = new List<GameObject>();
    private bool isWaitingForChoice = false;
    private bool isTyping = false;

    //character sprites
    [SerializeField] private Sprite teenDefault;
    [SerializeField] private Sprite teenSmile;
    [SerializeField] private Sprite teenSpeechless;
    [SerializeField] private Sprite teenSad;
    [SerializeField] private Sprite teenAnger;


    [SerializeField] private Image backgroundImage;
    //[SerializeField] private Sprite bgRoom1;

    

    // RawImage on the Canvas that displays the video
    [SerializeField] private RawImage videoRawImage;
    // VideoPlayer component 
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private VideoClip bgRoom1Video;
    [SerializeField] private Sprite bgRoom2;
    [SerializeField] private Sprite bgCityLight;
    [SerializeField] private Sprite bgStarryNight;
    //[SerializeField] private Sprite bgPhoneInterface;
    

    [SerializeField] private float typewriterSpeed = 0.03f;

    private Dictionary<string, Sprite> portraitMap;
    private Dictionary<string, Sprite> pictureMap;
    private HashSet<string> videoTags;

    // Start is called before the first frame update
    void Start()
    {
        portraitMap = new Dictionary<string, Sprite>
        {
            { "Teen_default", teenDefault },
            { "Teen_smile", teenSmile },
            { "Teen_speechless", teenSpeechless },
            { "Teen_sad", teenSad },
            { "Teen_anger", teenAnger },
            { "Teen_angry", teenAnger },
        };

        
        pictureMap = new Dictionary<string, Sprite>
        {
            //{ "Picture_Room1", bgRoom1 },
            { "Picture_Room2", bgRoom2 },
            { "Picture_CityLight", bgCityLight },
            { "Picture:StarryNight", bgStarryNight },
            //{ "Picture:PhoneInterfacewithWords", bgPhoneInterface },
        };

        videoTags = new HashSet<string>
        {
            "Picture_Room1",
        };

        // Hide video layer at start
        if (videoRawImage != null)
            videoRawImage.gameObject.SetActive(false);



        InitializeInk();
        // Connect emotion bars
        EmotionBarController emotionBar = FindObjectOfType<EmotionBarController>();
        if (emotionBar != null)
            emotionBar.Initialize(inkStory);

        StartCoroutine(PlayDialogSequence());

    }

    private void InitializeInk()
    {
        inkStory = new Story(inkJSONAsset.text);

        // Debug observers
        inkStory.ObserveVariable("TeenAffinity", (string n, object v) =>
            Debug.Log($"TeenAffinity = {v}"));

        inkStory.ObserveVariable("Dream", (string n, object v) =>
            Debug.Log($"Dream = {v}"));
        inkStory.ObserveVariable("Achievement", (string n, object v) =>
            Debug.Log($"Achievement = {v}"));
        inkStory.ObserveVariable("Stability", (string n, object v) =>
            Debug.Log($"Stability = {v}"));
        inkStory.ObserveVariable("Friend", (string n, object v) =>
            Debug.Log($"Friend = {v}"));


        // Jump directly to the "Start" knot for Day 1
        inkStory.ChoosePathString("Start");
        
    }

    //Switches to a sprite background. Stops any playing video.
    private void ShowSpriteBackground(Sprite sprite)
    {
        // Stop video and hide video layer
        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Stop();
        if (videoRawImage != null)
            videoRawImage.gameObject.SetActive(false);

        // Show sprite background
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.sprite = sprite;
        }
    }

    //Plays a video background. Hides the sprite background.
    private void ShowVideoBackground(VideoClip clip)
    {
        if (videoPlayer == null || videoRawImage == null) return;

        // Create a RenderTexture if we don't have one yet
        if (videoPlayer.targetTexture == null)
        {
            RenderTexture rt = new RenderTexture(1920, 1080, 0);
            videoPlayer.targetTexture = rt;
            videoRawImage.texture = rt;
        }

        // Show video layer on top
        videoRawImage.gameObject.SetActive(true);

        videoPlayer.clip = clip;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }


    private IEnumerator PlayDialogSequence()
    {
        // Loop continuously until the story ends or we change scenes
        while (true)
        {
            // 1. If there is more text to read...
            if (inkStory.canContinue)
            {
                // Pull the next line of text from Ink
                string line = inkStory.Continue().Trim();
                List<string> tags = inkStory.currentTags;


                if (inkStory.state.currentPathString != null &&
                     inkStory.state.currentPathString.StartsWith("Day2"))
                {
                    Debug.Log("=== Reached Day 2 — transitioning back to menu ===");
                    GameStateManager.Instance.SyncFromInk(inkStory);
                    SceneManager.LoadScene("MainMenu");
                    yield break;
                }


                // Skip processing if the line is just empty space
                if (string.IsNullOrEmpty(line)) continue;

                // --- PROCESS TAGS ---
                bool useTypewriter = false;
                dialogText.color = defaultColor;
                bool hasPortraitTag = false;

                foreach (string tag in tags)
                {
                    string t = tag.Trim();

                    // Color tags
                    if (t == "Class:Blue")
                        dialogText.color = blueColor;
                    else if (t == "Class:Purple" || t == "Class\uFF1APurple")
                        dialogText.color = purpleColor;

                    else if (t == "type_animation")
                        useTypewriter = true;

                    // Portrait tags
                    else if (portraitMap.ContainsKey(t))
                    {
                        characterPortrait.sprite = portraitMap[t];
                        characterPortrait.gameObject.SetActive(true);
                        hasPortraitTag = true;
                    }
                    
                    // Picture / CG tags
                    else if (videoTags.Contains(t))
                    {
                        if (t == "Picture_Room1")
                            ShowVideoBackground(bgRoom1Video);
                    }
                    // Sprite background tags
                    else if (pictureMap.ContainsKey(t))
                    {
                        ShowSpriteBackground(pictureMap[t]);
                    }

                }

                // --- PARSE SPEAKER NAME ---
                string speaker = ParseSpeaker(ref line);
                speakerNameText.text = speaker;
                speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));

                // Hide portrait for narration lines with no portrait tag and no speaker
                if (!hasPortraitTag && string.IsNullOrEmpty(speaker))
                    characterPortrait.gameObject.SetActive(false);

                // --- HIDE CHOICES PANEL DURING NORMAL DIALOG ---
                choicesPanel.gameObject.SetActive(false);

                // --- DISPLAY TEXT ---
                if (useTypewriter)
                    yield return StartCoroutine(TypewriterEffect(line));
                else
                    dialogText.text = line;

                // Wait for Enter key to advance to next line
                yield return new WaitUntil(() =>
                    Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));

                // Wait one frame to prevent double-advance
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
            // Press Enter to skip typewriter and show full text instantly
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

    // Called when the player clicks one of the choice buttons
    private void OnChoiceSelected(int index)
    {
        // Tell the Ink story which path the player selected
        inkStory.ChooseChoiceIndex(index);

        // Destroy all the choice buttons to clean up the UI
        foreach (GameObject btn in spawnedChoices)
        {
            Destroy(btn);
        }

        // Clear the tracking list
        spawnedChoices.Clear();

        choicesPanel.gameObject.SetActive(false);
        // Unpause the main dialog loop so it can continue reading the next lines
        isWaitingForChoice = false;
    }

    
}
