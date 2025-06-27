using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
/*****************************************************************************
* File Name :         MoodManager.cs
* Author :            Jay
* Creation Date :     June 22, 2025
*
* Brief Description :  Keeps track of the player's mood throughout a level.
* 
*****************************************************************************/


//thanks sky for the references

//actually not sure that this should be called from GameManager, since the player's mood is varying from level to level
//like how the player character should be initially moodier as a teenager than they are as a toddler etc etc
//this could change easily though idk
public class MoodManager: MonoBehaviour
{

    [HideInInspector] public static Dictionary<CardEmotion, MoodStats> emotions = new Dictionary<CardEmotion, MoodStats>();

    public MoodManager MoodManagerReference;

    [Foldout("Mood Stats")][SerializeField] private MoodStats charmingStartingValues;
    [Foldout("Mood Stats")][SerializeField] private MoodStats assertiveStartingValues;
    [Foldout("Mood Stats")][SerializeField] private MoodStats sappyStartingValues;

    public MoodManager(MoodStats charmingStartingValues, MoodStats assertiveStartingValues, MoodStats sappyStartingValues)
    {

        emotions.Add(CardEmotion.Charming, charmingStartingValues);
        emotions.Add(CardEmotion.Assertive, assertiveStartingValues);
        emotions.Add(CardEmotion.Sappy, sappyStartingValues);

    }


    void Start()
    {

        MoodManagerReference = MoodManagerReference ?? new MoodManager(charmingStartingValues, assertiveStartingValues, sappyStartingValues);

    }
    /// <summary>
    /// updates the energy costs of cards based on emotions expressed
    /// </summary>
    /// <param name="emotion">the emotion expressed by the player</param>
    public static void UpdateMood(CardEmotion emotion)
    {
        if(!emotions.ContainsKey(emotion))
        {
            Debug.LogError(emotion.ToString());
            return;
        }
        emotions[emotion].expressionValue += 1;

        for (int i = 0; i < emotions[emotion].expressionValue; i++)
        {

            if(i % emotions[emotion].intervalBetweenADecreasedCost == 0 & emotions[emotion].baseEnergyCost > 0)
            {

                emotions[emotion].baseEnergyCost += 1;

            }

            if(i % emotions[emotion].intervalBetweenIncreasedCosts == 0)
            {

                foreach(var key in emotions.Keys)
                {

                    if(key != emotion & emotions[key].baseEnergyCost >= emotions[key].maxEnergyCost)
                    {

                        emotions[key].baseEnergyCost -= 1;

                    }

                }

            }


        }

    }

}
