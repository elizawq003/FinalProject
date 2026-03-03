using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    //public static SingletonExample Instance { get; private set; }

    /* in updated: 
     * if (pauseManager.instance isPaused)
     *  return;
     */
    /** for ink dialog and audio file , use similar logic to pause
     * for ink file, edit the ink manager to respond to the pause
     */

   bool isPaused = false;
   public GameObject pauseMenuUI;
   public Button pauseButton;

    private void Start()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Press Escape to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);
        }

        else
        {
            Time.timeScale = 0;
            isPaused = true;

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);

        }
          
    }

    public bool IsPaused()
    {
        return isPaused;
    }




}
