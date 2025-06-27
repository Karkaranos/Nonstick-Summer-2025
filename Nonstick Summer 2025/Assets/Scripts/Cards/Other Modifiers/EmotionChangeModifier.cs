using NaughtyAttributes;
using UnityEngine;
/*****************************************************************************
// File Name :          EmotionChangeModifier.cs
// Author :             Sky
// Creation Date :      June 26, 2025
// Modified Date :      June 26, 2025
//
// Brief Description :  Modifier for changing emotion
*****************************************************************************/

[CreateAssetMenu(fileName = "EmotionChangeModifier", menuName = "Scriptable Objects/EmotionChangeModifier")]
public class EmotionChangeModifier : ModifierData
{
    [SerializeField]
    private CardEmotion emotionToSet;

    [SerializeField, ShowAssetPreview(32, 32)]
    private Sprite modifierSprite;

    public override bool CanApplyModifier(CardData[] cards)
    {
        bool canApply = base.CanApplyModifier(cards);

        foreach (CardData card in cards)
        {
            if (card.Emotion == emotionToSet)
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
            card.Emotion = emotionToSet;
        }
    }

    public override Sprite GetIcon()
    {
        return modifierSprite;
    }
}
