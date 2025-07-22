/*****************************************************************************
* File Name :         ModifierData.cs
* Author :            Toby
* Creation Date :     June 20, 2025
*
* Brief Description : Scriptable object for modifiers. This is what the player
* collects by interacting with the world, these will go in the players inventory.
* 
* Currently, modifiers only apply stamps, however it this script structured so that it
* could easily be refactored to support more complex modifier behaviour in the future.
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

//[CreateAssetMenu(fileName = "ModifierData", menuName = "Scriptable Objects/ModifierData")]
public abstract class ModifierData : ScriptableObject
{
    [Tooltip("How many cards this modifier can apply to")] [Min(1)]
    public int MaxCardsApplied = 1;

    [ShowIf(nameof(_showMinCardsApplied)), Min(1)]
    public int MinCardsApplied = 1;

    [SerializeField, ResizableTextArea]
    protected string _tooltipDescription;

    [SerializeField, ShowAssetPreview(32, 32)]
    protected Sprite icon;

    #region Debug
    private bool _showMinCardsApplied => MaxCardsApplied > 1;
    #endregion

    /// <summary>
    /// Returns if modifier was successfully used
    /// </summary>
    public bool TryApplyModifier(CardData[] cards)
    {
        if(CanApplyModifier(cards)) 
        {
            ApplyModifier(cards);
            return true;
        }
        return false;
        //TODO remove from player inventory
    }

    public virtual bool CanApplyModifier(CardData[] cards)
    {
        return cards.Length >= 1 && cards.Length <= MaxCardsApplied;
    }
    protected abstract void ApplyModifier(CardData[] cards);

    public virtual Sprite GetIcon()
    {
        return icon;
    }

    public virtual string GetTooltipDescription()
    {
        return _tooltipDescription;
    }

    public virtual int GetHashCodeByProperties()
    {
        // Modifiers dont have that much to differentiate them :/
        return HashCode.Combine(this.name); // <- this.name is single and ready to mingle
    }
}
