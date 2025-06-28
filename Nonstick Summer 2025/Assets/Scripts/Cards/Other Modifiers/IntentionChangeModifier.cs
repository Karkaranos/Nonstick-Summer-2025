using UnityEngine;
using NaughtyAttributes;

/*****************************************************************************
// File Name :          IntentionChangeModifier.cs
// Author :             Sky
// Creation Date :      June 26, 2025
// Modified Date :      June 26, 2025
//
// Brief Description :  Modifier for changing intention
*****************************************************************************/
[CreateAssetMenu(fileName = "IntentionChangeModifier", menuName = "Scriptable Objects/IntentionChangeModifier")]
public class IntentionChangeModifier : ModifierData
{
    [SerializeField]
    private CardIntention intentionToSet;

    [SerializeField, ShowAssetPreview(32, 32)]
    private Sprite modifierSprite;

    public override bool CanApplyModifier(CardData[] cards)
    {
        bool canApply = base.CanApplyModifier(cards);

        foreach (CardData card in cards)
        {
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
            card.Intention = intentionToSet;
        }
    }

    public override Sprite GetIcon()
    {
        return modifierSprite;
    }
}
