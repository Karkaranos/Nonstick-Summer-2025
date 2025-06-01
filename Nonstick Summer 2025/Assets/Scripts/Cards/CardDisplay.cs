using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CardDisplay : MonoBehaviour
{
    [BoxGroup("UI Components")] [SerializeField] Image IntentionImage;
    [BoxGroup("UI Components")] [SerializeField] Image CardBackground;

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

        IntentionImage.sprite = CardStyleManager.GetIntentionSprite(card);
        CardBackground.color = CardStyleManager.GetEmotionColor(card);

        // maybe play a lil animation? (add a parameter?)


    }

}
