using TMPro;
using UnityEngine;

[System.Serializable]

public class DialogueNPC
{

    public DialogueBranch BranchingDialogue;
    public string Dialogue;
    public Sprite Portrait;

    //for progressing dialogue. depends on the list 
    public int NextDialogueBox;

    public bool End;

    //private TMP_Text printedDialogue;

}
