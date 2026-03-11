using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private UnityEngine.UI.Button startDayButton;
    //[SerializeField] private TextMeshProUGUI buttonText;
    

    // Start is called before the first frame update
    void Start()
    {
        // if player somehow loads MainMenu first without GameStateManager, create it
        if (GameStateManager.Instance == null)
        {
            GameObject go = new GameObject("GameStateManager");
            go.AddComponent<GameStateManager>();

            
        }
        Debug.Log($"MainMenu loaded. CurrentDay = {GameStateManager.Instance.CurrentDay}");

        UpdateUI();

        startDayButton.onClick.AddListener(OnStartDayClicked);
    

    }

    private void UpdateUI()
    {
        int day = GameStateManager.Instance.CurrentDay;

        if (day <= 3)
        {
            dayText.text = "Day " + day;
            //buttonText.text = "Start Day " + day;
            startDayButton.interactable = true;
            
        }
        else
        {
            // All 3 days complete — go to ending
            dayText.text = "Journey Complete";
            //buttonText.text = "View Ending";
            startDayButton.interactable = true;
           

        }

    }

    private void OnStartDayClicked()
    {
        //if (PauseButton.IsPaused) return;
        if (SettingsPanelUI.IsPaused) return;

        string sceneName = GameStateManager.Instance.GetCurrentDayScene();
        SceneManager.LoadScene(sceneName);
    }

   
}
