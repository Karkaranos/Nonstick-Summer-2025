/*****************************************************************************
* File Name :         DuplicateCardModifier.cs
* Author :            Toby
* Creation Date :     July 13 2025
*
* Brief Description : Modifier that duplicates inputted card/cards
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "DuplicateCardModifier", menuName = "Modifier Card/Duplicate Card")]
public class DuplicateCardModifier : ModifierData
{
    [SerializeField, Min(1)]
    private int CopiesToMake = 1;

    public override string GetModifierName()
    {
        return this.name;
    }

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
