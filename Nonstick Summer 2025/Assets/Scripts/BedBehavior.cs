/*****************************************************************************
// File Name :          BedBehavior.cs
// Author :             Cade R. Naylor
// Creation Date :      June 20, 2025
// Modified Date :      July 10, 2025
//
// Brief Description :  Handles triggers and setting states with the Bed 

MOVED TO OPENCOMFIRMATIONINTERACTABLE
*****************************************************************************/
using UnityEngine;
using NaughtyAttributes;

public class BedBehavior : MonoBehaviour//, IInteractableObjective
{/*
    [HideInInspector] public bool InteractSuccessful = false;
    private bool _playerHasLeft = false;
    [HideInInspector] public bool BossDefeated = false;
    [SerializeField, Scene] private int _nextSceneIndex;


    /// <summary>
    /// Opens or closes the canvas and handles setting visuals
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        if (BossDefeated)
        {
            InteractSuccessful = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneIndex);
        }
    }

    public void GoToBed()
    {
        if (BossDefeated)
        {
            InteractSuccessful = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneIndex);
        }

        else
        {
            Debug.Log("Boss not defeated.");
        }
    }

    public void SetIsObjective(bool b = false) { }

    public void ClearBlocker()
    {
        BossDefeated = true;
    }

    public void TryBoss() { }
 */
}
   