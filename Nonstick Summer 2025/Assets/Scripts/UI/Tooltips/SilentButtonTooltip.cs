/*************************************************
* Author Names :          Toby
* Date Created :         8/5/2025
*   
***************************************************/

using UnityEngine;
using NaughtyAttributes;

public class SilentButtonTooltip : HoverTooltip
{
    [SerializeField, Required]
    private SilentButton silentButton;

    [SerializeField, ResizableTextArea]
    private string tooltipText = "Skip a turn for [SilentEnergy]";
    protected override string GetRawText()
    {
        return tooltipText;
    }

    protected override bool CanOpenTooltip()
    {
        return DialogueManager.ReadUserInput && DialogueManager.UserCanPlayCard;
    }
}
