using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class InkDialogManager : MonoBehaviour
{
    public TextAsset inkJSON;

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerNameText;
    public GameObject choices;
    public Button choiceButtonPrefab;
    //public GameObject continueIndicator;


    public NPCPortraitManager NPCportraitManager;

    public float normalTypeSpeed = 0.03f;
    public float slowTypeSpeed = 0.06f;

    private Story story;
    private bool isTyping = false;
    private bool canContinue = false;
    private string fullText = "";
    private float currentTypeSpeed;

    // Start is called before the first frame update
    void Start()
    {
       

        story = new Story(inkJSON.text);
        currentTypeSpeed = normalTypeSpeed;


        // Bind external functions from ink file
        story.BindExternalFunction("EasterEggTrigger", (int ID) =>
        {
            Debug.Log("Easter Egg triggered: " + ID);
            // Add easter egg logic here
        });

        dialogPanel.SetActive(false);
        //continueIndicator.SetActive(false);
        StartDialog();

    }

    public void StartDialog()
    {
        dialogPanel.SetActive(true);

        // Jump to Opening
        story.ChoosePathString("Opening");
        ContinueStory();
    }

    public void ContinueStory()
    {
        // If still typing, finish instantly
        if (isTyping)
        {
            StopAllCoroutines();
            dialogText.text = fullText;
            isTyping = false;
            //continueIndicator.SetActive(true);
            return;
        }

        //continueIndicator.SetActive(false);

        if (story.canContinue)
        {
            string rawText = story.Continue().Trim();
            List<string> tags = story.currentTags;

            // Parse speaker name (lines starting with "Maggie:")
            string speaker = "";
            string displayText = rawText;

            if (rawText.StartsWith("Maggie:"))
            {
                speaker = "Maggie";
                displayText = rawText.Substring("Maggie:".Length).Trim();
            }

            speakerNameText.text = speaker;
            fullText = displayText;

            // Handle tags
            HandleTags(tags);

            // Check if this line uses typewriter
            bool useTypewriter = tags.Contains("type_animation");
            if (useTypewriter)
                currentTypeSpeed = slowTypeSpeed;
            else
                currentTypeSpeed = normalTypeSpeed;

            StartCoroutine(TypeText(displayText));
        }
        else if(story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }

        else
        {
            EndDialog();
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(currentTypeSpeed);
        }

        isTyping = false;

        // After text finishes, show choices or continue indicator
        if (story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else if (story.canContinue)
        {
           // continueIndicator.SetActive(true);
            canContinue = true;
        }
        else
        {
            // Story ended
            //continueIndicator.SetActive(true);
            canContinue = true;
        }
    }

    void DisplayChoices()
    {
        // Clear old choice buttons
        foreach (Transform child in choices.transform)
            Destroy(child.gameObject);

        choices.SetActive(true);

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            Choice choice = story.currentChoices[i];
            Button button = Instantiate(choiceButtonPrefab, choices.transform);

            TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = choice.text.Trim();

            int index = i;
            button.onClick.AddListener(() => OnChoiceSelected(index));
        }
    }

    void OnChoiceSelected(int index)
    {
        story.ChooseChoiceIndex(index);

        // Clear choices
        foreach (Transform child in choices.transform)
            Destroy(child.gameObject);
        choices.SetActive(false);

        ContinueStory();
    }

    void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            // Portrait/expression tags
            if (tag.StartsWith("Teen_"))
            {
                NPCportraitManager.SetExpression(tag);
            }

            // Color class tags
            else if (tag.StartsWith("Class:"))
            {
                string color = tag.Split(':')[1];
                switch (color)
                {
                    case "Blue":
                        dialogText.color = new Color(0.4f, 0.7f, 1f);
                        break;
                    default:
                        dialogText.color = Color.white;
                        break;
                }
            }
            // type_animation is handled in ContinueStory()
        }
        // Reset color if no Class tag present
        if (!tags.Exists(t => t.StartsWith("Class:")))
            dialogText.color = Color.white;

        // Hide portrait if no expression tag
        if (!tags.Exists(t => t.StartsWith("Teen_")))
            NPCportraitManager.Hide();
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        Debug.Log("Dialog ended.");

        // Log final variable states
        Debug.Log($"Anxiety: {story.variablesState["Anxiety"]}");
        Debug.Log($"Sadness: {story.variablesState["Sadness"]}");
        Debug.Log($"Dream: {story.variablesState["Dream"]}");
        Debug.Log($"Stability: {story.variablesState["Stability"]}");
        Debug.Log($"TeenAffinity: {story.variablesState["TeenAffinity"]}");
    }

    void Update()
    {
        if (!dialogPanel.activeSelf) return;

        // Click or Space to continue (only when no choices shown)
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            && story.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    // Public access to Ink variables for other scripts
    public object GetVariable(string name) => story.variablesState[name];
    public void SetVariable(string name, object value) => story.variablesState[name] = value;


}
