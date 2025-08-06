/*************************************************
Author Names :          Cade, Naylor
Date Created :          June 20, 2025
Date Modified :         June 20, 2025
Brief Description :     Stores all objectives for the level
                        Updates the display when the condition is met

TODO :                  Link up Side Character when they exist
***************************************************/
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class Objectives : MonoBehaviour
{
    [SerializeField, Tooltip("Put objectives in the order they should be completed in. When an objective is complete, it will mark all objectives with a lower index than it complete.")] private ObjectiveData[] _conditions;
    [SerializeField, Required] private GameObject _objectiveCanvas;
    [SerializeField, Required] private TMP_Text _displayText;
    //[SerializeField, Required] private GameObject _objectiveIndicator;

    private void Start()
    {
        MetCondition(ObjectiveConditions.LEVEL_START);
        foreach(ObjectiveData od in _conditions)
        {
            if(od.TriggerCondition == ObjectiveConditions.INTERACT_WITH_OBJECT)
            {
                od.RequiredInteractions = od.RequiredObjects.Length;
                foreach(GameObject g in od.RequiredObjects)
                {
                    g.GetComponent<IInteractableObjective>()?.SetIsObjective(true);
                }
            }
        }
    }

    /// <summary>
    /// Called whenever a condition may have been met
    /// </summary>
    /// <param name="condition">The condition that has been met</param>
    public void MetCondition(ObjectiveConditions condition, GameObject obj = null)
    {
        for(int i=0; i<_conditions.Length; i++)
        {
            if (_conditions[i].TriggerCondition == condition && !_conditions[i].ConditionBeenMet)
            {
                // If the condition is interacting with a specific object and that object was not just interacted with, return
                // Otherwise the condition has been met; update the display
                if ((condition == ObjectiveConditions.INTERACT_WITH_OBJECT ||
                    condition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER) && System.Array.IndexOf(_conditions[i].RequiredObjects, obj) < 0)
                {
                    continue;
                }

                if(condition == ObjectiveConditions.INTERACT_WITH_OBJECT && (_conditions[i].CurrentInteractions + 1) < _conditions[i].RequiredInteractions)
                {
                    _conditions[i].CurrentInteractions++;
                    continue;
                }
                for(int j=0; j<i; j++)
                {
                    _conditions[j].ConditionBeenMet = true;
                    foreach(GameObject g in _conditions[j].RequiredObjects)
                    {
                        if(g!=null) g.GetComponent<IInteractableObjective>()?.ClearBlocker();
                        if (g != null) g.GetComponent<IInteractableObjective>()?.ClearAllShaders();
                    }
                }
                /*if (_conditions[i].HideNextObjectiveIfClear)
                {
                    // yes I know this is O(n*n*n) time
                    foreach (ObjectiveObjectLink oc in _conditions[i].ConditionsToHide)
                    {
                        for (int i = 0; i < _conditions.Length; i++)
                        {
                            if (_conditions[i].condition.TriggerCondition == oc.TriggerCondition)
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
                }*/
                Debug.LogWarning("Condition Met");
                _conditions[i].ConditionBeenMet = true;
                if (_displayText != null)
                    _displayText.text = _conditions[i].DisplayText;

                if (i < _conditions.Length-1)
                {
                    foreach(GameObject g in _conditions[i+1].RequiredObjects)
                    {
                        g.GetComponent<IInteractableObjective>()?.ClearBlocker();
                    }
                }
                else
                {
                    var bed = FindFirstObjectByType<OpenConfirmationInteractable>();
                    if (bed != null) bed.ClearBlocker();
                }
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
    [Tooltip ("A description of the Objective")] public string DisplayText;
    [Tooltip("What makes the above Objective Display")] public ObjectiveConditions TriggerCondition;

    [Tooltip("All objects that must be interacted with for this Objective to trigger the Display text. Order does not matter.")] [ShowIf(nameof(showOrHide))] [AllowNesting] 
        public GameObject[] RequiredObjects = new GameObject[0];    // Okay so for whatever reason this won't hide? Even though the format and everything is correct

    [HideInInspector] public int RequiredInteractions = 0;
    [HideInInspector] public int CurrentInteractions = 0;

    bool showOrHide => TriggerCondition == ObjectiveConditions.INTERACT_WITH_OBJECT
    || TriggerCondition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER;


    [HideInInspector] public bool ConditionBeenMet = false;
    //[Tooltip("Prevents the specified objectives from triggering")] public bool HideNextObjectiveIfClear;
    //[EnableIf("HideNextObjectiveIfClear"), Tooltip("What the objective to hide is triggered by"), AllowNesting] // i dont want this to appear unless the bool is true but it hates me for whatever reason
                                                                                                // no amount of 'allow nesting' worked :(
    //public ObjectiveObjectLink[] ConditionsToHide;



}
