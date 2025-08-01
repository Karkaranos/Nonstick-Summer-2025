/*****************************************************************************
* File Name :         DialogueNPC.cs
* Author :            Jay
* Creation Date :     ? 2025
*
* Brief Description :  
* 
*****************************************************************************/

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class DialogueNPC
{
    public string Dialogue;

    [ShowAssetPreview(32,32), Tooltip("Leave null to use the sprite from the last dialogue bubble")]
    public Sprite Portrait;

    [Tooltip("Create a prefab for your sprite/animation and then put it here.")]
    public GameObject AnimatedReaction;

}
