/*****************************************************************************
* File Name :         DrawExtraCardStamp.cs
* Author :            Toby
* Creation Date :     June 16, 2025
*
* Brief Description : 
*****************************************************************************/

using UnityEngine;

[CreateAssetMenu(fileName = "StatChange", menuName = "Scriptable Objects/Stamp/Draw Extra Card")]
public class DrawExtraCardStamp : ModifierStamp
{
    [SerializeField, Min(1)] private int NumCardsToDraw = 1;

    

    protected override void EffectTriggered(CardData affectedCard)
    {
        if (DialogueUIController.Instance != null)
        {
            for (int i = 0; i < NumCardsToDraw; i++)
            {
                DialogueUIController.Instance.DeckDisplay.DrawOneCard();
            }
        }
    }
    
    public override void BeforeCardDrawnFromDeck(CardData affectedCard)
    {
    }
}
