/*****************************************************************************
* File Name :         ModifierManager.cs
* Author :            Toby
* Creation Date :     June 21, 2025
*
* Brief Description : Stores the modifiers in the players inventory.
* 
* Less generalized than deckmanager, because there probably won't need to be 
* too many crazy modifier collections in our game (please). 
* Obviously this can be refactored need be.
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class ModifierManager
{
    public static ModifierManager Instance => GameManager.ModifierManagerReference;

    public static IReadOnlyCollection<ModifierData> ModifierCollection => _modifierCollection.AsReadOnly(); //asreadonly doesnt make a copy of the data, so this is pretty inexpensive >:)

    private static List<ModifierData> _modifierCollection = new List<ModifierData>();

    #region Function References
    
    /// <summary>
    /// Adds a modifier to collection
    /// </summary>
    public static void AddCard(ModifierData modifier)
    {
        _modifierCollection.Add(modifier);
    }

    /// <summary>
    /// Removes a modifier from collection
    /// </summary>
    public static void RemoveCard(ModifierData modifier)
    {
        _modifierCollection.Remove(modifier);
    }

    /// <summary>
    /// Replaces a modifier
    /// </summary>
    public static void UpdateCard(ModifierData oldCard, ModifierData newCard)
    {
        int idx = _modifierCollection.IndexOf(oldCard);
        if (idx != -1)
        {
            _modifierCollection[idx] = newCard;
        }
    }

    /// <summary>
    /// Shuffles modifiers (not sure if this will ever be needed? haha)
    /// </summary>
    public static void ShuffleDeck()
    {
        _modifierCollection.Shuffle();
    }

    #endregion

    #region Helper Functions

    // there is no help >:)

    #endregion




}
