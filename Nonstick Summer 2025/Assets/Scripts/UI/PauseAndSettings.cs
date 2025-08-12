/*************************************************
Author Names :          Cade, Naylor
Date Created :          June 20, 2025
Date Modified :         July 29, 2025
Brief Description :     Handles UI functionality for pause and settings menu
***************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class PauseAndSettings : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _pause;
    [HideInInspector] public bool OpenedFromPause;  // Reopens the pause menu if the setting menu is closed and it had been opened from pause
    private static OpenPause _openPauseReference;

    [SerializeField] private Slider _mouseSensitivitySlider;
    [SerializeField] private Slider _sfxVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField, Required] private Button settingsBackButton;

    private AudioManager am;

    private void Start()
    {
        am = FindFirstObjectByType<AudioManager>();

        if (_mouseSensitivitySlider != null)
        {
            _mouseSensitivitySlider.value = FindFirstObjectByType<PlayerCamera>().Sensitivity;
            _mouseSensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
        }

        if (_sfxVolume != null)
        {
            _sfxVolume.value = am.sfxVolume;
            _sfxVolume.onValueChanged.AddListener(UpdateSFXVolume);
        }

        if (_musicVolume != null)
        {
            _musicVolume.value = am.musicVolume;
            _musicVolume.onValueChanged.AddListener(UpdateMusicVolume);
        }

        settingsBackButton.onClick.AddListener(CloseSettings);
    }

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
        UITransitionManager.CloseMenu();
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

    public void OpenControlsFromPause()
    {
        GameObject controlRef = UITransitionManager.OpenMenu(_controls);
        controlRef.GetComponent<PauseAndSettings>().OpenedFromPause = true;
    }

    public void OpenControls()
    {
        GameObject controlRef = UITransitionManager.OpenMenu(_controls);
    }

    /// <summary>
    /// Closes the settings and handles cases depending on whether this was opened from pause or not
    /// </summary>
    public void CloseSettings()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {

            //If the line in else is not in an else, the cursor is hidden when returning to the pause menu
            if (OpenedFromPause)
            {
                UITransitionManager.CloseMenu(false, false);
                print("sjhhlgfdshgjfdsjhgshgjs");
                //i know static would help but it would set off a chain of making things static
                //and you couldn't assign pauseMenu in the inspector when i tried. if people have thoughts of a better way
                //please let me know!

                if (_openPauseReference == null)
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
        else
        {
            FindFirstObjectByType<MainMenu>().CloseMenu();
        }
    }

    public void UpdateMouseSensitivity(float val)
    {
        FindFirstObjectByType<PlayerCamera>().UpdateSensitivity(val);
    }

    public void UpdateSFXVolume(float val)
    {
        am.sfxVolume = val;
        am.UpdateVolume();
    }

    public void UpdateMusicVolume(float val)
    {
        am.musicVolume = val;
        am.UpdateVolume();
    }



}
