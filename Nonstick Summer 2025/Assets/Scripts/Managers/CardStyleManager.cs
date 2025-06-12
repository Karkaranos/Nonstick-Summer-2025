using UnityEngine;

/*
 * constant variables for card displays to reference.
 * Variables are filled in GameManager. They aren't technically needed, this script is really just for organization.
 * -Toby
 */

public class CardStyleManager
{
    public static CardStyleManager Instance => GameManager.CardStyleManagerReference;

    private static CardValueStyle YellowStyle, RedStyle, BlueStyle, 
        ExpressionStyle, ObservationStyle, QuestionStyle; // names subject to change
    private static Sprite ExpressionSprite, ObservationSprite, QuestionSprite;

    private static CardValueStyle _errorStyle;
    
    public CardStyleManager(
        CardValueStyle yellowStyle, CardValueStyle redStyle, CardValueStyle blueStyle, 
        CardValueStyle ExpressionStyle, CardValueStyle ObservationStyle, CardValueStyle QuestionStyle,
        Sprite ExpressionSprite, Sprite ObservationSprite, Sprite QuestionSprite) 
    { 
        YellowStyle = yellowStyle;
        RedStyle = redStyle;
        BlueStyle = blueStyle;
        ExpressionStyle = ExpressionStyle;
        ObservationStyle = ObservationStyle;
        QuestionStyle = QuestionStyle;
        ExpressionSprite = ExpressionSprite;
        ObservationSprite = ObservationSprite;
        QuestionSprite = QuestionSprite;

        _errorStyle = new CardValueStyle(Color.red, "ERROR");
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
                return YellowStyle;
            case CardEmotion.Assertive:
                return RedStyle;
            case CardEmotion.Sappy:
                return BlueStyle;
            default:
                Debug.LogWarning("Card has no emotion set!");
                return _errorStyle;
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
                return _errorStyle;
        }
    }
}
