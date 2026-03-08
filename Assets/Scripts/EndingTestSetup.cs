using UnityEngine;
using Ink.Runtime;

public class EndingTestSetup : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;

    void Awake()
    {
        // Create GameStateManager if it doesn't exist
        if (GameStateManager.Instance == null)
        {
            GameObject go = new GameObject("GameStateManager");
            go.AddComponent<GameStateManager>();
        }

        // Set values that lead to Ending 2
        GameStateManager.Instance.Dream = 2;
        GameStateManager.Instance.Achievement = 1;
        GameStateManager.Instance.Stability = 1;
        GameStateManager.Instance.Friend = 0;
        GameStateManager.Instance.DOL = 0;
        GameStateManager.Instance.DOT = 1;
        GameStateManager.Instance.EndingReached = "Preserve the Night";
        GameStateManager.Instance.EndingPicture = "Picture:StarrySky";
        GameStateManager.Instance.EndingMusic = "Music:Recession";

        // Create story and navigate to Ending2
        Story story = new Story(inkJSONAsset.text);
        story.BindExternalFunction("EasterEggTrigger", (int id) => { });
        story.ChoosePathString("Ending2");

        // Save state for EndingSceneManager to pick up
        GameStateManager.Instance.EndingStoryState = story.state.ToJson();
        GameStateManager.Instance.InkJSONText = inkJSONAsset.text;

        Debug.Log("Test setup complete — Ending 2 ready");
    }
}