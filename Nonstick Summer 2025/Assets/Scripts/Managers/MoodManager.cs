/*****************************************************************************
* File Name :         MoodManager.cs
* Author :            Jay
* Creation Date :     June 22, 2025
*
* Brief Description :  Keeps track of the player's mood throughout a level.
* 
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MoodManager
{

    [HideInInspector] public static Dictionary<CardEmotion, MoodStats> emotions = new Dictionary<CardEmotion, MoodStats>();

}
