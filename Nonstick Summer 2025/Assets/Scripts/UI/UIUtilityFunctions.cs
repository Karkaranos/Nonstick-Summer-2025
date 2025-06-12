using System;
using UnityEngine;

/*
 * Useful functions for ui.
 * Put this script on any canvas to get a bunch of functions to call from buttons.
 */
public class UIUtilityFunctions : MonoBehaviour
{
    /// <summary>
    /// Give player n x card.
    /// Call this function multiple times on a button call for multiple different cards.
    /// </summary>
    public static void GetCard(CardData card)
    {
        DialogueManager.PlayerHand.Add(card);
        if (UITransitionManager.WorldObjectReference != null)
        {
            UITransitionManager.WorldObjectReference.GetComponent<OpenCanvasInteractable>().GiveCard();
        }
    }

    public static void CloseCurrentPopup()
    {
        UITransitionManager.CloseMenu();
    }
}
