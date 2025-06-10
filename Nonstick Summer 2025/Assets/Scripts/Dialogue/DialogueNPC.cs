using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class DialogueNPC
{
    public string Dialogue;
    public Image Portrait;

    //for progressing dialogue. depends on the list 
    //public int NextDialogueBox;

    public bool End;

    //private TMP_Text printedDialogue;

}
