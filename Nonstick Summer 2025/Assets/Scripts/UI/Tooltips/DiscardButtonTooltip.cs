/*************************************************
* Author Names :          Toby
* Date Created :          7/17/2025
* 
* Brief Description : Displays tooltip when player can't discard.
* Sometimes the player cant discard for multiple reasons, i tried to make
* my best judgement for which tooltip would display, in order of inportance.
*   
***************************************************/


using NaughtyAttributes;
using UnityEngine;

public class DiscardButtonTooltip : HoverTooltip
{
    //[SerializeField, Required]
    //private DiscardButton discardButton;

    private DeckDisplayer hand => DialogueUIController.Instance.deckDisplay;

    [SerializeField, ResizableTextArea]
    private string canDiscardText = "Discards a selected card for [DiscardEnergy]";
    [SerializeField, ResizableTextArea]
    private string noCardSelected = "Discards a selected card for [DiscardEnergy]\n[Gray((No card selected))]";

    [Header("Archipelago")]
    [SerializeField] public ArchipelagoItem archipelagoItem = ArchipelagoItem.DiscardButton;
    [SerializeField, ResizableTextArea]
    private string archipelagoTooltip = "Discard button is not unlocked in the multiworld!";
    private bool apItemUnlocked => APInventoryService.Instance.IsItemCollected(archipelagoItem);

    protected override string GetRawText()
    {
        if (!apItemUnlocked) return archipelagoTooltip;

        if (!hand.HasCardsSelected) return noCardSelected;

        return canDiscardText;
    }

    protected override bool CanOpenTooltip()
    {
        return  DialogueManager.ReadUserInput && DialogueManager.UserCanPlayCard;
    }

    protected override void OnPlayerClickComponent()
    {
        RefreshTooltipText();
    }
}