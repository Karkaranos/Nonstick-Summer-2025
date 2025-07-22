using UnityEngine;

/*
 * constant variables for card displays to reference.
 * Variables are filled in GameManager. They aren't technically needed, this script is really just for organization.
 * -Toby
 */

public class CardStyleManager
{
    public static CardStyleManager Instance => GameManager.CardStyleManagerReference;

    public static CardValueStyle YellowStyle, RedStyle, BlueStyle, 
        ExpressionStyle/*, ObservationStyle*/, QuestionStyle; // names subject to change

    public static CardValueStyle ErrorStyle;
    
    public CardStyleManager(
        CardValueStyle yellowStyle, CardValueStyle assertiveStyle, CardValueStyle blueStyle, 
        CardValueStyle expressionStyle/*, CardValueStyle observationStyle*/, CardValueStyle questionStyle) 
    { 
        YellowStyle = yellowStyle;
        RedStyle = assertiveStyle;
        BlueStyle = blueStyle;
        ExpressionStyle = expressionStyle;
        //ObservationStyle = observationStyle;
        QuestionStyle = questionStyle;

        ErrorStyle = new CardValueStyle(Color.red, "ERROR");
    }

    public static Sprite GetIntentionSprite(CardData card)
    {
        return GetIntentionSprite(card.Intention);
    }

    public static Sprite GetIntentionSprite(CardIntention intention)
    {
        switch (intention)
        {
            case CardIntention.Expression:
                return ExpressionStyle.sprite;
            /*case CardIntention.Observation:
                return ObservationStyle.sprite;*/
            case CardIntention.Question:
                return QuestionStyle.sprite;
            default:
                Debug.LogWarning("Card has no intention set!");
                return null;
        }
    }

    public static Color GetEmotionColor(CardData card)
    {
        return GetEmotionStyle(card).color;
    }

    public static CardValueStyle GetEmotionStyle(CardData card)
    {
        switch (card.Emotion)
        {
            case CardEmotion.Charming:
                return YellowStyle;
            case CardEmotion.Assertive:
                return RedStyle;
            case CardEmotion.Sappy:
                return BlueStyle;
            default:
                Debug.LogWarning("Card has no emotion set!");
                return ErrorStyle;
        }
    }

    public static Color GetIntentionColor(CardData card)
    {
        return GetIntentionStyle(card).color;
    }

    public static CardValueStyle GetIntentionStyle(CardData card)
    {
        switch (card.Intention)
        {
            case CardIntention.Expression:
                return ExpressionStyle;
            /*case CardIntention.Observation:
                return ObservationStyle;*/
            case CardIntention.Question:
                return QuestionStyle;
            default:
                Debug.LogWarning("Card has no intention set!");
                return ErrorStyle;
        }
    }
}
