/*************************************************
Author Names :          Cade Naylor
Date Created :          July 24, 2025
Date Modified :         July 24, 2025
Brief Description :     Contains script for special and silly things that occur after interactions
***************************************************/
using UnityEngine;

public class SpecialInteractBehavior : MonoBehaviour
{
    [SerializeField] private specialBehaviorType specialBehavior;
    [SerializeField] private GameObject affectedObject;
    /// <summary>
    /// Not all may be used; just threw in a few things to give options
    /// </summary>
    private enum specialBehaviorType
    {
        FILL_TOYBOX, REARRANGE_MAGNETS, SLICE_CAKE
    }

    private void Start()
    {
        if(specialBehavior == specialBehaviorType.FILL_TOYBOX)
        {
            affectedObject.SetActive(false);
        }
    }

    public void CallSpecialInteraction()
    {
        switch (specialBehavior)
        {
            case specialBehaviorType.FILL_TOYBOX:
                affectedObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
