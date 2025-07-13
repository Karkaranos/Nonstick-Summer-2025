/*****************************************************************************
* File Name :         DestroyCardModifier.cs
* Author :            Toby
* Creation Date :     July 13 2025
*
* Brief Description : Modifier that destroys inputted card/cards
* 
*****************************************************************************/

using UnityEngine;

[CreateAssetMenu(fileName = "EmotionChangeModifier", menuName = "Modifier Card/Destroy Card")]
public class DestroyCardModifier : ModifierData
{
    protected override void ApplyModifier(CardData[] cards)
    {
        foreach (var card in cards)
        {
            DeckManager.RemoveCard(card);
        }
    }
}
