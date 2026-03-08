using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Ink.Runtime;

public class EndingSceneManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI endingTitleText;
    [SerializeField] private RectTransform dialogPanel;
    [SerializeField] private GameObject dialogLinePrefab;
    [SerializeField] private UnityEngine.UI.Button NextButton;
    [SerializeField] private TextMeshProUGUI nextButtonText;

    
    [SerializeField] private Image backgroundImage;

  
    [SerializeField] private Sprite bgDesertedStation;
    [SerializeField] private Sprite bgPromotion;
    [SerializeField] private Sprite bgStarrySky;
    [SerializeField] private Sprite bgCityLight;
    [SerializeField] private Sprite bgTraveller;
    [SerializeField] private Sprite bgFriend;
    [SerializeField] private Sprite bgWork;
    [SerializeField] private Sprite bgRoom;
    [SerializeField] private Sprite bgStation;

  
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicRecession;
    [SerializeField] private AudioClip musicPressTheButton;
    [SerializeField] private AudioClip musicTrip;
    [SerializeField] private AudioClip musicIntrovertInTheParty;

    
    [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private float pauseBetweenLines = 1.0f;

    [Header("Colors")]
    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color dimColor = new Color(0.78f, 0.78f, 0.78f, 0.45f);

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

        // Hide replay button until ending finishes
        NextButton.gameObject.SetActive(false);
        NextButton.onClick.AddListener(OnReplayClicked);

        // Display the ending title
        string title = GameStateManager.Instance.EndingReached;
        endingTitleText.text = title;
        Debug.Log($"Ending Scene loaded: {title}");

        // Set initial background from stored tag
        string picTag = GameStateManager.Instance.EndingPicture;
        if (!string.IsNullOrEmpty(picTag) && pictureMap.ContainsKey(picTag))
        {
            backgroundImage.sprite = pictureMap[picTag];
        }

        // Play music from stored tag
        string musTag = GameStateManager.Instance.EndingMusic;
        if (!string.IsNullOrEmpty(musTag) && musicMap.ContainsKey(musTag))
        {
            musicSource.clip = musicMap[musTag];
            musicSource.loop = true;
            musicSource.Play();
        }

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
            NextButton.gameObject.SetActive(true);
            return;
        }

        inkStory = new Story(jsonText);
        inkStory.BindExternalFunction("EasterEggTrigger", (int id) =>
        {
            Debug.Log($"Easter Egg triggered: {id}");
        });

        inkStory.state.LoadJson(storyState);

        StartCoroutine(PlayEndingSequence());
    }

    private IEnumerator PlayEndingSequence()
    {
        // Small delay before starting
        yield return new WaitForSeconds(1.0f);

        while (inkStory.canContinue)
        {
            string line = inkStory.Continue().Trim();
            List<string> tags = inkStory.currentTags;

            if (string.IsNullOrEmpty(line)) continue;

            // Skip the title line (already displayed)
            bool isEndingTitle = false;
            foreach (string tag in tags)
            {
                if (tag.Trim() == "EndingTitle") isEndingTitle = true;
            }
            if (isEndingTitle) continue;

            // Process tags for background and music changes mid-ending
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

        // Ending complete — show replay button
        //canPlayTypeSound = false;
        //sfxAudioSource.Stop();

        yield return new WaitForSeconds(2.0f);
        NextButton.gameObject.SetActive(true);
        nextButtonText.text = "Next";
    }

    private TextMeshProUGUI SpawnLine(Color color)
    {
        GameObject lineObj = Instantiate(dialogLinePrefab, dialogPanel);
        TextMeshProUGUI tmp = lineObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.color = color;
        spawnedLines.Add(tmp);
        return tmp;
    }

    private IEnumerator TypewriteText(TextMeshProUGUI textComponent, string fullText)
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

            yield return new WaitForSeconds(typeSpeed);
        }

        textComponent.text = fullText;
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

    private void OnReplayClicked()
    {
        //GameStateManager.Instance.ResetAll();
        SceneManager.LoadScene("Credits");
    }
}