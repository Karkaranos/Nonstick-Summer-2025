/*****************************************************************************
* File Name :         CardData.cs
* Author :            Sky, Cade
* Creation Date :     July 10, 2025
*
* Brief Description : Controls confirmation UI buttons for going to sleep. Lets the user know if they cannot sleep yet.
* 
*****************************************************************************/

using UnityEngine;
using TMPro;

public class BedInteractionPopupCanvas : MonoBehaviour
{
    [HideInInspector]
    public OpenConfirmationInteractable Bed;
    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_Text statement;
    [SerializeField] private GameObject canSleepButtons;
    [SerializeField] private GameObject cannotSleepButtons;
    public EndType SceneTransitionType;

    public enum EndType
    {
        DOOR, BED
    };
    public void Start()
    {
        if (SceneTransitionType == EndType.BED)
        {
            statement.text = "It's your bed.";
            message.text = (Bed.BossDefeated ? "Would you like to sleep?\n(This will end the moment.)" : "You cannot sleep yet");
        }
        else
        {
            statement.text = "It's your front door.";
            message.text = (Bed.BossDefeated ? "Would you like to leave?\n(This will end the moment.)" : "You cannot leave yet");
        }
        canSleepButtons.SetActive(Bed.BossDefeated);
        cannotSleepButtons.SetActive(!Bed.BossDefeated);
    }

    public void OnYesPressed()
    {
        if (Bed.BossDefeated)
        {
            Bed.InteractSuccessful = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(Bed.NextSceneIndex);
            UITransitionManager.CloseMenu();
        }
        else
        {
            Debug.Log("Boss not defeated.");
        }
    }

    public void OnNoPressed()
    {
        UITransitionManager.CloseMenu();
    }
}
