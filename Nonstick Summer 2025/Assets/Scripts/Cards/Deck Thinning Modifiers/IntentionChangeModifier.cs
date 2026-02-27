/*****************************************************************************
// File Name :          IntentionChangeModifier.cs
// Author :             Sky
// Creation Date :      June 26, 2025
// Modified Date :      June 26, 2025
//
// Brief Description :  Modifier for changing intention
*
* TODO: Hide the image icon variable, somehow
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "IntentionChangeModifier", menuName = "Modifier Card/Change Intention")]
public class IntentionChangeModifier : ModifierData
{
    [SerializeField]
    private CardIntention intentionToSet;

    public override bool CanApplyModifier(CardData[] cards)
    {
        bool canApply = base.CanApplyModifier(cards);

        foreach (CardData card in cards)
        {
            if(card==null)
                continue;

            if (card.Intention == intentionToSet)
            {
                return false;
            }
        }
        return canApply;
    }

    protected override void ApplyModifier(CardData[] cards)
    {
        foreach (CardData card in cards)
        {
            if (card == null)
                continue;

            card.Intention = intentionToSet;
        }
    }

    public override Sprite GetIcon()
    {
        return CardStyleManager.GetIntentionSprite(intentionToSet);

    }

    public override string GetModifierName()
    {
        return intentionToSet.ToString();
    }
}
