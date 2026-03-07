using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SavePanelController : MonoBehaviour
{
    [SerializeField] private GameObject savePanel;
    [SerializeField] private UnityEngine.UI.Button saveButton;
    [SerializeField] private UnityEngine.UI.Button cancelButton;
    [SerializeField] private TextMeshProUGUI messageText;


    // Start is called before the first frame update
    void Start()
    {
        // Hidden by default
        savePanel.SetActive(false);

        saveButton.onClick.AddListener(OnSaveClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);

    }

    // Call this from a Day Manager when the day ends.Shows the save panel and pauses the transition.
    public void ShowSavePanel()
    {
        if (messageText != null)
            messageText.text = "Save Progress?";
        savePanel.SetActive(true);
    }

    private void OnSaveClicked()
    {
        SaveManager.Save();

        if (messageText != null)
            messageText.text = "Progress saved!";

        // Brief delay then go to main menu
        Invoke(nameof(GoToMainMenu), 1.0f);
    }

    private void OnCancelClicked()
    {
        savePanel.SetActive(false);
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }



}
