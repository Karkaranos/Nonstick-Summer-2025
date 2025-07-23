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

[CreateAssetMenu(fileName = "StatChange", menuName = "Scriptable Objects/Stamp/Return To Hand")]
public class ReturnToHandStamp : ModifierStamp
{
    protected override void EffectTriggered(CardData affectedCard)
    {
        if(DialogueUIController.Instance != null )
        {
            DialogueUIController.Instance.DeckDisplay.AddCardToHand(affectedCard.CopyCardWithoutStampType(this.type));
        }
    }
}
