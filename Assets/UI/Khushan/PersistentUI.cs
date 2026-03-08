using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance { get; private set; }

    [Header("References")]
    public Slider volumeSlider;
    public Button pauseButton;
    public Text pauseButtonText;
    public AudioMixer audioMixer;

    private bool _isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Restore saved volume
        float saved = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = saved;
        SetVolume(saved);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        pauseButton.onClick.AddListener(TogglePause);
    }

    public void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        pauseButtonText.text = _isPaused ? "▶" : "||";
    }
}