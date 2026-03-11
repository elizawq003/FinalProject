using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset_First : MonoBehaviour
{
  public void RestartGame()
{
        // GameStateManager.Instance.ResetAll(); // reset all game progress too
        //SceneManager.LoadScene("MainMenu");

        // Kill the persistent MusicManager so it doesn't interfere
        if (MusicManager.Instance != null)
        {
            Destroy(MusicManager.Instance.gameObject);
        }

        GameStateManager.Instance.ResetAll();
        SaveManager.DeleteSave();
        SceneManager.LoadScene("OpeningScene");
    }
}