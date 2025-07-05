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
using UnityEngine;
using UnityEngine.UI;

public class DiscardButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        UpdateButtonEnabled();
        button.onClick.AddListener(OnButtonPressed);

        DialogueUIController.Instance.DeckDisplay.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool enabled = (DeckManager.RemainingDeck.Count > 0)
            && (DialogueManager.CurrentEnergy >= DialogueManager.DrawButtonEnergyCost);
        button.interactable = enabled;
    }

    public void OnButtonPressed()
    {
        DialogueManager.DrawCards(N: 1, forceDraw: true);
        DialogueManager.CurrentEnergy = DialogueManager.CurrentEnergy - DialogueManager.DrawButtonEnergyCost;
        UpdateButtonEnabled();
    }
}
