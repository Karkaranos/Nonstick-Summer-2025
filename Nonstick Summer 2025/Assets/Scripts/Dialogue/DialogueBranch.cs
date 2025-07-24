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
    [Label("NPC Dialogue")] public DialogueNPC[] dialogue;

    [SerializeField]
    [BoxGroup("Dialogue Options")][HideIf("End")] public DialogueOption Charming_Expression, Assertive_Expression, Sappy_Expression, Charming_Observation, Assertive_Observation,
        Sappy_Observation, Charming_Question, Assertive_Question, Sappy_Question;

    [SerializeField]
    [BoxGroup("Dialogue Options")] [HideIf("End")] private DialogueOption Silent;


    public bool End;

    private static DialogueOption blank;

    public DialogueOption ReturnDialogueOption(CardData card)
    {
        if (card == null)
        {
            Debug.Log("Playing silent");
            return Silent ?? blank;
        }

        switch ((card.Intention, card.Emotion))
        {

            case (CardIntention.Expression, CardEmotion.Charming):
                if (Charming_Expression != null)
                {
                    return Charming_Expression;
                }
                else return blank;
            case (CardIntention.Expression, CardEmotion.Assertive):
                if (Assertive_Expression != null)
                {
                    return Assertive_Expression;
                }
                else return blank;
            case (CardIntention.Expression, CardEmotion.Sappy):
                if (Sappy_Expression != null)
                {
                    return Sappy_Expression;
                }
                else return blank;
            case (CardIntention.Observation, CardEmotion.Charming):
                if (Charming_Observation != null)
                {
                    return Charming_Observation;
                }
                else return blank;
            case (CardIntention.Observation, CardEmotion.Assertive):
                if (Assertive_Observation != null)
                {
                    return Assertive_Observation;
                }
                else return blank;
            case (CardIntention.Observation, CardEmotion.Sappy):
                if (Sappy_Observation != null)
                {
                    return Sappy_Observation;
                }
                else return blank;
            case (CardIntention.Question, CardEmotion.Charming):
                if (Charming_Question != null)
                {
                    return Charming_Question;
                }
                else return blank;
            case (CardIntention.Question, CardEmotion.Assertive):
                if (Assertive_Question != null)
                {
                    return Assertive_Question;
                }
                else return blank;
            case (CardIntention.Question, CardEmotion.Sappy):
                if (Sappy_Question != null)
                {
                    return Sappy_Question;
                }
                else return blank;
            default:
                Debug.LogError($"Can't match played card {card.Emotion.ToString()},{card.Intention.ToString()} to result");
                return blank;
        }

    }

}
