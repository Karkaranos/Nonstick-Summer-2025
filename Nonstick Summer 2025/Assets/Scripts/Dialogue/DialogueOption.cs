using UnityEngine;
using System;
using TMPro;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

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

    //buttons?
    //[Required]
    //public DialogueBranch BranchingDialogue;

    [ResizableTextArea]
    public string PlayerDialogue;

    public DialogueNPC[] NpcReactionText;

    [Tooltip("How good should the player's relationship be with this NPC in order to progress past this point?")]
    public float RelationshipRequirement;

    private bool showOtherBranch => RelationshipRequirement == 0;

    [AllowNesting]
    [HideIf("showOtherBranch")] [Tooltip("If the player's relationship score with an NPC is too low, they go here.")] public DialogueBranch AlternateBranch;

    [Tooltip("How much this dialogue option changes the character's relationship value.")]
    public float ChangeInRelationshipStatus;

}
