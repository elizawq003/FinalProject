using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SnackPickerUI : MonoBehaviour
{
    [Header("Desk")]
    public Image snackOnDeskImage;
    public Image drinkOnDeskImage;

    [Header("Snack Buttons")]
    public Button popcorn;
    public Button cheesecake;
    public Button dumpling;
    public Button burger;
    public Button hotdog;

    [Header("Drink Buttons")]
    public Button hotChocolate;
    public Button coffee;

    [Header("Snack Sprites")]
    public Sprite popcornSprite;
    public Sprite cheesecakeSprite;
    public Sprite dumplingSprite;
    public Sprite burgerSprite;
    public Sprite hotdogSprite;

    [Header("Drink Sprites")]
    public Sprite hotChocolateSprite;
    public Sprite coffeeSprite;

    [Header("Selection Highlight")]
    public Color selectedColor   = new Color(0.7f, 0.6f, 1f, 1f);
    public Color unselectedColor = Color.white;

    [Header("Play Button")]
    public Button playButton;
    public string nextSceneName = "NPCScene";

    [Header("Optional")]
    public TextMeshProUGUI selectedSnackLabel;

    private Button _selectedSnackButton = null;
    private Button _selectedDrinkButton = null;
    private string _selectedSnackName   = "";
    private string _selectedDrinkName   = "";

    private void Start()
    {
        // Snack listeners
        if (popcorn)    popcorn.onClick.AddListener(()    => SelectSnack(popcorn,    popcornSprite,    "Popcorn"));
        if (cheesecake) cheesecake.onClick.AddListener(() => SelectSnack(cheesecake, cheesecakeSprite, "Cheesecake"));
        if (dumpling)   dumpling.onClick.AddListener(()   => SelectSnack(dumpling,   dumplingSprite,   "Dumpling"));
        if (burger)     burger.onClick.AddListener(()     => SelectSnack(burger,     burgerSprite,     "Burger"));
        if (hotdog)     hotdog.onClick.AddListener(()     => SelectSnack(hotdog,     hotdogSprite,     "Hot Dog"));

        // Drink listeners
        if (hotChocolate) hotChocolate.onClick.AddListener(() => SelectDrink(hotChocolate, hotChocolateSprite, "Hot Chocolate"));
        if (coffee)       coffee.onClick.AddListener(()       => SelectDrink(coffee,       coffeeSprite,       "Coffee"));

        if (playButton) playButton.onClick.AddListener(OnPlayPressed);

        if (snackOnDeskImage) snackOnDeskImage.enabled = false;
        if (drinkOnDeskImage) drinkOnDeskImage.enabled = false;

        UpdateLabel();
    }

    private void SelectSnack(Button btn, Sprite sprite, string snackName)
    {
        _selectedSnackButton = btn;
        _selectedSnackName   = snackName;

        ShowOnDesk(snackOnDeskImage, sprite);
        RefreshHighlights();
        UpdateLabel();

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.Dessert = snackName;

        Debug.Log($"[SnackPicker] Snack selected: {snackName}");
    }

    private void SelectDrink(Button btn, Sprite sprite, string drinkName)
    {
        _selectedDrinkButton = btn;
        _selectedDrinkName   = drinkName;

        ShowOnDesk(drinkOnDeskImage, sprite);
        RefreshHighlights();
        UpdateLabel();

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.Drink = drinkName;

        Debug.Log($"[SnackPicker] Drink selected: {drinkName}");
    }

    private void ShowOnDesk(Image target, Sprite sprite)
    {
        if (target == null || sprite == null) return;

        target.sprite  = sprite;
        target.enabled = true;
        target.SetNativeSize();

        var rect    = target.rectTransform;
        float maxPx = 96f;
        float biggest = Mathf.Max(rect.sizeDelta.x, rect.sizeDelta.y);
        if (biggest > maxPx)
            rect.sizeDelta *= maxPx / biggest;
    }

    private void RefreshHighlights()
    {
        // Snack group
        Button[] snacks = { popcorn, cheesecake, dumpling, burger, hotdog };
        foreach (var btn in snacks)
        {
            if (btn == null) continue;
            var img = btn.GetComponent<Image>();
            if (img) img.color = (btn == _selectedSnackButton) ? selectedColor : unselectedColor;
        }

        // Drink group
        Button[] drinks = { hotChocolate, coffee };
        foreach (var btn in drinks)
        {
            if (btn == null) continue;
            var img = btn.GetComponent<Image>();
            if (img) img.color = (btn == _selectedDrinkButton) ? selectedColor : unselectedColor;
        }
    }

    private void UpdateLabel()
    {
        if (selectedSnackLabel == null) return;

        bool hasSnack = !string.IsNullOrEmpty(_selectedSnackName);
        bool hasDrink = !string.IsNullOrEmpty(_selectedDrinkName);

        if (hasSnack && hasDrink)
            selectedSnackLabel.text = $"{_selectedSnackName} + {_selectedDrinkName}";
        else if (hasSnack)
            selectedSnackLabel.text = $"{_selectedSnackName} — pick a drink!";
        else if (hasDrink)
            selectedSnackLabel.text = $"{_selectedDrinkName} — pick a snack!";
        else
            selectedSnackLabel.text = "Pick a snack and a drink!";
    }

    private void OnPlayPressed()
    {
        bool hasSnack = !string.IsNullOrEmpty(_selectedSnackName);
        bool hasDrink = !string.IsNullOrEmpty(_selectedDrinkName);

        if (!hasSnack || !hasDrink)
        {
            if (selectedSnackLabel)
            {
                selectedSnackLabel.text  = !hasSnack ? "Pick a snack first!" : "Pick a drink first!";
                selectedSnackLabel.color = new Color(1f, 0.4f, 0.4f);
                Invoke(nameof(ResetLabel), 1.5f);
            }
            Debug.LogWarning("[SnackPicker] Selection incomplete.");
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