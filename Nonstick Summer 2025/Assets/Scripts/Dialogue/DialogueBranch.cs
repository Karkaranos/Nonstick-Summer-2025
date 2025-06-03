using UnityEngine;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "DialogueBranch", menuName = "Scriptable Objects/DialogueBranch")]

public class DialogueBranch : ScriptableObject
{

    //assign per npc/"moment"?? swap between branches or continue old ones???

    [SerializeField]
    DialogueOption Option1, Option2, Option3, Option4, Option5,
        Option6, Option7, Option8, Option9;

    private static DialogueOption blank;

    [SerializeField]
    [Label("NPC Dialogue")] DialogueNPC[] dialogue;

    private int currentText;

    public DialogueOption ReturnDialogueOption(CardData card)
    {

        switch ((card.Intention, card.Emotion))
        {

            case (CardIntention.Intention1, CardEmotion.Yellow):
                if (Option1 != null)
                {
                    return Option1;
                }
                else return blank;
            case (CardIntention.Intention1, CardEmotion.Red):
                if (Option2 != null)
                {
                    return Option2;
                }
                else return blank;
            case (CardIntention.Intention1, CardEmotion.Blue):
                if (Option3 != null)
                {
                    return Option3;
                }
                else return blank;
            case (CardIntention.Intention2, CardEmotion.Yellow):
                if (Option4 != null)
                {
                    return Option4;
                }
                else return blank;
            case (CardIntention.Intention2, CardEmotion.Red):
                if (Option5 != null)
                {
                    return Option5;
                }
                else return blank;
            case (CardIntention.Intention2, CardEmotion.Blue):
                if (Option6 != null)
                {
                    return Option6;
                }
                else return blank;
            case (CardIntention.Intention3, CardEmotion.Yellow):
                if (Option7 != null)
                {
                    return Option7;
                }
                else return blank;
            case (CardIntention.Intention3, CardEmotion.Red):
                if (Option8 != null)
                {
                    return Option8;
                }
                else return blank;
            case (CardIntention.Intention3, CardEmotion.Blue):
                if (Option9 != null)
                {
                    return Option9;
                }
                else return blank;
            default:
                Debug.Log("Nothing here!");
                return blank;
        }

    }

}
