using NaughtyAttributes;
using UnityEngine;
/*****************************************************************************
* File Name :         MoodStats.cs
* Author :            Jay
* Creation Date :     June 22, 2025
*
* Brief Description :  The numbers correlating to a player's mood.
* 
*****************************************************************************/

[System.Serializable]

public class MoodStats
{

    //TODO: add modifiers depending on CardIntention??
    [Tooltip("What should the default energy cost be for a card with this emotion? This should either be 0 or a negative value.")]
    //[AllowNesting] [MaxValue(0)]
    public int energyCostOffset;

    [Tooltip("What is the most energy that a card should EVER cost? Should be a negative value.")]
    //[AllowNesting] [MaxValue(-1)]
    public int maxEnergyCost;


    //keeps track of how many times an emotion has been expressed by the player
    [HideInInspector] public int expressionValue = 0;


    [Tooltip("How often would a player have to express this emotion before this emotion becomes easier to play? The lower the value, the more effective.")]
    [AllowNesting] [MinValue(1)]
    public int intervalBetweenADecreasedCost;

    [Tooltip("How often would a player have to express this emotion before other emotions become harder to play? The lower the value, the more effective.")]
    [AllowNesting] [MinValue(1)]
    public int intervalBetweenIncreasedCosts;


    //TODO: add an additional value that is preserved throughout the entire game??
    //i think that's just the personality value whoops

}
