/*************************************************
Author Names :          Cade, Naylor
Date Created :          June 20, 2025
Date Modified :         June 20, 2025
Brief Description :     Stores all objectives for the level
                        Updates the display when the condition is met

TODO :                  Link up Side Characters when they exist
***************************************************/
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class Objectives : MonoBehaviour
{
    [SerializeField] private ObjectiveData[] _conditions;
    [SerializeField, Required] private GameObject _objectiveCanvas;
    [SerializeField, Required] private TMP_Text _displayText;

    private void Start()
    {
        MetCondition(ObjectiveConditions.LEVEL_START);
    }

    /// <summary>
    /// Called whenever a condition may have been met
    /// </summary>
    /// <param name="condition">The condition that has been met</param>
    public void MetCondition(ObjectiveConditions condition, GameObject obj = null)
    {
        foreach(ObjectiveData od in _conditions)
        {
            if(od.condition.TriggerCondition == condition && !od.ConditionBeenMet)
            {
                // If the condition is interacting with a specific object and that object was not just interacted with, return
                // Otherwise the condition has been met; update the display
                if((condition == ObjectiveConditions.INTERACT_WITH_OBJECT || 
                    condition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER) && obj != od.condition.RequiredObject)
                {
                    continue;
                if(od.HideNextObjectiveIfClear)
                {
                    // yes I know this is O(n*n*n) time
                    foreach(ObjectiveObjectLink oc in od.ConditionsToHide)
                    {
                        for(int i=0; i<_conditions.Length; i++)
                        {
                            if(_conditions[i].condition.TriggerCondition == oc.TriggerCondition)
                            {
                                if ((oc.TriggerCondition == ObjectiveConditions.INTERACT_WITH_OBJECT ||
                                    oc.TriggerCondition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER) && 
                                    _conditions[i].condition.RequiredObject != oc.RequiredObject)
                                {
                                    continue;
                                }
                                _conditions[i].ConditionBeenMet = true;
                                print("yay");

                            }
                        }
                    }
                }
                od.ConditionBeenMet = true;
                if (_displayText != null)
                    _displayText.text = od.DisplayText;
            }
        }
    }

    /// <summary>
    /// Handles the objective's visibility
    /// </summary>
    /// <param name="visibility">Bool denoting whether the objective should be visible or not</param>
    public void SetObjectiveVisibility(bool visibility)
    {
        if (_objectiveCanvas != null)
            _objectiveCanvas?.SetActive(visibility);
        //else
            //Debug.LogError("No objective Canvas");
    }


}

public enum ObjectiveConditions
{
    LEVEL_START, LEAVE_BEDROOM, INTERACT_WITH_OBJECT, TALK_TO_SIDE_CHARACTER, FINISH_COMBAT, NONE
}

[System.Serializable]
public class ObjectiveData
{
    public string DisplayText;
    public ObjectiveObjectLink condition;


    [HideInInspector] public bool ConditionBeenMet = false;
    [Tooltip("Prevents the specified objectives from triggering")] public bool HideNextObjectiveIfClear;
    [EnableIf("HideNextObjectiveIfClear"), Tooltip("What the objective to hide is triggered by"), AllowNesting] // i dont want this to appear unless the bool is true but it hates me for whatever reason
                                                                                                // no amount of 'allow nesting' worked :(
    public ObjectiveObjectLink[] ConditionsToHide;



}

[System.Serializable]
public class ObjectiveObjectLink
{
    [Tooltip("What makes this objective appear")]public ObjectiveConditions TriggerCondition;
    [ShowIf(nameof(showOrHide))]
    [AllowNesting]
    public GameObject RequiredObject;

    bool showOrHide => TriggerCondition == ObjectiveConditions.INTERACT_WITH_OBJECT
    || TriggerCondition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER;
}