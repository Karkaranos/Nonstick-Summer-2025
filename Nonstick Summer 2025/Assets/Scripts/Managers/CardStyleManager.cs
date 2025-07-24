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
        StatementStyle, QuestionStyle; // names subject to change

    public static CardValueStyle ObservationStyle => StatementStyle;

    public static Sprite DefaultCardBack, YellowCardBack, RedCardBack, BlueCardBack;

    public static CardValueStyle ErrorStyle;
    
    public CardStyleManager(

        CardValueStyle yellowStyle, CardValueStyle redStyle, CardValueStyle blueStyle, 
        CardValueStyle expressionStyle, CardValueStyle questionStyle, 
        Sprite blankCardBack, Sprite yellowCardBack, Sprite redCardBack, Sprite blueCardBack)
    { 
        YellowStyle = yellowStyle;
        RedStyle = redStyle;
        BlueStyle = blueStyle;

        StatementStyle = expressionStyle;
        QuestionStyle = questionStyle;

        DefaultCardBack = blankCardBack;
        YellowCardBack = yellowCardBack;
        RedCardBack = redCardBack;
        BlueCardBack = blueCardBack;

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
                return StatementStyle.sprite;
            case CardIntention.Observation:
                return ObservationStyle.sprite;
            case CardIntention.Question:
                return QuestionStyle.sprite;
            default:
                Debug.LogWarning("Card has no intention set!");
                return null;
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
                return StatementStyle;
            case CardIntention.Observation:
                return ObservationStyle;
            case CardIntention.Question:
                return QuestionStyle;
            default:
                Debug.LogWarning("Card has no intention set!");
                return ErrorStyle;
        }
    }

    public static CardValueStyle GetEmotionStyle(CardData card)
    {
        return GetEmotionStyle(card.GetEmotion());
    }

    public static CardValueStyle GetEmotionStyle(CardEmotion emotion)
    {
        switch (emotion)
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

    public static Color GetEmotionColor(CardData card)
    {
        return GetEmotionStyle(card).color;
    }

    public static Sprite GetEmotionSprite(CardData card)
    {
       return GetEmotionStyle(card).sprite;
    }

    public static Sprite GetEmotionSprite(CardEmotion emotion)
    {
        return GetEmotionStyle(emotion).sprite;
    }

    public static Sprite GetCardBack(CardData card)
    {
        switch (card.Emotion)
        {
            case CardEmotion.Charming:
                return YellowCardBack;
            case CardEmotion.Assertive:
                return RedCardBack;
            case CardEmotion.Sappy:
                return BlueCardBack;
            default:
                Debug.LogWarning("Card has no emotion set!");
                return DefaultCardBack;
        }
    }
}
