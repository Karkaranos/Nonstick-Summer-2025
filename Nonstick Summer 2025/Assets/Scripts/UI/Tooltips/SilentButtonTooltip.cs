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

    [Header("Archipelago")]
    [SerializeField] public ArchipelagoItem archipelagoItem = ArchipelagoItem.SilentButton;
    [SerializeField, ResizableTextArea]
    private string archipelagoTooltip = "Silent button is not unlocked in the multiworld!";
    private bool apItemUnlocked => APInventoryService.Instance.IsItemCollected(archipelagoItem);

    protected override string GetRawText()
    {
        if (!apItemUnlocked) return archipelagoTooltip;

        return tooltipText;
    }

    protected override bool CanOpenTooltip()
    {
        return DialogueManager.ReadUserInput && DialogueManager.UserCanPlayCard;
    }
}
