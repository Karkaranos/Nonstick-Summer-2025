/*****************************************************************************
* File Name :         CardData.cs
* Author :            Sky
* Creation Date :     July 10, 2025
*
* Brief Description : Controls confirmation UI buttons for going to sleep.
* 
*****************************************************************************/

using UnityEngine;

public class BedInteractionPopupCanvas : MonoBehaviour
{
    [HideInInspector]
    public OpenConfirmationInteractable Bed;

    public void OnYesPressed()
    {
        if (Bed.BossDefeated)
        {
            Bed.InteractSuccessful = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(Bed.NextSceneIndex);
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
