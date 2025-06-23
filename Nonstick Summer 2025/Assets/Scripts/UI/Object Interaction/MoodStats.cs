/*****************************************************************************
* File Name :         MoodStats.cs
* Author :            Jay
* Creation Date :     June 22, 2025
*
* Brief Description :  The numbers correlating to a player's mood.
* 
*****************************************************************************/
using UnityEngine;

[System.Serializable]

public class MoodStats
{

    [Tooltip("How high can this value go?")]
    public float maxValue = 10;
    [Tooltip("What should the starting value be for this scene? The higher, the more effective!")]
    public float currentValue = 0;

    //TODO: add an additional value that is preserved throughout the entire game??
    //i think that's just the personality value whoops

}
