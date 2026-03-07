using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsSceneManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button replayButton;


    void Start()
    {
        replayButton.onClick.AddListener(OnReplayClicked);
    }

    private void OnReplayClicked()
    {
        GameStateManager.Instance.ResetAll();
        SceneManager.LoadScene("MainMenu");
    }
}
