using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit_Reload : MonoBehaviour
{
  public void RestartGame()
{
    GameStateManager.Instance.ResetAll(); // reset all game progress too
    SceneManager.LoadScene("MainMenu");
}
}