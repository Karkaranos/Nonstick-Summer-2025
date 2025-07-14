/*************************************************
* Author Names :          Toby
* Date Created :          7/13/2025
* Brief Description :     
*   
***************************************************/

using UnityEngine;

[RequireComponent(typeof(MouseInteractionEvents))]
public class StampTooltip : HoverTooltip
{
    private ModifierCardDisplay cardDisplay;
    private ModifierData modifierData => cardDisplay.modifierData;

    protected override void Start()
    {
        base.Start(); // ! important
    }

    public override string GetText()
    {
        return modifierData.GetTooltipDescription();
    }
}
