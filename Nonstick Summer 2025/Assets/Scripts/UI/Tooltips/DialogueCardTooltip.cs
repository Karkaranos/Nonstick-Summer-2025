/*************************************************
* Author Names :          DialogueCardTooltip
* Date Created :          ?
* Brief Description :    
*   
***************************************************/


using UnityEngine;

public class DialogueCardTooltip : HoverTooltip
{
    [SerializeField]
    private string tooltipText;
    protected override string GetRawText()
    {
        return tooltipText;
    }
}
