using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class DialogueNPC
{
    public string Dialogue;

    [ShowAssetPreview(32,32)]
    public Sprite Portrait;

    //for progressing dialogue. depends on the list 
    //public int NextDialogueBox;

    //public bool End;

    //private TMP_Text printedDialogue;

}
