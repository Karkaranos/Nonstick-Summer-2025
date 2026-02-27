using System;
using UnityEngine;

/*
 * Useful functions for ui.
 * Put this script on any canvas to get a bunch of functions to call from buttons.
 */
public class UIUtilityFunctions : MonoBehaviour
{
    /* MOVED TO INTERACTABLEOBJECTBEHAVIOUR
    private static GameObject objRef;
    
    /// <summary>
    /// Gives all intents of the specified emotion
    /// </summary>
    /// <param name="emotion">The emotion to give intents of</param>
    public static void GetEmotion(CardEmotion emotion, GameObject openedFrom)
    {
        objRef = openedFrom;
        DeckManager.PlayerFullDeck.Add(CardData.NewCard(-2, emotion, CardIntention.Expression));
        DeckManager.PlayerFullDeck.Add(CardData.NewCard(-2, emotion, CardIntention.Observation));
        DeckManager.PlayerFullDeck.Add(CardData.NewCard(-2, emotion, CardIntention.Question));

        openedFrom.GetComponent<InteractableObjectBehavior>().GiveCard(emotion);
    }
    */

    public static void CloseCurrentPopup()
    {
        UITransitionManager.CloseMenu();
    }

    public static void CloseCurrentPopupKeepMouse()
    {
        UITransitionManager.CloseMenu(false, false);
    }

    public static void SetObjectiveVis(bool visibility)
    {
        GameManager.ObjectiveReference.SetObjectiveVisibility(visibility);
    }
}
