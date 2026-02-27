/*************************************************
* Author Names :          DialogueCardTooltip
* Date Created :          7/17/25
* Brief Description :    
*   
***************************************************/

using NaughtyAttributes;
using System;
using UnityEngine;

public class DialogueCardTooltip : HoverTooltip
{
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField, ResizableTextArea]
    private string tooltipText = "A [Emotion] [Intention] card.\nCan be spoken during a conversation for [EnergyCost]";

    private CardData data => cardDisplay.cardData;


    // this one is hardcoded unfortunately
    //[SerializeField] 
    //private string cardTooltipText;
    protected override string GetRawText()
    {
        return tooltipText
            .Replace("[Emotion]", $"[{data.Emotion.ToString()}]")
            .Replace("[Intention]", $"[{data.Intention.ToString()}]")
            .Replace("[EnergyCost]", data.EnergyCost.AddSignToString(additonalText: "<sprite name=\"Energy\"> energy"));
    }
}
