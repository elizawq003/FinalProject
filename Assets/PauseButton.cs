using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseButton : MonoBehaviour
{
    public Button pauseButton;
    public TextMeshProUGUI buttonText;

    private bool _isPaused = false;

    private void Start()
    {
        pauseButton.onClick.AddListener(TogglePause);
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        if (buttonText != null)
            buttonText.text = _isPaused ? "Resume" : "Pause";
        Debug.Log($"Game paused: {_isPaused}");
    }
}