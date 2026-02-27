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
    [SerializeField]
    private StampIconDisplay stampDisplay;
    private ModifierStamp stampData => stampDisplay.modifierStamp;

    protected override void Start()
    {
        base.Start(); // ! important
    }

    protected override bool CanOpenTooltip()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return true; // for debug ;)
#endif

        return stampData != null;
    }

    protected override string GetRawText()
    {
        return stampData == null ? "No Stamp! If youre reading this, there's a problem" : 
            $"[{stampData.StampName}]\n{stampData.ShortDescription}";
    }


}
