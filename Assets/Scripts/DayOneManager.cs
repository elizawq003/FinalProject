using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DayOneManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;

    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Transform choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;

    //default color for normal text.
    [SerializeField] private Color defaultColor = new Color(0.78f, 0.78f, 0.78f, 1f);

    //color used when the 'Class:Blue' tag is active.
    [SerializeField] private Color blueColor = new Color(0.36f, 0.61f, 0.84f, 1f);

    private Story inkStory;
    private List<GameObject> spawnedChoices = new List<GameObject>();
    private bool isWaitingForChoice = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the story and start reading it
        InitializeInk();
        StartCoroutine(PlayDialogSequence());

    }

    private void InitializeInk()
    {
        inkStory = new Story(inkJSONAsset.text);

        inkStory.ObserveVariable("TeenAffinity", (string varName, object newValue) => {
            Debug.Log($"Teen Affinity changed to: {newValue}");
        });

        inkStory.ObserveVariable("Dream", (string varName, object newValue) => {
            Debug.Log($"Dream index changed to: {newValue}");
        });

        // Jump directly to the "Start" knot for Day 1
        inkStory.ChoosePathString("Start");
        
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

                // If we reach the start of Day 2, stop this script's loop 
                // (will can replace the Debug.Log with a SceneManager.LoadScene call)
                if (line.Contains("An official visited the Helio Centre today."))
                {
                    Debug.Log("Transitioning to Day 2 Scene...");
                    break;
                }

                // Skip processing if the line is just empty space
                if (string.IsNullOrEmpty(line)) continue;

                // Reset the text color to default
                dialogText.color = defaultColor;

                // Check the Ink tags for this specific line and change color if needed
                foreach (string tag in inkStory.currentTags)
                {
                    if (tag.Trim() == "Class:Blue")
                    {
                        dialogText.color = blueColor;
                    }
                }
                // Instantly display the text
                dialogText.text = line;

                // Pause the loop and wait until the player clicks the left mouse button (or taps the screen)
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                // Wait one extra frame to prevent a single click from accidentally skipping two lines
                yield return null;
            }
            //2. If there is no more text, but there ARE choices...
            else if (inkStory.currentChoices.Count > 0)
            {
                // Generate the buttons for the player
                SpawnChoices();

                // Pause the dialog loop until the player clicks a button
                isWaitingForChoice = true;
                yield return new WaitUntil(() => !isWaitingForChoice);

            }

            // 3. If there is no text and no choices, the story has ended.
            else
            {
                Debug.Log("End of Ink Story reached.");
                break;
            }
        }

    }

    private void SpawnChoices()
    {
        foreach (Choice choice in inkStory.currentChoices)
        {
            // Create a new button from the prefab and place it in the choices panel
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesPanel);

            // Keep track of the spawned buttons so we can delete them later
            spawnedChoices.Add(buttonObj);

            // Set the text on the button to match the Ink choice text
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text;

            // Add a click listener to the button
            Button button = buttonObj.GetComponent<Button>();
            int choiceIndex = choice.index; // Store the index to pass to the listener
            button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
        }
    
    }

    // Called when the player clicks one of the choice buttons
    private void OnChoiceSelected(int index)
    {
        // Tell the Ink story which path the player selected
        inkStory.ChooseChoiceIndex(index);

        // Destroy all the choice buttons to clean up the UI
        foreach (GameObject choiceBtn in spawnedChoices)
        {
            Destroy(choiceBtn);
        }

        // Clear the tracking list
        spawnedChoices.Clear();

        // Unpause the main dialog loop so it can continue reading the next lines
        isWaitingForChoice = false;
    }

    
}
