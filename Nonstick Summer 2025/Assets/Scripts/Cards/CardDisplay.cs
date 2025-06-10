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

    [SerializeField] [Tooltip("Set this for debug only")]
    private CardData card;


    //this is for display purposes
    public bool selected = false;

    private MouseInteractionEvents mouseInteraction;

    private void Start()
    {
        if(card != null) SetCard(card); // mostfly for debugging

        mouseInteraction = GetComponent<MouseInteractionEvents>();

        mouseInteraction.OnMouseHoverStart.AddListener(OnMouseHoverStart);
        mouseInteraction.OnMouseHoverEnd.AddListener(OnMouseHoverEnd);
        mouseInteraction.OnMouseDown.AddListener(OnMouseDown);
    }

    private void OnMouseHoverStart() // this should be moved to another script
    {
        if (DialogueManager.PlayerInCombat /*&& TODO: if player does not have card selected*/ )
           DialogueUIController.Instance.UpdateHoveringCard(card);
    }

    private void OnMouseHoverEnd() // this should be moved to another script
    {
        if (DialogueManager.PlayerInCombat /*&& TODO: if player does not have card selected*/)
            DialogueUIController.Instance.UpdateHoveringCard(null);
    }

    private void OnMouseDown()
    {

        if (DialogueManager.PlayerInCombat)
        {

            if (!selected)
            {

                selected = true;
                DialogueUIController.Instance.UpdateSelection(card, selected, this);

            }
            else if(selected)
            {

                selected = false;
                DialogueUIController.Instance.UpdateSelection(card, selected, this);

            }

        }
        

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
