using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CanvasInteractionBehavior : MonoBehaviour
{
    [SerializeField] [Required] private GameObject interactPrompt;
    [SerializeField] [Required] private GameObject talkPrompt;
    public static Action ShowInteractUI;
    public static Action HideInteractUI;

    public static Action ShowTalkUI;
    public static Action HideTalkUI;

    private void Awake()
    {
        ShowInteractUI += EnableInteractUI;
        HideInteractUI += DisableInteractionUI;

        ShowTalkUI += EnableTalkUI;
        HideTalkUI += DisableTalkUI;

        DisableInteractionUI();
    }

    /// <summary>
    /// General method to show the interactable prompt
    /// </summary>
    private void EnableInteractUI()
    {
        if (talkPrompt.activeSelf)
        {
            return;
        }

        interactPrompt?.SetActive(true);
    }

    /// <summary>
    /// <summary>
    /// General method to hide the interactable prompt
    /// </summary>
    private void DisableInteractionUI()
    {
        interactPrompt?.SetActive(false);
    }


    /// <summary>
    /// General method to show the interactable prompt
    /// </summary>
    private void EnableTalkUI()
    {
        talkPrompt?.SetActive(true);
    }

    /// <summary>
    /// <summary>
    /// General method to hide the interactable prompt
    /// </summary>
    private void DisableTalkUI()
    {
        talkPrompt?.SetActive(false);
    }

    private void OnDisable()
    {
        ShowInteractUI -= EnableInteractUI;
        HideInteractUI -= DisableInteractionUI;

        ShowTalkUI -= EnableTalkUI;
        HideTalkUI -= DisableTalkUI;
    }
}
