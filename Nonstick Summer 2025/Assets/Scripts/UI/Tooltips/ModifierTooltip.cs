/*************************************************
* Author Names :          Toby
* Date Created :          7/13/2025
* Brief Description :     
*   
***************************************************/

using UnityEngine;

[RequireComponent(typeof(MouseInteractionEvents), typeof(ModifierCardDisplay))]
public class ModifierTooltip : HoverTooltip
{
    private ModifierCardDisplay cardDisplay;
    private ModifierData modifierData => cardDisplay.modifierData;

    protected override void Start()
    {
        base.Start(); // ! important
        cardDisplay = GetComponent<ModifierCardDisplay>();
    }

    protected override string GetRawText()
    {
        return modifierData.GetTooltipDescription();
    }
}
