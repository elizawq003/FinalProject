using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // Tracks which day the player is on (1, 2, or 3)
    public int CurrentDay = 1;


    // Ending variables
    public int Dream;
    public int Achievement;
    public int Stability;
    public int Friend;

    // Dream sub-indices
    public int DOL;
    public int DOR;
    public int DOT;
    public int DOS;

    // NPC Affinity (carried across days)
    public int TeenAffinity;
    public int AdultAffinity;
    public int KidAffinity;

    // Food/Song choices
    public string Dessert;
    public string Song;
    public string Drink;

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

    // Call at END of each day scene to snapshot Ink variables.

    public void SyncFromInk(Ink.Runtime.Story story)
    {
        Dream = (int)story.variablesState["Dream"];
        Achievement = (int)story.variablesState["Achievement"];
        Stability = (int)story.variablesState["Stability"];
        Friend = (int)story.variablesState["Friend"];
        DOL = (int)story.variablesState["DOL"];
        DOR = (int)story.variablesState["DOR"];
        DOT = (int)story.variablesState["DOT"];
        DOS = (int)story.variablesState["DOS"];
        TeenAffinity = (int)story.variablesState["TeenAffinity"];
        AdultAffinity = (int)story.variablesState["AdultAffinity"];
        KidAffinity = (int)story.variablesState["KidAffinity"];

        // Advance to next day
        CurrentDay++;
        Debug.Log($"Day complete. Next day: {CurrentDay}");
    }

    // Call at START of Day 2/3 to inject carried-over values.
    public void SyncToInk(Ink.Runtime.Story story)
    {
        story.variablesState["Dream"] = Dream;
        story.variablesState["Achievement"] = Achievement;
        story.variablesState["Stability"] = Stability;
        story.variablesState["Friend"] = Friend;
        story.variablesState["DOL"] = DOL;
        story.variablesState["DOR"] = DOR;
        story.variablesState["DOT"] = DOT;
        story.variablesState["DOS"] = DOS;
        story.variablesState["TeenAffinity"] = TeenAffinity;
        story.variablesState["AdultAffinity"] = AdultAffinity;
        story.variablesState["KidAffinity"] = KidAffinity;


    }

    // Returns the scene name for the current day.
    public string GetCurrentDayScene()
    {
        switch (CurrentDay)
        {
            case 1: return "DayOne";
            case 2: return "DayTwo";
            case 3: return "DayThree";
            default: return "Ending";
        }
    }


    // Determines the final ending. Call after Day 3.
    public string DetermineEnding()
    {
        int maxDream = Mathf.Max(DOL, DOR, DOT, DOS);

        int[] scores = { Dream, Achievement, Stability, Friend };
        string[] names = { "Dream", "Achievement", "Stability", "Friend" };

        int bestIndex = 0;
        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > scores[bestIndex])
                bestIndex = i;
        }


        if (names[bestIndex] == "Dream")
        {
            if (maxDream == DOL) return "Dream of Light";
            if (maxDream == DOR) return "Dream of Research";
            if (maxDream == DOT) return "Dream of Travel";
            if (maxDream == DOS) return "Dream of Story";
            return "Dream";
        }

        return names[bestIndex];
    }

    // for reloading
    public void ResetAll()
    {
        CurrentDay = 1;
        Dream = 0;
        Achievement = 0;
        Stability = 0;
        Friend = 0;
        DOL = 0;
        DOR = 0;
        DOT = 0;
        DOS = 0;
        TeenAffinity = 0;
        AdultAffinity = 0;
        KidAffinity = 0;
        Dessert = "";
        Song = "";
        Drink = "";
    }

}
