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
        Intention1Style, Intention2Style, Intention3Style; // names subject to change
    private static Sprite Intention1Sprite, Intention2Sprite, Intention3Sprite;

    private static CardValueStyle _errorStyle;
    
    public CardStyleManager(
        CardValueStyle yellowStyle, CardValueStyle redStyle, CardValueStyle blueStyle, 
        CardValueStyle intention1Style, CardValueStyle intention2Style, CardValueStyle intention3Style,
        Sprite intention1Sprite, Sprite intention2Sprite, Sprite intention3Sprite) 
    { 
        YellowStyle = yellowStyle;
        RedStyle = redStyle;
        BlueStyle = blueStyle;
        Intention1Style = intention1Style;
        Intention2Style = intention2Style;
        Intention3Style = intention3Style;
        Intention1Sprite = intention1Sprite;
        Intention2Sprite = intention2Sprite;
        Intention3Sprite = intention3Sprite;

        _errorStyle = new CardValueStyle(Color.red, "ERROR");
    }

    public static Sprite GetIntentionSprite(CardData card)
    {
        // there might be individual sprites for each emotion,
        // in that case i will be making a dictionary with [tuple<Emotion, CardIntention>] keys

        switch (card.Intention)
        {
            case CardIntention.Intention1:
                return Intention1Sprite;
            case CardIntention.Intention2:
                return Intention2Sprite;
            case CardIntention.Intention3:
                return Intention3Sprite;
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
            case CardEmotion.Yellow:
                return YellowStyle;
            case CardEmotion.Red:
                return RedStyle;
            case CardEmotion.Blue:
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
            case CardIntention.Intention1:
                return Intention1Style;
            case CardIntention.Intention2:
                return Intention2Style;
            case CardIntention.Intention3:
                return Intention3Style;
            default:
                Debug.LogWarning("Card has no intention set!");
                return _errorStyle;
        }
    }
}
