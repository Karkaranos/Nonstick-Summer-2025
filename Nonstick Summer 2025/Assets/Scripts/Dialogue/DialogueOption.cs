using UnityEngine;
using TMPro;
using NaughtyAttributes;

[System.Serializable]

public class DialogueOption
{
    //buttons?
    public DialogueBranch BranchingDialogue;
    [ResizableTextArea] public string PlayerDialogue;

    //for progressing dialogue. depends on the list 
    //public int NextDialogueBox;

}
