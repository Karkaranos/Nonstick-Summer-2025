/*****************************************************************************
* File Name :         StampModifierData.cs
* Author :            Toby
* Creation Date :     June 20, 2025
*
* Brief Description : Scriptable object for applying stamps to cards.
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "StampModifierData", menuName = "Scriptable Objects/Stamp Modifier Data")]
public class StampModifierData : ModifierData
{
    [Header("Stamps")]

    [Required, Expandable]
    public ModifierStamp StampToApply;

    public override bool CanApplyModifier(CardData[] cards)
    {
        if( !base.CanApplyModifier(cards)) 
            return false;

        foreach(CardData card in cards) 
        {
            // TODO get rid of this because of modifier upgrading.
            if(card.HasStampOfType(StampToApply.type))
            {
                return false;
            }
        }
        return true; 
    }

    protected override void ApplyModifier(CardData[] cards)
    {
        foreach (CardData card in cards)
            card.AddStamp(StampToApply);
    }

    public override Sprite GetIcon()
    {
        return StampToApply.Icon;
    }
}
