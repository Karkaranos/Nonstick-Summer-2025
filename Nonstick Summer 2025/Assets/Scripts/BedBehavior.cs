/*****************************************************************************
// File Name :          BedBehavior.cs
// Author :             Cade R. Naylor
// Creation Date :      June 20, 2025
// Modified Date :      June 21, 2025
//
// Brief Description :  Handles triggers and setting states with the Bed 
*****************************************************************************/
using UnityEngine;
using NaughtyAttributes;

public class BedBehavior : MonoBehaviour, IInteractable
{
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneIndex);
        }
    }

}
