using System;
using UnityEngine;

/*
 * Useful functions for ui.
 * Put this script on any canvas to get a bunch of functions to call from buttons.
 */
public class UIUtilityFunctions : MonoBehaviour
{
    private static GameObject objRef;
    /// <summary>
    /// Gives all intents of the specified emotion
    /// </summary>
    /// <param name="emotion">The emotion to give intents of</param>
    public static void GetEmotion(CardEmotion emotion, GameObject openedFrom)
    {
        objRef = openedFrom;
        DialogueManager.PlayerHand.Add(CardData.NewCard(-2, emotion, CardIntention.Expression));
        DialogueManager.PlayerHand.Add(CardData.NewCard(-2, emotion, CardIntention.Observation));
        DialogueManager.PlayerHand.Add(CardData.NewCard(-2, emotion, CardIntention.Question));

        openedFrom.GetComponent<InteractableObjectBehavior>().GiveCard(emotion);
    }

    public static void CloseCurrentPopup()
    {
        UITransitionManager.CloseMenu();
    }

    public static void SetObjectiveVis(bool visibility)
    {

        GameManager.ObjectiveReference.SetObjectiveVisibility(visibility);
    }
}
