/*****************************************************************************
* File Name :         MumbleStamp.cs
* Author :            Cade
* Creation Date :     July 22 2025
*
* Brief Description : Modifier that occurs when the player plays a card
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "MumbleStamp", menuName = "Modifier Card/Mumble Card")]
public class MumbleStamp : ModifierStamp
{
    protected override void EffectTriggered(CardData affectedCard)
    {
        var dialogueOption = DialogueManager.CurrentDialogueBranch.ReturnDialogueOption(affectedCard);
        float relationshipChange = dialogueOption.ChangeInRelationshipStatus;

        if(relationshipChange < 0)
        {
            DialogueUIController.Instance.DeckDisplay.AddCardToHand(affectedCard.CopyCardWithoutStampType(this.type));
            DialogueManager.StopCardProcessing();
        }
    }
}
