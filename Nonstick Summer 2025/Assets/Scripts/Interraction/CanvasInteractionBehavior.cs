using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInteractionBehavior : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt;
    public static Action ShowInteractUI;
    public static Action HideInteractUI;

    private void Awake()
    {
        ShowInteractUI += EnableInteractUI;
        HideInteractUI += DisableInteractUI;
    }

    /// <summary>
    /// General method to show the interactable prompt
    /// </summary>
    private void EnableInteractUI()
    {
        interactPrompt.SetActive(true);
    }

    /// <summary>
    /// <summary>
    /// General method to hide the interactable prompt
    /// </summary>
    private void DisableInteractUI()
    {
        interactPrompt.SetActive(false);
    }

    private void OnDisable()
    {
        ShowInteractUI -= EnableInteractUI;
        HideInteractUI -= DisableInteractUI;
    }
}
