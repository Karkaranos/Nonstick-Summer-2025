/*****************************************************************************
* File Name :         DestroyCardModifier.cs
* Author :            Toby
* Creation Date :     July 13 2025
*
* Brief Description : Modifier that destroys inputted card/cards
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "EmotionChangeModifier", menuName = "Modifier Card/Destroy Card")]
public class DuplicateCardModifier : ModifierData
{
    [SerializeField, Min(1)]
    private int CopiesToMake = 1;

    protected override void ApplyModifier(CardData[] cards)
    {
        foreach (var card in cards)
        {
            for(int i = 0; i < CopiesToMake; i++)
            {
                DeckManager.AddCardCopy(card);
            }
        }
    }
}
