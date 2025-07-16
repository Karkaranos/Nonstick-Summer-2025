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

    [SerializeField]
    [Label("NPC Reaction")] private DialogueNPC[] NpcReactionText;

    //buttons?
    [Required]
    public DialogueBranch BranchingDialogue;

    [HideInInspector] public DialogueNPC[] CombinedDialogue;

    [ResizableTextArea]
    public string PlayerDialogue;

    [Tooltip("How good should the player's relationship be with this NPC in order to progress past this point?")]
    public float RelationshipRequirement;

    private bool showOtherBranch => RelationshipRequirement == 0;

    [AllowNesting]
    [HideIf("showOtherBranch")] [Tooltip("If the player's relationship score with an NPC is too low, they go here.")] public DialogueBranch AlternateBranch;

    [Tooltip("How much this dialogue option changes the character's relationship value.")]
    public float ChangeInRelationshipStatus;


    public void SetNextBranchReaction()
    {

        CombinedDialogue = new DialogueNPC[NpcReactionText.Length + BranchingDialogue.dialogue.Length];

        for(int i = 0; i < NpcReactionText.Length; i++)
        {

            CombinedDialogue[i] = NpcReactionText[i];

        }
        for(int i = 0; i < BranchingDialogue.dialogue.Length; i++)
        {

            CombinedDialogue[i + NpcReactionText.Length] = BranchingDialogue.dialogue[i];

        }


    }

}
