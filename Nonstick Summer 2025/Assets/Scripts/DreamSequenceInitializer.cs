using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
/*****************************************************************************
* File Name :         DreamSequence.cs
* Author :            Sky
* Creation Date :     July 11, 2025
*
* Brief Description :  Controls canvas anD actions During Dream Sequence.
* 
*****************************************************************************/
public class DreamSequenceInitializer : MonoBehaviour
{
    [Header("Required Attributes")] [Required]
    public GameObject CanvasToOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UITransitionManager.OpenMenu(CanvasToOpen);
    }
}