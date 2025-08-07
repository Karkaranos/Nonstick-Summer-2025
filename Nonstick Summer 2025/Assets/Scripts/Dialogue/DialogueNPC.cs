/*****************************************************************************
* File Name :         DialogueNPC.cs
* Author :            Jay
* Creation Date :     ? 2025
*
* Brief Description :  
* 
*****************************************************************************/

using NaughtyAttributes;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]

public class DialogueNPC
{
    public string Dialogue;

    [ShowAssetPreview(32,32), Tooltip("Leave null to use the sprite from the last dialogue bubble")]
    public Sprite Portrait;

    [Tooltip("Create a prefab for your sprite/animation and then put it here.")]
    public GameObject AnimatedReaction;

    List<string> options = new List<string> { "Silent", "Happy", "Sad", "Angry", "Neutral" };

    [Dropdown("options")] public string AudioResponse;

}
