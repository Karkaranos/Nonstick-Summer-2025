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
    /// Call this function multiple times on a button call for multiple different _cards.
    /// </summary>
    public static void GetCard(CardData card)
    {
        Debug.LogWarning("Implement giving player cards later");
    }

    public static void CloseCurrentPopup()
    {
        UITransitionManager.CloseMenu();
    }
}
