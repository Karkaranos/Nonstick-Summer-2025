/*****************************************************************************
* File Name :         DiscardButton.cs
* Author :            Toby
* Creation Date :     July 5, 2025
*
* Brief Description : The Discard button during NPC combat. Gives the player a card
* when pressed.
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DiscardButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;

    private DeckDisplayer hand => DialogueUIController.Instance.DeckDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        UpdateButtonEnabled();
        button.onClick.AddListener(OnButtonPressed);

        hand.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool enabled = (hand.HasCardsSelected);
        button.interactable = enabled;
    }

    /// <summary>
    /// can only be pressed if button is interactable
    /// </summary>
    public void OnButtonPressed()
    {
        DialogueManager.SetCurrentEnergy(DialogueManager.CurrentEnergy += 1);

        // foreach in case player somehow has multiple cards selected
        foreach(var card in hand.selectedCards.ToArray()) //ToArray so we can safely remove items from the original collection
        {
            hand.DiscardCard(card.cardData);
        }

        UpdateButtonEnabled();
    }
}
