using UnityEngine;
using UnityEngine.UI;

public class SongPickerUI : MonoBehaviour
{
    [Header("Song Buttons")]
    public Button song1Button;
    public Button song2Button;
    public Button song3Button;

    [Header("Highlight Colors")]
    public Color selectedColor   = new Color(0.7f, 0.6f, 1f, 1f);
    public Color unselectedColor = Color.white;

    private Button _selectedButton = null;

    private void Start()
    {
        song1Button.onClick.AddListener(() => SelectSong(song1Button, 1));
        song2Button.onClick.AddListener(() => SelectSong(song2Button, 2));
        song3Button.onClick.AddListener(() => SelectSong(song3Button, 3));

        // Restore previous selection if returning from a day
        RestorePreviousSelection();
    }

    private void SelectSong(Button btn, int index)
    {
        _selectedButton = btn;
        RefreshHighlights();
        MusicManager.Instance?.PlaySong(index);
        Debug.Log($"[SongPicker] Selected song {index}");
    }

    private void RefreshHighlights()
    {
        Button[] all = { song1Button, song2Button, song3Button };
        foreach (var btn in all)
        {
            if (btn == null) continue;
            var img = btn.GetComponent<Image>();
            if (img) img.color = (btn == _selectedButton) ? selectedColor : unselectedColor;
        }
    }

    private void RestorePreviousSelection()
    {
        if (GameStateManager.Instance == null) return;

        // Highlight previously chosen song when returning to menu
        string saved = GameStateManager.Instance.Song;
        if (saved == "Song1") SelectSong(song1Button, 1);
        else if (saved == "Song2") SelectSong(song2Button, 2);
        else if (saved == "Song3") SelectSong(song3Button, 3);
    }
}