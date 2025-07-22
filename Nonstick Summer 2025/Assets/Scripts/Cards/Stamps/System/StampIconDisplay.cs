/*************************************************
* Author Names :          Toby
* Date Created :          7/13/2025
* 
* Brief Description : Displays stamps
*   
***************************************************/

using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class StampIconDisplay : MonoBehaviour
{
    [SerializeField, Required]
    private Image icon;

    public ModifierStamp modifierStamp { get; private set; }

    private void Start()
    {
        RefreshDisplay();
    }

    public void SetStamp(ModifierStamp newStamp)
    {
        // newStamp will be null sometimes and that is good 
        modifierStamp = newStamp;
    }

    public void RefreshDisplay()
    {
        // hide if no stamp
        icon.color = modifierStamp == null ? Color.clear : Color.white;

        if(modifierStamp != null)
            icon.sprite = modifierStamp.Icon;
    }
}
