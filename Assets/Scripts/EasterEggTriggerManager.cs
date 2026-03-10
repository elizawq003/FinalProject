using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggTriggerManager : MonoBehaviour
{
    public static EasterEggTriggerManager Instance { get; private set; }

    [Header("Scene Names")]
    public string easterEgg1Scene = "EasterEgg1";
    public string easterEgg2Scene = "EasterEgg2";
    public string day1Scene = "DayOne";
    public string day2Scene = "DayTwo";
    public string day3Scene = "DayThree";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this from SnackPickerUI's OnPlayPressed
    public void TriggerEasterEgg()
    {
        string snack = GameStateManager.Instance?.Dessert ?? "";
        string drink = GameStateManager.Instance?.Drink   ?? "";
        string song  = GameStateManager.Instance?.Song    ?? "";
        int    day   = GameStateManager.Instance?.CurrentDay ?? 1;

        string combo = $"{snack}|{drink}|{song}";
        string nextDay = GetNextDayScene(day);

        Debug.Log($"[EasterEggTrigger] Day={day} Combo={combo} NextDay={nextDay}");

        // Only check for easter eggs before Day 2 and Day 3
        if (day >= 2 && IsEasterEggCombo(combo))
        {
            // Store where to go after the easter egg finishes
            GameStateManager.Instance.PostEasterEggScene = nextDay;

            if (IsEasterEgg1(combo))
            {
                Debug.Log("[EasterEggTrigger] → EasterEgg1");
                SceneManager.LoadScene(easterEgg1Scene);
            }
            else
            {
                Debug.Log("[EasterEggTrigger] → EasterEgg2");
                SceneManager.LoadScene(easterEgg2Scene);
            }
        }
        else
        {
            Debug.Log($"[EasterEggTrigger] → {nextDay} (no easter egg)");
            SceneManager.LoadScene(nextDay);
        }
    }

    private string GetNextDayScene(int currentDay)
    {
        switch (currentDay)
        {
            case 1:  return day2Scene;
            case 2:  return day3Scene;
            default: return day1Scene;
        }
    }

    private bool IsEasterEggCombo(string combo)
    {
        return IsEasterEgg1(combo) || IsEasterEgg2(combo);
    }

    private bool IsEasterEgg1(string combo)
    {
        switch (combo)
        {
            // EasterEgg1 combos (8)
            case "Popcorn|Coffee|Song1":
            case "Popcorn|Hot Chocolate|Song2":
            case "Cheesecake|Coffee|Song1":
            case "Cheesecake|Hot Chocolate|Song3":
            case "Dumpling|Coffee|Song2":
            case "Dumpling|Hot Chocolate|Song1":
            case "Burger|Coffee|Song3":
            case "Burger|Hot Chocolate|Song2":
                return true;
            default:
                return false;
        }
    }

    private bool IsEasterEgg2(string combo)
    {
        switch (combo)
        {
            // EasterEgg2 combos (7)
            case "Ice Cream|Coffee|Song1":
            case "Ice Cream|Hot Chocolate|Song3":
            case "Popcorn|Coffee|Song3":
            case "Cheesecake|Coffee|Song2":
            case "Dumpling|Hot Chocolate|Song3":
            case "Burger|Coffee|Song1":
            case "Ice Cream|Hot Chocolate|Song2":
                return true;
            default:
                return false;
        }
    }
}