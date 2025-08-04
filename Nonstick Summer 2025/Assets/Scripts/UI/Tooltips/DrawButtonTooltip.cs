/*************************************************
* Author Names :          Toby
* Date Created :          7/17/2025
* 
* Brief Description : Displays tooltip when player can't draw.
* Sometimes the player cant draw for multiple reasons, i tried to make
* my best judgement for which tooltip would display, in order of inportance.
*   
***************************************************/

using UnityEngine;
using NaughtyAttributes;

public class DrawButtonTooltip : HoverTooltip
{
    [SerializeField, Required]
    private DrawButton drawButton;

    [SerializeField, ResizableTextArea]
    private string canDrawText = "Draws a card for [DrawButtonEnergy]";
    [SerializeField, ResizableTextArea]
    private string deckHasNoCards = "No cards left to draw";
    [SerializeField, ResizableTextArea]
    private string alreadyDrew = "A card has already been drawn this round";
    [SerializeField, ResizableTextArea]
    private string noEnergy = "Draws a card for [DrawButtonEnergy]\n[Gray(You currently have [EnergyColor([PlayerEnergy] Energy)])]";

    protected override string GetRawText()
    {
        if(DeckManager.RemainingDeck.Count <= 0) return deckHasNoCards;

        if (drawButton.CantDrawAnymore) return alreadyDrew;

        if (DialogueManager.CurrentEnergy < DialogueManager.DrawButtonEnergyCost) return noEnergy;

        return canDrawText;
    }

    protected override bool CanOpenTooltip()
    {
        return DialogueManager.ReadUserInput && DialogueManager.UserCanPlayCard;
    }

    protected override void OnPlayerClickComponent()
    {
        RefreshTooltipText();
    }
}
