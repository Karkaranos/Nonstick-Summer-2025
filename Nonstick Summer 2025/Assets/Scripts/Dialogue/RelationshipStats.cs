using UnityEngine;
/*****************************************************************************
* File Name :         RelationshipStats.cs
* Author :            Sky
* Creation Date :     June 6, 2025
*
* Brief Description : Holds Stats for each Character' relationship status.
*
* TODO:
* 
* 
*****************************************************************************/
[System.Serializable]
public class RelationshipStats
{
    [Tooltip("Max value the relationship and slider can go.")]
    public float maxValue = 300;
    [Tooltip("Current relationship value")]
    public float currentValue = 0;
    [Tooltip("Amount required for a good ending, will likely have more quotas later.")]
    public float relationshipQuota = 75;
}
