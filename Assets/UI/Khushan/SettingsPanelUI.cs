using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsPanelUI : MonoBehaviour
{
    [Header("Settings Button")]
    public Button settingsButton;

    [Header("Popup Panel")]
    public GameObject settingsPopup;
    public Button closeButton;

    [Header("Volume")]
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    [Header("Pause")]
    public Button pauseButton;
    public TextMeshProUGUI pauseButtonText;

    [Header("Font Size")]
    public Slider fontSizeSlider;
    public TextMeshProUGUI[] dialogueTexts;

    private bool _isPaused = false;
    private float[] fontSizes = { 24f, 32f, 40f };

    private void Start()
    {
        settingsPopup.SetActive(true);

        settingsButton.onClick.AddListener(OpenPopup);
        closeButton.onClick.AddListener(ClosePopup);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        // Volume setup
        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 6;
        float saved = PlayerPrefs.GetFloat("MasterVolume", 4f); 
        volumeSlider.value = saved;
        SetVolume(saved);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Font slider setup
        fontSizeSlider.minValue = 0;
        fontSizeSlider.maxValue = 2;
        fontSizeSlider.wholeNumbers = true;
        fontSizeSlider.value = PlayerPrefs.GetInt("FontSizeIndex", 0);
        fontSizeSlider.onValueChanged.AddListener(OnFontSizeChanged);
        ApplyFontSize((int)fontSizeSlider.value);

        settingsPopup.SetActive(false);
    }

    private void OpenPopup()
    {
        settingsPopup.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ClosePopup()
    {
        settingsPopup.SetActive(false);
        if (!_isPaused)
            Time.timeScale = 1f;
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        if (pauseButtonText != null)
            pauseButtonText.text = _isPaused ? "Resume" : "Pause";
        Debug.Log($"Paused: {_isPaused}");
    }

    private void SetVolume(float value)
    {
        float normalized = value / 6f;
        float dB = Mathf.Log10(Mathf.Max(normalized, 0.0001f)) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void OnFontSizeChanged(float value)
    {
        ApplyFontSize((int)value);
    }

    private void ApplyFontSize(int index)
    {
        foreach (var text in dialogueTexts)
        {
            if (text != null)
                text.fontSize = fontSizes[index];
        }
        PlayerPrefs.SetInt("FontSizeIndex", index);
    }
}