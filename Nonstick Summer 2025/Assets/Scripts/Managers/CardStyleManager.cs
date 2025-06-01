using UnityEngine;

/*
 * constant variables for card displays to reference.
 * Variables are filled in GameManager. They aren't technically needed, this script is really just for organization.
 * -Toby
 */

public class CardStyleManager
{
    public static CardStyleManager Instance => GameManager.CardStyleManagerReference;

    public static Color YellowColor, RedColor, BlueColor; // names subject to change
    public static Sprite Intention1Sprite, Intention2Sprite, Intention3Sprite;
    
    public CardStyleManager(Color yellowColor, Color redColor, Color blueColor, 
        Sprite intention1Sprite, Sprite intention2Sprite, Sprite intention3Sprite) 
    { 
        YellowColor = yellowColor;
        RedColor = redColor;
        BlueColor = blueColor;
        Intention1Sprite = intention1Sprite;
        Intention2Sprite = intention2Sprite;
        Intention3Sprite = intention3Sprite;
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
        switch (card.Emotion)
        {
            case CardEmotion.Yellow:
                return YellowColor;
            case CardEmotion.Red:
                return RedColor;
            case CardEmotion.Blue:
                return BlueColor;
            default:
                Debug.LogWarning("Card has no emotion set!");
                return Color.clear;
        }
    }
}
