/*************************************************
Author Names :          Cade, Naylor
Date Created :          June 20, 2025
Date Modified :         June 20, 2025
Brief Description :     Handles UI functionality for pause and settings menu
***************************************************/
using UnityEngine;

public class PauseAndSettings : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [HideInInspector] public bool OpenedFromPause;  // Reopens the pause menu if the setting menu is closed and it had been opened from pause
    private static OpenPause _openPauseReference;

    /// <summary>
    /// Resume normal gameplay and close the pause menu
    /// </summary>
    public void Resume()
    {
        UITransitionManager.CloseMenu();
    }

    /// <summary>
    /// Loads the named scene
    /// </summary>
    /// <param name="scene">the scene index to go to</param>
    public void LoadScene(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Opens the settings menu
    /// </summary>
    public void OpenSettings()
    {
        GameObject settingRef = UITransitionManager.OpenMenu(_settings);
        settingRef.GetComponent<PauseAndSettings>().OpenedFromPause = true;

    }

    /// <summary>
    /// Closes the settings and handles cases depending on whether this was opened from pause or not
    /// </summary>
    public void CloseSettings()
    {
        //If the line in else is not in an else, the cursor is hidden when returning to the pause menu
        if(OpenedFromPause)
        {
            //i know static would help but it would set off a chain of making things static
            //and you couldn't assign pauseMenu in the inspector when i tried. if people have thoughts of a better way
            //please let me know!

            if(_openPauseReference == null)
            {
                _openPauseReference = FindFirstObjectByType<OpenPause>();
            }

            _openPauseReference.PausePressed();
        }
        else
        {
            UITransitionManager.CloseMenu();
        }
    }



}
