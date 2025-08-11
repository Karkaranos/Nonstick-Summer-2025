/*****************************************************************************
* File Name :         StampModifierData.cs
* Author :            Toby
* Creation Date :     June 20, 2025
*
* Brief Description : Scriptable object for applying stamps to cards.
*****************************************************************************/

using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StampModifierData", menuName = "Modifier Card/Stamp Applier")]
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
            // TODO: this doesnt consider relationship / social battery mods are the same type
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

    public override string GetTooltipDescription()
    {
        return (base.GetTooltipDescription() + "\n\n[StampName]\n" + StampToApply.ShortDescription)
            .Replace("[StampName]", $"<color=#{GameManager.Instance.StampTooltipColor.ToHex()}>{StampToApply.StampName}</color>");
        
    }
}
