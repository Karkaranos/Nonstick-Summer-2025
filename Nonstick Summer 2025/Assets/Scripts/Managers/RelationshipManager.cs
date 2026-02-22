using System.Collections.Generic;
using UnityEngine;
/*****************************************************************************
* File Name :         RelationshipManager.cs
* Author :            Sky
* Creation Date :     June 8, 2025
*
* Brief Description :  Records relationship data among all NPCs. Mainly for Dictionary storage.
*
* TODO:
* 
*****************************************************************************/
[Tooltip("Character bosses in the game.")]
[HideInInspector] public enum Character { Grandma, Uncle, Cousin, Mom, Tutorial };

public class RelationshipManager
{
    [Tooltip("Dictionary that holds character stats. Keys are characters (enum) and value are relationship" +
        "stats (references the class.")]
    [HideInInspector] public static Dictionary<Character, RelationshipStats> characterRelationships = new Dictionary<Character, RelationshipStats>();

    /// <summary>
    /// Initializes dictionary
    /// </summary>
    /// <param name="grandmaStartingValue"></param>
    /// <param name="uncleStartingValue"></param>
    /// <param name="cousinStartingValue"></param>
    /// <param name="momStartingValue"></param>
    public RelationshipManager(RelationshipStats grandmaStartingValue, RelationshipStats uncleStartingValue, RelationshipStats cousinStartingValue, RelationshipStats momStartingValue)
    {
        characterRelationships.Clear();
        characterRelationships = new Dictionary<Character, RelationshipStats>();
        characterRelationships.Add(Character.Grandma, grandmaStartingValue);
        characterRelationships.Add(Character.Uncle, uncleStartingValue);
        characterRelationships.Add(Character.Cousin, cousinStartingValue);
        characterRelationships.Add(Character.Mom, momStartingValue);
    }
}
