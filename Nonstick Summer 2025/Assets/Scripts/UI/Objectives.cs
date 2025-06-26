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
            if(od.TriggerCondition == condition && !od.ConditionBeenMet)
            {
                // If the condition is interacting with a specific object and that object was not just interacted with, return
                // Otherwise the condition has been met; update the display
                if((condition == ObjectiveConditions.INTERACT_WITH_OBJECT || 
                    condition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER) && obj != od.RequiredObject)
                {
                    print("Check failed");
                    return;
                }
                if(_displayText != null)
                    _displayText.text = od.DisplayText;
                od.ConditionBeenMet = true;
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
    LEVEL_START, LEAVE_BEDROOM, INTERACT_WITH_OBJECT, TALK_TO_SIDE_CHARACTER, FINISH_COMBAT
}

[System.Serializable]
public class ObjectiveData
{
    public string DisplayText;
    [Tooltip("What makes this objective appear")]public ObjectiveConditions TriggerCondition; 

    [ShowIf(nameof(showOrHide))]
    [AllowNesting]
    public GameObject RequiredObject;
    [HideInInspector] public bool ConditionBeenMet = false;

    bool showOrHide => TriggerCondition == ObjectiveConditions.INTERACT_WITH_OBJECT 
        || TriggerCondition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER;


}