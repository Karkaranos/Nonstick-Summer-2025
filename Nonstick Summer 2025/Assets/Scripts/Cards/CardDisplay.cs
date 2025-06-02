using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [BoxGroup("UI Components")][SerializeField] TMP_Text EmotionText;
    [BoxGroup("UI Components")][SerializeField] TMP_Text IntentionText;
    [BoxGroup("UI Components")][SerializeField] Image CardBackground;
    [BoxGroup("UI Components")] [SerializeField] Image IntentionImage;

    [SerializeField] [Tooltip("Set this for debug only")]
    private CardData card;

    private void Start()
    {
        if(card != null) SetCard(card); // mostfly for debugging
    }

    public void SetCard(CardData newCard)
    {
        card.OnCardValueChanged -= RefreshDisplay;

        card = newCard;
        card.OnCardValueChanged += RefreshDisplay;

        RefreshDisplay();
    }

    [Button]
    public void RefreshDisplay()
    {
        if(card == null)
        {
            Debug.LogWarning("No card is set.");
            return;
        }

        if (GameManager.CardStyleManagerReference == null)
            return;

        EmotionText.text = CardStyleManager.GetEmotionStyle(card).DisplayName;
        IntentionText.text = CardStyleManager.GetIntentionStyle(card).DisplayName;
        IntentionImage.sprite = CardStyleManager.GetIntentionSprite(card);
        CardBackground.color = CardStyleManager.GetEmotionColor(card);

        // maybe play a lil animation? (add a parameter?)
    }

}
