using UnityEngine;
using System;
using TMPro;
using NaughtyAttributes;

/*****************************************************************************
* File Name :         DialogueOption.cs
* Author :            Jay, Sky
* Creation Date :     ???
*
* Brief Description : 
*
* TODO:
* 
* 
*****************************************************************************/
[System.Serializable]

public class DialogueOption
{

    [SerializeField, AllowNesting]
    [Label("NPC Reaction")] private DialogueNPC[] NpcReactionText;

    [HideInInspector] public DialogueNPC[] CombinedDialogue;

    [ResizableTextArea]
    public string PlayerDialogue;

    [Tooltip("Check this off if this should lead to one of multiple branches based off of the player's relationship with the NPC!")] 
    public bool RelationshipCheckRequired;

    //TODO: documentation

    [AllowNesting]
    [ShowIf("RelationshipCheckRequired")]
    [MinMaxSlider(0, 100)]
    [Tooltip("Set a range! Read each tooltip to figure out what score leads to which branch.")]
    public Vector2 RelationshipRange;

    [AllowNesting]
    [Tooltip("The player has exceeded the range required to at least progress through the conversation OR a relationship check was not required.")]
    [Required] public DialogueBranch BranchingDialogueHigh;

    private bool showBranchingDialogueNeutral => RelationshipRange.y < 100 && RelationshipCheckRequired;

    [AllowNesting]
    [ShowIf("showBranchingDialogueNeutral")]
    [Tooltip("The player is within the range to at least continue the conversation, but not to get the best branch.")]
    [Required] public DialogueBranch BranchingDialogueNeutral;

    private bool showBranchingDialogueLow => RelationshipRange.x > 0 && RelationshipCheckRequired;

    [AllowNesting]
    [ShowIf("showBranchingDialogueLow")]
    [Tooltip("The player has not met the range required to further converse with this NPC.")]
    [Required] public DialogueBranch BranchingDialogueLow;

    [Tooltip("How much this dialogue option changes the character's relationship value.")]
    [SerializeField] public float ChangeInRelationshipStatus;

    public void SetNextBranchReaction(DialogueBranch branch)
    {

        CombinedDialogue = new DialogueNPC[(NpcReactionText.Length + branch.dialogue.Length)];

        for(int i = 0; i < NpcReactionText.Length; i++)
        {

            CombinedDialogue[i] = NpcReactionText[i];

        }
        for(int i = 0; i < branch.dialogue.Length; i++)
        {

            CombinedDialogue[i + NpcReactionText.Length] = branch.dialogue[i];

        }

    }

}
