using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;


public class OpeningSceneManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;

    [SerializeField] private RectTransform dialogPanel;
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button startButton;
    [SerializeField] private CanvasGroup titleCanvasGroup;

    [SerializeField] private GameObject dialogLinePrefab;

    //for typing animation
    [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private float pauseBetweenLines = 0.8f;
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float titleFadeInDuration = 1.5f;

    //colors
    // light gray
    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    // #5b9bd5
    [SerializeField] private Color blueColor = new Color(0.36f, 0.61f, 0.84f, 1f);
    // dimmed previous lines
    [SerializeField] private Color dimColor = new Color(0.78f, 0.78f, 0.78f, 0.45f);

    //audio setup
    [SerializeField] private AudioClip typeSFX;
    [SerializeField] [Range(0f, 1f)] private float typeVolume = 0.3f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMin = 0.9f;
    [SerializeField] [Range(0.8f, 1.2f)] private float pitchMax = 1.1f;
    // play sound every N characters
    [SerializeField] [Range(1, 6)] private int playSoundEveryNChars = 3;

    private Story inkStory;
    private List<TextMeshProUGUI> spawnedLines = new List<TextMeshProUGUI>();
    private CanvasGroup dialogCanvasGroup;
    private AudioSource audioSource;
    private bool canPlayTypeSound = true;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartClicked);
        //title panel is hidden at start
        titlePanel.SetActive(false);

        // Setup audio source for typing SFX
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;


        //Add CanvasGroup to dialog panel for fading
        dialogCanvasGroup = dialogPanel.GetComponent<CanvasGroup>();
        if (dialogCanvasGroup == null)
            dialogCanvasGroup = dialogPanel.gameObject.AddComponent<CanvasGroup>();

        // Ensure title has a CanvasGroup for fading
        if (titleCanvasGroup == null)
        {
            titleCanvasGroup = titlePanel.GetComponent<CanvasGroup>();
            if (titleCanvasGroup == null)
                titleCanvasGroup = titlePanel.AddComponent<CanvasGroup>();
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        InitializeInk();
        StartCoroutine(PlayOpeningDialog());

    }

    private void InitializeInk()
    {
        inkStory = new Story(inkJSONAsset.text);
        // Register external function stub
        inkStory.BindExternalFunction("EasterEggTrigger", (int ID) =>
        {
            Debug.Log($"EasterEggTrigger called with ID: {ID}");
        });

        // Navigate to Opening knot
        inkStory.ChoosePathString("Opening");
    }

    //Reads lines from the Ink story until it hits the Menu knot
    //displaying each with a typewriter effect.
    private IEnumerator PlayOpeningDialog()
    {
        while (inkStory.canContinue)
        {
            string line = inkStory.Continue().Trim();
            List<string> tags = inkStory.currentTags;

            // Skip empty lines or comments
            if (string.IsNullOrEmpty(line))
                continue;

            //Check if we've reached the Menu knot
            //stop dialog here and show the title screen
            if (line == "A Piece of Night")
            {
                break;
            }

            // Parse tags
            bool isTypeAnimation = false;
            Color lineColor = defaultColor;

            foreach (string tag in tags)
            {
                string t = tag.Trim();
                if (t == "type_animation") isTypeAnimation = true;
                if (t == "Class:Blue") lineColor = blueColor;
            }

            // Dim all previous lines
            foreach (var prev in spawnedLines)
            {
                StartCoroutine(FadeTextColor(prev, dimColor, 0.4f));
            }

            // Spawn new line
            TextMeshProUGUI newLineText = SpawnDialogLine(lineColor);

            if (isTypeAnimation)
            {
                yield return StartCoroutine(TypewriteText(newLineText, line, lineColor));
            }
            else
            {
                newLineText.text = line;
            }

            yield return new WaitForSeconds(pauseBetweenLines);
        }

        // Dialog complete, transition to title
        //stop audio
        canPlayTypeSound = false;
        audioSource.Stop();
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(TransitionToTitle());
    }

    private TextMeshProUGUI SpawnDialogLine(Color color)
    {
        GameObject lineObj = Instantiate(dialogLinePrefab, dialogPanel);
        TextMeshProUGUI tmp = lineObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        tmp.color = color;
        spawnedLines.Add(tmp);
        return tmp;
    }

    //Types out text one character at a time with a blinking cursor.
    private IEnumerator TypewriteText(TextMeshProUGUI textComponent, string fullText, Color color)
    {
        string cursor = "<color=#5BFF5B>▌</color>";
        int charsSinceLastSound = 0;


        for (int i = 0; i <= fullText.Length; i++)
        {
            // Show typed text + blinking cursor
            string visibleText = fullText.Substring(0, i);
            textComponent.text = visibleText + cursor;

            // Play typing sound (skip spaces for a natural rhythm)
            if (i < fullText.Length && fullText[i] != ' ' && typeSFX != null && canPlayTypeSound)
            {
                charsSinceLastSound++;
                if(charsSinceLastSound >= playSoundEveryNChars)
                {
                    audioSource.pitch = Random.Range(pitchMin, pitchMax);
                    audioSource.PlayOneShot(typeSFX, typeVolume);
                    charsSinceLastSound = 0;
                }

            }

            yield return new WaitForSeconds(typeSpeed);
        }
        // Remove cursor, show final text
        textComponent.text = fullText;
    }


    //Fades out dialog, fades in title + start button.
    private IEnumerator TransitionToTitle()
    {
        // Fade out dialog
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            dialogCanvasGroup.alpha = 1f - (elapsed / fadeOutDuration);
            yield return null;
        }

        dialogCanvasGroup.alpha = 0f;
        dialogPanel.gameObject.SetActive(false);

        // Show and fade in title
        titlePanel.SetActive(true);
        titleCanvasGroup.alpha = 0f;

        elapsed = 0f;
        while (elapsed < titleFadeInDuration)
        {
            elapsed += Time.deltaTime;
            titleCanvasGroup.alpha = elapsed / titleFadeInDuration;
            yield return null;
        }
        titleCanvasGroup.alpha = 1f;
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

    private void OnStartClicked()
    {
        Debug.Log("Start button clicked - load menu scene");
        //load menu scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");

    }
}
