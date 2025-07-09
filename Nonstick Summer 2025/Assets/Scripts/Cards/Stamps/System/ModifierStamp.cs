/*****************************************************************************
* File Name :         ModifierStamp.cs
* Author :            Toby
* Creation Date :     June 16, 2025
*
* Brief Description : Virtual class to be used inherited modifier _modifier scriptable
* objects.
*****************************************************************************/

using System;
using UnityEngine;
using NaughtyAttributes;

//[CreateAssetMenu(fileName = "Modifer Stamp", menuName = "Scriptable Objects/Stamp/...")]
public abstract class ModifierStamp : ScriptableObject
{
    public StampTriggerConditions TriggerCondition;

    [Header("Display")]
    [ShowAssetPreview(32,32)]
    public Sprite Icon;
    public string StampName;
    public string ShortDescription;

    public Type type => this.GetType();

    /// <summary>
    /// Invoked in CardStampCollection. this function should ideally have 1 reference besides overrides.
    /// </summary>
    public virtual void TryTriggerEffect(StampTriggerConditions reason, CardData affectedCard) // virtual in case the modifier needs to have some extra logic
    {
        if (TriggerCondition == reason)
            EffectTriggered(affectedCard);
    }

    /// <summary>
    /// This function is not invoked with stamps that are on cards by default. If an effects needs to be applied before/at
    /// game startup, then you should hardcode it to the CardData scriptable object
    /// </summary>
    public virtual void OnStampAdded(CardData affectedCard)
    {
        // no further behaviour unless you override this method
        Debug.Log("Stamp added.");
    }

    /// <summary>
    /// Performs a cards effect with absolute blind confidence that all of its conditions to activate have been met.
    /// </summary>
    /// <param name="affectedCard"></param>
    protected abstract void EffectTriggered(CardData affectedCard);
}
