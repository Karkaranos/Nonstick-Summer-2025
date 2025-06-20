/*****************************************************************************
* File Name :         CardStatAffectorStamp.cs
* Author :            Toby
* Creation Date :     June 16, 2025
*
* Brief Description : 
*****************************************************************************/

using UnityEngine;

[CreateAssetMenu(fileName = "StatChange", menuName = "Scriptable Objects/Stamps/Stat Change")]
public class CardStatAffectorStamp : ModifierStamp
{
    [Header("Parameters")]
    [SerializeField] private StatToModify stat;
    [SerializeField] private StatOperator mode;
    [SerializeField] private float value = 0;

    // these could have been two seperate scripts.
    // idk why i didnt feel like it.
    public enum StatToModify
    {
        EnergyCost,
        Relationship
    }

    public enum StatOperator
    {
        Add,
        Multiply,
    }

    // this could in theory be moved to 
    public void ModifyEnergyCost(ref float currentEnergyCost)
    {
        if (stat != StatToModify.EnergyCost)
            return;

        switch(mode)
        {
            case StatOperator.Add:
                currentEnergyCost += value;
                break;
            case StatOperator.Multiply:
                currentEnergyCost *= value;
                break;
            default:
                currentEnergyCost = -1;
                Debug.LogError("Invalid operator");
                break;
        }
        //Debug.LogError($"Energy cost changed to {currentEnergyCost}");
    }

    // this could in theory be moved to 
    public void ModifyRelationshipValue(ref float currentRelationshipChange)
    {
        if (stat != StatToModify.Relationship)
            return;

        switch (mode)
        {
            case StatOperator.Add:
                currentRelationshipChange += value;
                break;
            case StatOperator.Multiply:
                currentRelationshipChange *= value;
                break;
            default:
                currentRelationshipChange = -1;
                Debug.LogError("Invalid operator");
                break;
        }
        Debug.LogError($"Relationship changed set to {currentRelationshipChange}");
    }

    protected override void EffectTriggered(CardData affectedCard)
    {
        // nothing needs to happen, actually.
        return;
    }
}
