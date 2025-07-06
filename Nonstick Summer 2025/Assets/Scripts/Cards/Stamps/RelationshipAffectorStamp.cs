/*****************************************************************************
* File Name :         RelationshipAffectorStamp.cs
* Author :            Toby
* Creation Date :     June 16, 2025
*
* Brief Description : 
*****************************************************************************/

using UnityEngine;

[CreateAssetMenu(fileName = "StatChange", menuName = "Scriptable Objects/Stamp/Stat Change")]
public class RelationshipAffectorStamp : ModifierStamp
{
    [Header("Parameters")]
    [SerializeField] private StatOperator mode;
    [SerializeField] private float value = 0;

    public void ModifyRelationshipValue(ref float currentRelationshipChange)
    {
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
