/*****************************************************************************
* File Name :         WaveAnimation.cs
* Author :            Toby
* Creation Date :     8/11/2025
*
* Brief Description : Animates all children of this gameobject.
* This script is NOT used for decks/hands. See the PositionAnimator scripts for that
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    [InfoBox("Animates the children of this gameobject")]

    [SerializeField] float height=5;
    [SerializeField] float speed=0.75f;
    [SerializeField] float width = 1;

    RectTransform[] children;
    Dictionary<RectTransform, Vector3> originalPositions = new Dictionary<RectTransform, Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        children = GetComponentsInChildren<RectTransform>();
        foreach (var child in children)
            originalPositions[child] = child.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < children.Length; i++)
        {
            var child = children[i];
            child.localPosition = originalPositions[child] + new Vector3(0, Mathf.Sin((Time.time + (i* width)) * speed) * height);
        }
    }
}
