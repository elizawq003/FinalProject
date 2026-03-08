using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SnackPickerUI : MonoBehaviour
{
    [Header("Desk")]
    [Tooltip("The Image where the chosen snack appears on the desk corner")]
    public Image snackOnDeskImage;

    [Header("Snack Buttons — drag each button from Hierarchy")]
    public Button popcorn;
    public Button cheesecake;
    public Button dumpling;
    public Button burger;
    public Button iceCream;

    [Header("Snack Sprites — drag matching sprite for each snack")]
    public Sprite popcornSprite;
    public Sprite cheesecakeSprite;
    public Sprite dumplingSprite;
    public Sprite burgerSprite;
    public Sprite iceCreamSprite;

    [Header("Selection Highlight")]
    public Color selectedColor   = new Color(0.7f, 0.6f, 1f, 1f);
    public Color unselectedColor = Color.white;

    [Header("Play Button")]
    public Button playButton;
    public string nextSceneName = "NPCScene";

    [Header("Optional")]
    public TextMeshProUGUI selectedSnackLabel;

    private Button _selectedButton = null;
    private string _selectedName   = "";
    private bool   _hasSelected    = false;

    private void Start()
    {
        if (popcorn)    popcorn.onClick.AddListener(()    => SelectSnack(popcorn,    popcornSprite,    "Popcorn"));
        if (cheesecake) cheesecake.onClick.AddListener(() => SelectSnack(cheesecake, cheesecakeSprite, "Cheesecake"));
        if (dumpling)   dumpling.onClick.AddListener(()   => SelectSnack(dumpling,   dumplingSprite,   "Dumpling"));
        if (burger)     burger.onClick.AddListener(()     => SelectSnack(burger,     burgerSprite,     "Burger"));
        if (iceCream)   iceCream.onClick.AddListener(()   => SelectSnack(iceCream,   iceCreamSprite,   "Ice Cream"));

        if (playButton) playButton.onClick.AddListener(OnPlayPressed);

        if (snackOnDeskImage) snackOnDeskImage.enabled = false;

        UpdateLabel();
    }

    private void SelectSnack(Button btn, Sprite sprite, string snackName)
    {
        _selectedButton = btn;
        _selectedName   = snackName;
        _hasSelected    = true;

        ShowOnDesk(sprite);
        RefreshHighlights();
        UpdateLabel();

        // Save to GameStateManager if it exists
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.Dessert = snackName;

        Debug.Log($"[SnackPicker] Selected: {snackName}");
    }

    private void ShowOnDesk(Sprite sprite)
    {
        if (snackOnDeskImage == null || sprite == null) return;

        snackOnDeskImage.sprite  = sprite;
        snackOnDeskImage.enabled = true;
        snackOnDeskImage.SetNativeSize();

        var rect      = snackOnDeskImage.rectTransform;
        float maxPx   = 96f;
        float biggest = Mathf.Max(rect.sizeDelta.x, rect.sizeDelta.y);
        if (biggest > maxPx)
            rect.sizeDelta *= maxPx / biggest;
    }

    private void RefreshHighlights()
    {
        Button[] all = { popcorn, cheesecake, dumpling, burger, iceCream };
        foreach (var btn in all)
        {
            if (btn == null) continue;
            var img = btn.GetComponent<Image>();
            if (img) img.color = (btn == _selectedButton) ? selectedColor : unselectedColor;
        }
    }

    private void UpdateLabel()
    {
        if (selectedSnackLabel == null) return;
        selectedSnackLabel.text = _hasSelected
            ? $"Selected: {_selectedName}"
            : "Pick a snack!";
    }

    private void OnPlayPressed()
    {
        if (!_hasSelected)
        {
            if (selectedSnackLabel)
            {
                selectedSnackLabel.text  = "Pick a snack first!";
                selectedSnackLabel.color = new Color(1f, 0.4f, 0.4f);
                Invoke(nameof(ResetLabel), 1.5f);
            }
            Debug.LogWarning("[SnackPicker] No snack selected.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private void ResetLabel()
    {
        if (selectedSnackLabel)
        {
            selectedSnackLabel.color = Color.white;
            UpdateLabel();
        }
    }
}