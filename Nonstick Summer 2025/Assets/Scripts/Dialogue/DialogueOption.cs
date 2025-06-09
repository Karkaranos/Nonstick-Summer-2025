using UnityEngine;
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
    //buttons?
    public DialogueBranch BranchingDialogue;

    [ResizableTextArea]
    public string PlayerDialogue;

    [Tooltip("How much this dialogue option changes the character's relationship value.")]
    public float ChangeInRelationshipStatus;

}
