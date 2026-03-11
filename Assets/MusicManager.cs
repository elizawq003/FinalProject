using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Songs — drag your 3 audio clips here")]
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;
    public AudioMixerGroup mixerGroup;
    public float fadeDuration = 2f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = mixerGroup;
    }

    public void PlaySong(int index)
    {
        AudioClip clip = index switch
        {
            1 => song1,
            2 => song2,
            3 => song3,
            _ => null
        };

        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.Play();

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.Song = "Song" + index;

        Debug.Log($"[MusicManager] Playing song {index}");
    }

    public void FadeOutAndStop()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 1f;
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();
    }

    public void UnPauseMusic()
    {
        if (audioSource != null)
            audioSource.UnPause();
    }
}