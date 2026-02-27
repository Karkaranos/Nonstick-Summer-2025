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
    [ResizableTextArea, AllowNesting]
    public string Dialogue;

    [ShowAssetPreview(32,32), Tooltip("Leave null to use the sprite from the last dialogue bubble")]
    public Sprite Portrait;

    [Tooltip("Create a prefab for your sprite/animation and then put it here.")]
    public GameObject AnimatedReaction;

    [Foldout("Advanced")] public string AudioResponse;
    [Foldout("Advanced")] public bool HasAdvancedSignal;
    [Tooltip("Under certain values, the AdvancedSignal will cause certain events to trigger mid-dialogue")]
    [Foldout("Advanced"), ShowIf(nameof(HasAdvancedSignal)), AllowNesting] public AdvancedSignal AdvancedSignal;
}

public enum AdvancedSignal
{
    None, ShakeEnergyBar,
}

public enum AudioResponseType
{
    Silent, Happy, Sad, Angry, Neutral
}
