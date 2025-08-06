/*****************************************************************************
* File Name :         PlayCardButton.cs
* Author :            Toby
* Creation Date :     8/5/2025 (day before code freeze)
*
* Brief Description : Plays the selected card
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private CanvasGroup group;
    private DeckDisplayer hand => DialogueUIController.Instance.DeckDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        button.onClick.AddListener(OnButtonPressed);
        UpdateButtonEnabled();

        DialogueManager.OnCardPlayedStarted.AddListener(UpdateButtonEnabled);
        DialogueManager.OnPlayerFinishReadingDialogue.AddListener(UpdateButtonEnabled);
        DialogueManager.OnCardPlayedFinished.AddListener(UpdateButtonEnabled);
        hand.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool enabled = hand.HasCardsSelected;
        button.interactable = enabled;

        StaticUtilities.ToggleCanvasGroup(group, DialogueManager.ReadUserInput && DialogueManager.UserCanPlayCard);
    }

    public void OnButtonPressed()
    {
        DialogueManager.ProcessPlayCard(hand.FirstSelectedCard);
        UpdateButtonEnabled();
    }
}
