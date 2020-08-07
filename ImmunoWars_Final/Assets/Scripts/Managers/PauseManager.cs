using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : GenericSingletonClass<PauseManager>
{
    /// <summary>
    /// Pauses and unpauses the game
    /// </summary>

    public static bool isPaused = false;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
    }
}
