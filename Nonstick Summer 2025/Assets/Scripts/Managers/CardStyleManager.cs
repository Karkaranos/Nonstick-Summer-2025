using UnityEngine;

/*
 * constant variables for card displays to reference.
 * Variables are filled in GameManager. They aren't technically needed, this script is really just for organization.
 * -Toby
 */

public class CardStyleManager
{
    public static CardStyleManager Instance => GameManager.CardStyleManagerReference;

    public static CardValueStyle CharmingStyle, AssertiveStyle, SappyStyle, 
        ExpressionStyle, ObservationStyle, QuestionStyle; // names subject to change
    public static Sprite ExpressionSprite, ObservationSprite, QuestionSprite;

    public static CardValueStyle ErrorStyle;
    
    public CardStyleManager(
        CardValueStyle yellowStyle, CardValueStyle assertiveStyle, CardValueStyle blueStyle, 
        CardValueStyle expressionStyle, CardValueStyle observationStyle, CardValueStyle questionStyle,
        Sprite expressionSprite, Sprite observationSprite, Sprite questionSprite) 
    { 
        CharmingStyle = yellowStyle;
        AssertiveStyle = assertiveStyle;
        SappyStyle = blueStyle;
        ExpressionStyle = expressionStyle;
        ObservationStyle = observationStyle;
        QuestionStyle = questionStyle;
        ExpressionSprite = expressionSprite;
        ObservationSprite = observationSprite;
        QuestionSprite = questionSprite;

        ErrorStyle = new CardValueStyle(Color.red, "ERROR");
    }

    public static Sprite GetIntentionSprite(CardData card)
    {
        // there might be individual sprites for each emotion,
        // in that case i will be making a dictionary with [tuple<Emotion, CardIntention>] keys

        switch (card.Intention)
        {
            case CardIntention.Expression:
                return ExpressionSprite;
            case CardIntention.Observation:
                return ObservationSprite;
            case CardIntention.Question:
                return QuestionSprite;
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
                return CharmingStyle;
            case CardEmotion.Assertive:
                return AssertiveStyle;
            case CardEmotion.Sappy:
                return SappyStyle;
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
            case CardIntention.Observation:
                return ObservationStyle;
            case CardIntention.Question:
                return QuestionStyle;
            default:
                Debug.LogWarning("Card has no intention set!");
                return ErrorStyle;
        }
    }
}
