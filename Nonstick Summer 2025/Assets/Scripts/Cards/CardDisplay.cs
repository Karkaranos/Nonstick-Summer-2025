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

[RequireComponent(typeof(MouseInteractionEvents))]
public class CardDisplay : MonoBehaviour
{
    [BoxGroup("UI Components")][SerializeField] TMP_Text EmotionText;
    [BoxGroup("UI Components")][SerializeField] TMP_Text IntentionText;
    [BoxGroup("UI Components")][SerializeField] Image CardBackground;
    [BoxGroup("UI Components")][SerializeField] Image IntentionImage;
    [BoxGroup("UI Components")][SerializeField] TMP_Text EnergyText;

    public CardData cardData { get{ return card; } }

    [SerializeField] [Tooltip("Set this for debug only")]
    private CardData card;

    private MouseInteractionEvents mouseInteraction;

    public UnityEvent<CardDisplay> OnMouseDown = new UnityEvent<CardDisplay> ();

    private void Start()
    {
        if(card != null) SetCard(card); // mostly for debugging

        mouseInteraction = GetComponent<MouseInteractionEvents>();

        mouseInteraction.OnMouseHoverStart.AddListener(OnMouseHoverStart);
        mouseInteraction.OnMouseHoverEnd.AddListener(OnMouseHoverEnd);
        mouseInteraction.OnMouseDown.AddListener(OnMouseDownStart);
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
            StartCoroutine(DialogueUIController.Instance.UpdateSelection(this));
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
        CardBackground.color = CardStyleManager.GetEmotionColor(card);

        // maybe play a lil animation? (add a parameter?)
    }
}
