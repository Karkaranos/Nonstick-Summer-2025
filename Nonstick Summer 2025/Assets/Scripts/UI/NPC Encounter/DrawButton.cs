/*****************************************************************************
* File Name :         DrawButton.cs
* Author :            Toby
* Creation Date :     June 29, 2025
*
* Brief Description : The Draw button during NPC combat. Gives the player a card
* when pressed.
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class DrawButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    private DeckDisplayer handDisplay => DialogueUIController.Instance.DeckDisplay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        UpdateButtonEnabled();
        button.onClick.AddListener(OnButtonPressed);

        DialogueManager.OnCardPlayedStarted.AddListener(UpdateButtonEnabled);
        DialogueManager.OnCardPlayedFinished.AddListener(UpdateButtonEnabled);
        handDisplay.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled); // idk it just feels right
        DialogueUIController.Instance.playCardButton.onClick.AddListener(UpdateButtonEnabled);
        DeckManager.PlayerHand.OnDeckChanged.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        Debug.Log($"{DeckManager.RemainingDeck.Count} Cards left in remaining deck");
        bool enabled = (DeckManager.RemainingDeck.Count > 0) && DialogueManager.ReadUserInput
            && (DialogueManager.CurrentEnergy >= DialogueManager.DrawButtonEnergyCost );
        button.interactable = enabled;
    }

    public void OnButtonPressed()
    {
        DialogueManager.DrawCards(N: 1, forceDraw: true);
        DialogueManager.CurrentEnergy = DialogueManager.CurrentEnergy - DialogueManager.DrawButtonEnergyCost;
        UpdateButtonEnabled();
    }
}
