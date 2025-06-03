using UnityEngine;
using TMPro;

[System.Serializable]

public class DialogueOption
{
    //buttons?
    public DialogueBranch BranchingDialogue;
    public string PlayerDialogue;

    //for progressing dialogue. depends on the list 
    public int NextDialogueBox;

}
