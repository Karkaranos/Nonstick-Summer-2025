using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;

//[RequireComponent(typeof(MouseInteractionEvents))]
public partial class CardDisplay : MonoBehaviour
{
    [Header("Display")]

    [Foldout("UI Components"), SerializeField, Required] TMP_Text EmotionText;
    [Foldout("UI Components"), SerializeField, Required] TMP_Text IntentionText;
    [Foldout("UI Components"), SerializeField, Required] Image CardBackgroundImage;
    [Foldout("UI Components"), SerializeField, Required] RectTransform cardBackground;
    [Foldout("UI Components"), SerializeField, Required] Image IntentionImage;
    [Foldout("UI Components"), SerializeField, Required] TMP_Text EnergyText;
    [Foldout("UI Components"), SerializeField] StampIconDisplay[] StampImages;

    public CardData cardData { get{ return card; } }

    [SerializeField] [Tooltip("Set this for debug only")]
    private CardData card;

    private MouseInteractionEvents mouseInteraction;
    private RectTransform rectTransform;

    public UnityEvent<CardDisplay> OnMouseDown = new UnityEvent<CardDisplay> ();

    private void Start()
    {
        if (card != null) SetCard(card); // mostly for debugging

        rectTransform = GetComponent<RectTransform>();

        if(TryGetComponent<MouseInteractionEvents>(out mouseInteraction))
        {
            mouseInteraction.OnMouseHoverStart.AddListener(OnMouseHoverStart);
            mouseInteraction.OnMouseHoverEnd.AddListener(OnMouseHoverEnd);
            mouseInteraction.OnMouseHoverStay.AddListener(OnMouseHoverStart);
            mouseInteraction.OnMouseDown.AddListener(OnMouseDownStart);
        }

        // EVERYTHING breaks if you uncomment this. DO NOT touch it.
        //basePosition = cardBackground.anchoredPosition;
    }

    private void OnMouseHoverStart() // TODO this should be moved to another script
    {
        if (DialogueUIController.Instance != null && DialogueUIController.Instance.DeckDisplay.FirstSelectedCard == null 
            && DialogueManager.PlayerInCombat && DialogueManager.ReadUserInput)
           DialogueUIController.Instance.UpdateHoveringCard(card);
    }

    private void OnMouseHoverEnd() // TODO this should be moved to another script
    {
        if (DialogueUIController.Instance != null && DialogueUIController.Instance.DeckDisplay.FirstSelectedCard == null
            && DialogueManager.PlayerInCombat && DialogueManager.ReadUserInput)
            DialogueUIController.Instance.UpdateHoveringCard(null);
    }

    private void OnMouseDownStart()
    {
        OnMouseDown.Invoke(this);
        /*if (DialogueUIController.Instance != null && DialogueManager.ReadUserInput)
        {
            Debug.Log("selected card");
            StartCoroutine(DialogueUIController.Instance.OnSelectionUpdated(this));
        }*/
    }

    public void SetCard(CardData newCard)
    {
        if(card != null)
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
        EnergyText.text = (card.EnergyCost == 0) ? "" : card.EnergyCost.ToString();
        EnergyText.color = (card.EnergyCost < 0) ? Color.red : Color.green;
        IntentionImage.sprite = CardStyleManager.GetIntentionSprite(card);
        CardBackgroundImage.color = CardStyleManager.GetEmotionColor(card);

        UpdateStampIcons();

        // maybe play a lil animation? (add a parameter?)
    }

    private void UpdateStampIcons()
    {
        int i;
        for (i = 0; i<card.Stamps.Count && i<StampImages.Length; i++)
        {
            var stamp = card.Stamps.ElementAt(i);

            if (stamp == null)
                continue;

            StampImages[i].SetStamp(stamp);
        }

        for(;i<StampImages.Length; i++)
        {
            StampImages[i].SetStamp(null);
        }

        if (card.Stamps.Count > StampImages.Length)
        {
            Debug.LogError("not enough stamp icons for the number of stamps");
        }
    }
}
