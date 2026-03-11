using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Ink.Runtime;

public class EndingSceneManager : MonoBehaviour
{

    //dialog panel
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private RectTransform dialogContent;
    [SerializeField] private GameObject dialogLinePrefab;

    [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private float pauseBetweenLines = 1.0f;

    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color dimColor = new Color(0.78f, 0.78f, 0.78f, 0.45f);


    //ending panel
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private TextMeshProUGUI endingTitleText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private UnityEngine.UI.Button replayButton;
    [SerializeField] private TextMeshProUGUI replayButtonText;

    //background
    [SerializeField] private Sprite bgDesertedStation;
    [SerializeField] private Sprite bgPromotion;
    [SerializeField] private Sprite bgStarrySky;
    [SerializeField] private Sprite bgCityLight;
    [SerializeField] private Sprite bgTraveller;
    [SerializeField] private Sprite bgFriend;
    [SerializeField] private Sprite bgWork;
    [SerializeField] private Sprite bgRoom;
    [SerializeField] private Sprite bgStation;

    //music
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicRecession;
    [SerializeField] private AudioClip musicPressTheButton;
    [SerializeField] private AudioClip musicTrip;
    [SerializeField] private AudioClip musicIntrovertInTheParty;

  
   /*
    [SerializeField] private AudioClip typeSFX;
    [SerializeField] [Range(0f, 1f)] private float typeVolume = 0.3f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMin = 0.9f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMax = 1.1f;
    [SerializeField] [Range(1, 6)] private int playSoundEveryNChars = 3;
   */

    private Story inkStory;
    private List<TextMeshProUGUI> spawnedLines = new List<TextMeshProUGUI>();
    //private AudioSource sfxAudioSource;
    //private bool canPlayTypeSound = true;

    private Dictionary<string, Sprite> pictureMap;
    private Dictionary<string, AudioClip> musicMap;

    private CanvasGroup dialogCanvasGroup;
    private CanvasGroup endingCanvasGroup;

    void Start()

    {
        
        pictureMap = new Dictionary<string, Sprite>
        {
            { "Picture:DesertedStation", bgDesertedStation },
           { "Picture:Promotion", bgPromotion },
            { "Picture:StarrySky", bgStarrySky },
            { "Picture:CityLight", bgCityLight },
           { "Picture:Traveller", bgTraveller },
           { "Picture:Friend", bgFriend },
          { "Picture:Work", bgWork },
           { "Picture:Room", bgRoom },
            { "Picture:Station", bgStation },
        };

        musicMap = new Dictionary<string, AudioClip>
        {
            { "Music:Recession", musicRecession },
            { "Music:PressTheButton", musicPressTheButton },
            { "Music:Trip", musicTrip },
            { "Music:IntrovertInTheParty", musicIntrovertInTheParty },
        };

        //sfxAudioSource = gameObject.AddComponent<AudioSource>();
        //sfxAudioSource.playOnAwake = false;

        dialogCanvasGroup = dialogPanel.GetComponent<CanvasGroup>();
        if (dialogCanvasGroup == null)
            dialogCanvasGroup = dialogPanel.AddComponent<CanvasGroup>();

        endingCanvasGroup = endingPanel.GetComponent<CanvasGroup>();
        if (endingCanvasGroup == null)
            endingCanvasGroup = endingPanel.AddComponent<CanvasGroup>();

        // Phase 1 visible, Phase 2 hidden
        dialogPanel.SetActive(true);
        endingPanel.SetActive(false);
        replayButton.gameObject.SetActive(false);
        replayButton.onClick.AddListener(OnReplayClicked);

        // Start music immediately when reach ending scene
        // Play music from stored tag
        string musTag = GameStateManager.Instance.EndingMusic;
        if (!string.IsNullOrEmpty(musTag) && musicMap.ContainsKey(musTag))
        {
            musicSource.clip = musicMap[musTag];
            musicSource.loop = true;
            musicSource.Play();
        }

        // Set ending title and background for Phase 2
        endingTitleText.text = GameStateManager.Instance.EndingReached;

        

        // Set initial background from stored tag
        string picTag = GameStateManager.Instance.EndingPicture;
        if (!string.IsNullOrEmpty(picTag) && pictureMap.ContainsKey(picTag))
        {
            backgroundImage.sprite = pictureMap[picTag];
        }

        Debug.Log($"Ending Scene loaded: {GameStateManager.Instance.EndingReached}");


        // Restore Ink story from saved state and continue
        RestoreAndPlayEnding();
    }

    private void RestoreAndPlayEnding()
    {
        string jsonText = GameStateManager.Instance.InkJSONText;
        string storyState = GameStateManager.Instance.EndingStoryState;

        if (string.IsNullOrEmpty(jsonText) || string.IsNullOrEmpty(storyState))
        {
            Debug.LogError("No story state saved. Cannot play ending.");
            // Skip to ending panel directly
            StartCoroutine(TransitionToEndingPanel());
            return;
        }

        inkStory = new Story(jsonText);
        inkStory.BindExternalFunction("EasterEggTrigger", (int id) =>
        {
            Debug.Log($"Easter Egg triggered: {id}");
        });

        inkStory.state.LoadJson(storyState);

        StartCoroutine(PlayFullSequence());
    }

    private IEnumerator PlayFullSequence()
    {
     
        yield return StartCoroutine(PlayEndingText());


       
        yield return StartCoroutine(TransitionToEndingPanel());
    }

    // PHASE 1 — TYPEWRITER TEXT
    private IEnumerator PlayEndingText()
    {
        yield return new WaitForSeconds(1.0f);

        while (inkStory.canContinue)
        {
            string line = inkStory.Continue().Trim();
            List<string> tags = inkStory.currentTags;

            if (string.IsNullOrEmpty(line)) continue;

            // Skip the title line (will be shown in Phase 2)
            bool isEndingTitle = false;
            foreach (string tag in tags)
            {
                if (tag.Trim() == "EndingTitle") isEndingTitle = true;
            }
            if (isEndingTitle) continue;

            // Process tags for background changes mid-ending
            // (updates the background that Phase 2 will show)
            foreach (string tag in tags)
            {
                string t = tag.Trim();
                if (pictureMap.ContainsKey(t))
                {
                    backgroundImage.sprite = pictureMap[t];
                }
                else if (musicMap.ContainsKey(t))
                {
                    if (musicSource.clip != musicMap[t])
                    {
                        musicSource.clip = musicMap[t];
                        musicSource.Play();
                    }
                }
            }

            // Dim previous lines
            foreach (var prev in spawnedLines)
            {
                StartCoroutine(FadeTextColor(prev, dimColor, 0.4f));
            }

            // Spawn and type new line
            TextMeshProUGUI newLine = SpawnLine(defaultColor);
            yield return StartCoroutine(TypewriteText(newLine, line));
            yield return new WaitForSeconds(pauseBetweenLines);
        }
    }

    private TextMeshProUGUI SpawnLine(Color color)
    {
        GameObject lineObj = Instantiate(dialogLinePrefab, dialogContent);
        TextMeshProUGUI tmp = lineObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.color = color;
        spawnedLines.Add(tmp);
        return tmp;
    }


    private IEnumerator TypewriteText(TextMeshProUGUI textComponent, string fullText)
    {
        string cursor = "<color=#5BFF5B>▌</color>";

        for (int i = 0; i <= fullText.Length; i++)
        {
            string visibleText = fullText.Substring(0, i);
            textComponent.text = visibleText + cursor;
            yield return new WaitForSeconds(typeSpeed);
        }

        textComponent.text = fullText;

    }

    private IEnumerator TransitionToEndingPanel()
    {
        dialogPanel.SetActive(false);
        endingPanel.SetActive(true);
        replayButton.gameObject.SetActive(true);
        replayButtonText.text = "Replay";
        yield return null;
    }


    // HELPERS
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

    private void OnReplayClicked()
    {
        // Kill the persistent MusicManager so it doesn't interfere
        if (MusicManager.Instance != null)
        {
            Destroy(MusicManager.Instance.gameObject);
        }

        GameStateManager.Instance.ResetAll();
        SaveManager.DeleteSave();
        SceneManager.LoadScene("OpeningScene");
    }
}