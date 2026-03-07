using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Ink.Runtime;

public class EasterEggTwoManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;


    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private Transform choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;


    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);
    [SerializeField] private Color blueColor = new Color(0.36f, 0.61f, 0.84f, 1f);
    [SerializeField] private Color purpleColor = new Color(0.7f, 0.5f, 0.85f, 1f);

    [SerializeField] private Sprite catSprite;

    private Story inkStory;
    private List<GameObject> spawnedChoices = new List<GameObject>();
    private bool isWaitingForChoice = false;

    private Dictionary<string, Sprite> portraitMap;

    // Start is called before the first frame update
    void Start()
    {
        portraitMap = new Dictionary<string, Sprite>
        {
            { "Cat", catSprite },
        };

        InitializeInk();
        StartCoroutine(PlayEasterEgg());
    }

    private void InitializeInk()
    {
        inkStory = new Story(inkJSONAsset.text);

        // Load current game state so variables like Friend and DOS are correct
        GameStateManager.Instance.SyncToInk(inkStory);

        inkStory.BindExternalFunction("EasterEggTrigger", (int id) =>
        {
            Debug.Log($"Easter Egg triggered: {id}");
        });

        inkStory.ObserveVariable("Friend", (string n, object v) =>
            Debug.Log($"Friend = {v}"));
        inkStory.ObserveVariable("DOS", (string n, object v) =>
            Debug.Log($"DOS = {v}"));

        inkStory.ChoosePathString("EsterEgg2");
        Debug.Log($"Easter Egg started: {"EsterEgg2"}");
    }

    private IEnumerator PlayEasterEgg()
    {
        while (true)
        {
            if (inkStory.canContinue)
            {
                string line = inkStory.Continue().Trim();
                List<string> tags = inkStory.currentTags;

                if (string.IsNullOrEmpty(line)) continue;

                // Process tags
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
                }

                // Parse speaker
                string speaker = ParseSpeaker(ref line);
                speakerNameText.text = speaker;
                speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));

                if (!hasPortraitTag && string.IsNullOrEmpty(speaker))
                    characterPortrait.gameObject.SetActive(false);

                choicesPanel.gameObject.SetActive(false);
                dialogText.text = line;

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
                // Easter egg finished — sync variables but do NOT advance day
                SyncWithoutAdvancingDay();
                Debug.Log($"Easter Egg complete — returning to Main Menu. Day stays at {GameStateManager.Instance.CurrentDay}");
                SceneManager.LoadScene("MainMenu");
                yield break;
            }
        }
    }

    // Saves variable changes (like Friend++ or DOS++) back to GameStateManager WITHOUT incrementing CurrentDay.
    private void SyncWithoutAdvancingDay()
    {
        int savedDay = GameStateManager.Instance.CurrentDay;

        // SyncFromInk increments CurrentDay, so we save and restore it
        GameStateManager.Instance.SyncFromInk(inkStory);
        GameStateManager.Instance.CurrentDay = savedDay;
    }

    //helped functions
    private string ParseSpeaker(ref string line)
    {
        string[] speakers = { "Betty", "Cat" };
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
