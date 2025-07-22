/*************************************************
* Author Names :          DialogueCardTooltip
* Date Created :          7/17/25
* Brief Description :    
*   
***************************************************/

using System;
using UnityEngine;

public class DialogueCardTooltip : HoverTooltip
{
    [SerializeField]
    private CardDisplay cardDisplay;

    private CardData data => cardDisplay.cardData;

    // this one is hardcoded unfortunately
    //[SerializeField] 
    //private string cardTooltipText;
    protected override string GetRawText()
    {
        return $"A [{data.Emotion.ToString()}] [{data.Intention.ToString()}] card\nCan be spoken during a conversation";
    }
}
