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
[HideInInspector] public enum characters { Grandma, Uncle, Cousin, Mom };

public class RelationshipManager
{
    [Tooltip("Dictionary that holds character stats. Keys are characters (enum) and value are relationship" +
        "stats (references the class.")]
    [HideInInspector] public static Dictionary<characters, RelationshipStats> characterRelationships = new Dictionary<characters, RelationshipStats>();

    /// <summary>
    /// Initializes dictionary
    /// </summary>
    /// <param name="grandmaStartingValue"></param>
    /// <param name="uncleStartingValue"></param>
    /// <param name="cousinStartingValue"></param>
    /// <param name="momStartingValue"></param>
    public RelationshipManager(RelationshipStats grandmaStartingValue, RelationshipStats uncleStartingValue, RelationshipStats cousinStartingValue, RelationshipStats momStartingValue)
    {
        characterRelationships.Remove(characters.Grandma);
        characterRelationships.Remove(characters.Uncle);
        characterRelationships.Remove(characters.Cousin);
        characterRelationships.Remove(characters.Mom);
        characterRelationships.Add(characters.Grandma, grandmaStartingValue);
        characterRelationships.Add(characters.Uncle, uncleStartingValue);
        characterRelationships.Add(characters.Cousin, cousinStartingValue);
        characterRelationships.Add(characters.Mom, momStartingValue);
    }
}
