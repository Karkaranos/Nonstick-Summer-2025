/*****************************************************************************
* File Name :         CardStamp.cs
* Author :            Toby
* Creation Date :     June 16, 2025
*
* Brief Description : Partial class for CardData
 * Collection for stamps and _modifier logic.
 * This script isnt super necessary, but it handles a lot of _modifier logic,
 * instead of putting it in CardData.
 *
 * TODO: apply stamps to cards with user input somehow
*****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using NUnit.Framework;

public partial class CardData
{
    public IReadOnlyCollection<ModifierStamp> Stamps => _stamps.AsReadOnly();

    [Tooltip("Set default stamps here")]
    [SerializeField] private List<ModifierStamp> _stamps = new List<ModifierStamp>();

    /// <summary>
    /// Tries to invoke the stamps effect.
    /// </summary>
    /// <param name="reason">Why a _modifier would be triggered. This should line up to what the card is currently doing.</param>
    public void TryTriggerStampEffect(StampTriggerConditions reason)
    {
        /* affectedCard needs to be a parameter (and not be a stored variable) because
         * card cant have an initialize function, thus CardStampCollection cant have an
         * initialize function. */

        foreach (var stamp in _stamps) 
            stamp.TryTriggerEffect(reason, this);
    }

    #region modification

    public void AddStamp(ModifierStamp stamp)
    {
        _stamps.Add(Instantiate(stamp));
        stamp.OnStampAdded(this);
        OnCardValueChanged.Invoke();
    }

    public void RemoveStamp(ModifierStamp stamp)
    {
        var idx = _stamps.IndexOf(stamp);
        if (idx != -1)
        {
            _stamps.RemoveAt(idx);
            OnCardValueChanged.Invoke();
        }
    }

    public void RemoveStampOfType(Type stampType)
    {
        foreach (var stamp in _stamps)
        {
            if (stamp.GetType() == stampType)
            {
                _stamps.Remove(stamp);
                OnCardValueChanged?.Invoke();

                return;
            }
        }
    }

    #endregion

    #region utility functions

    public bool HasStampOfType(Type stampType)
    {
        foreach (var stamp in _stamps)
        {
            if (stamp.GetType() == stampType)
                return true;
        }

        return false;
    }

    /// <summary>
    /// TODO use this function with the _modifier that puts a copy of a card into the players hand (but not into permanent deck)
    /// </summary>
    public CardData CopyCardWithoutStampType(Type stampType)
    {
        var card = CopyCard();
        card.RemoveStampOfType(stampType);
        return card;
    }

    #endregion
}
