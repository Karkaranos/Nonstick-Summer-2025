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

    //tried hiding this with a bool but w/e
    //also?? this might not be a float in the future???
    //depends on toby's changes to measuring the player's relationship with an npc
    [Tooltip("How good should the player's relationship be with this NPC in order to progress past this point?")]
    public float RelationshipRequirement;

}
