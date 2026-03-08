using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    private const string SAVE_EXISTS_KEY = "SaveExists";
    private const string DAY_KEY = "CurrentDay";
    private const string DREAM_KEY = "Dream";
    private const string ACHIEVEMENT_KEY = "Achievement";
    private const string STABILITY_KEY = "Stability";
    private const string FRIEND_KEY = "Friend";
    private const string DOL_KEY = "DOL";
    private const string DOR_KEY = "DOR";
    private const string DOT_KEY = "DOT";
    private const string DOS_KEY = "DOS";
    private const string TEEN_AFFINITY_KEY = "TeenAffinity";
    private const string ADULT_AFFINITY_KEY = "AdultAffinity";
    private const string KID_AFFINITY_KEY = "KidAffinity";
    private const string DESSERT_KEY = "Dessert";
    private const string SONG_KEY = "Song";
    private const string DRINK_KEY = "Drink";

    // Saves current GameStateManager values to PlayerPrefs.
    public static void Save()
    {
        GameStateManager gs = GameStateManager.Instance;

        PlayerPrefs.SetInt(SAVE_EXISTS_KEY, 1);
        PlayerPrefs.SetInt(DAY_KEY, gs.CurrentDay);
        PlayerPrefs.SetInt(DREAM_KEY, gs.Dream);
        PlayerPrefs.SetInt(ACHIEVEMENT_KEY, gs.Achievement);
        PlayerPrefs.SetInt(STABILITY_KEY, gs.Stability);
        PlayerPrefs.SetInt(FRIEND_KEY, gs.Friend);
        PlayerPrefs.SetInt(DOL_KEY, gs.DOL);
        PlayerPrefs.SetInt(DOR_KEY, gs.DOR);
        PlayerPrefs.SetInt(DOT_KEY, gs.DOT);
        PlayerPrefs.SetInt(DOS_KEY, gs.DOS);
        PlayerPrefs.SetInt(TEEN_AFFINITY_KEY, gs.TeenAffinity);
        PlayerPrefs.SetInt(ADULT_AFFINITY_KEY, gs.AdultAffinity);
        PlayerPrefs.SetInt(KID_AFFINITY_KEY, gs.KidAffinity);
        PlayerPrefs.SetString(DESSERT_KEY, gs.Dessert);
        PlayerPrefs.SetString(SONG_KEY, gs.Song);
        PlayerPrefs.SetString(DRINK_KEY, gs.Drink);

        PlayerPrefs.Save();
        Debug.Log($"Game saved. Current day: {gs.CurrentDay}");
    }

    // Loads saved values into GameStateManager.
    public static void Load()
    {
        if (!HasSave()) return;

        GameStateManager gs = GameStateManager.Instance;

        gs.CurrentDay = PlayerPrefs.GetInt(DAY_KEY, 1);
        gs.Dream = PlayerPrefs.GetInt(DREAM_KEY, 0);
        gs.Achievement = PlayerPrefs.GetInt(ACHIEVEMENT_KEY, 0);
        gs.Stability = PlayerPrefs.GetInt(STABILITY_KEY, 0);
        gs.Friend = PlayerPrefs.GetInt(FRIEND_KEY, 0);
        gs.DOL = PlayerPrefs.GetInt(DOL_KEY, 0);
        gs.DOR = PlayerPrefs.GetInt(DOR_KEY, 0);
        gs.DOT = PlayerPrefs.GetInt(DOT_KEY, 0);
        gs.DOS = PlayerPrefs.GetInt(DOS_KEY, 0);
        gs.TeenAffinity = PlayerPrefs.GetInt(TEEN_AFFINITY_KEY, 0);
        gs.AdultAffinity = PlayerPrefs.GetInt(ADULT_AFFINITY_KEY, 0);
        gs.KidAffinity = PlayerPrefs.GetInt(KID_AFFINITY_KEY, 0);
        gs.Dessert = PlayerPrefs.GetString(DESSERT_KEY, "");
        gs.Song = PlayerPrefs.GetString(SONG_KEY, "");
        gs.Drink = PlayerPrefs.GetString(DRINK_KEY, "");

        Debug.Log($"Game loaded. Current day: {gs.CurrentDay}");
    }

    // Returns true if a save file exists.
    public static bool HasSave()
    {
        return PlayerPrefs.GetInt(SAVE_EXISTS_KEY, 0) == 1;
    }

    /// <summary>
    /// Deletes the save data.
    /// </summary>
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save data deleted.");
    }
}

