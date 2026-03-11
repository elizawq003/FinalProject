using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseButton : MonoBehaviour
{
    public Button pauseButton;
    public TextMeshProUGUI buttonText;

    //private bool _isPaused = false;

    public static bool IsPaused { get; private set; } = false;

    private void Start()
    {
        pauseButton.onClick.AddListener(TogglePause);
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        if (buttonText != null)
            buttonText.text = IsPaused ? "Resume" : "Pause";

        // Pause / unpause the music
        if (MusicManager.Instance != null)
        {
            if (IsPaused) MusicManager.Instance.PauseMusic();
            else MusicManager.Instance.UnPauseMusic();
        }

        Debug.Log($"Game paused: {IsPaused}");
    }
}