/*****************************************************************************
* File Name :         ReturnToHandStamp.cs
* Author :            Toby
* Creation Date :     June 16, 2025
*
* Brief Description : 
* 
* TODO: if we make it so that the player can modify cards in their hand theres gonna be a lot of bugs
*****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "StatChange", menuName = "Scriptable Objects/Stamp/Return To Hand")]
public class ReturnToHandStamp : ModifierStamp
{
    // If stamp has been used this combat
    [HideInInspector] public bool Expended = false;
    protected override void EffectTriggered(CardData affectedCard)
    {
        if(DialogueUIController.Instance != null && Expended == false)
        {
            Debug.Log("Using Return to Hand Stamp");
            Expended = true;
            var copyCard = DialogueUIController.Instance.DeckDisplay.AddCardToHand(affectedCard); // add this card to deck lol
            var thisStamp = affectedCard.Stamps.Where(s => s.type == typeof(ReturnToHandStamp)).First();
            ((ReturnToHandStamp)thisStamp).Expended = true;
            //DialogueUIController.Instance.DeckDisplay.AddCardToHand(affectedCard.CopyCardWithoutStampType(this.type));
        }
        DialogueUIController.Instance.DeckDisplay.DisplayAllCards();
    }

    public override void OnStampAdded(CardData affectedCard)
    {
        base.OnStampAdded(affectedCard);

        Expended = false;
    }

    public override void BeforeCardDrawnFromDeck(CardData affectedCard)
    {
        Expended = false;
    }
}
