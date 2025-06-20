/*************************************************
Author Names :          Cade, Naylor
Date Created :          June 20, 2025
Date Modified :         June 20, 2025
Brief Description :     Stores all objectives for the level
                        Updates the display when the condition is met
***************************************************/
using UnityEngine;
using NaughtyAttributes;

public class Objectives : MonoBehaviour
{
    [SerializeField] private ObjectiveData _conditions;
}

[System.Serializable]
public class ObjectiveData
{
    public string DisplayText;
    public ObjectiveConditions Condition; 

    [ShowIf(nameof(showOrHide))]
    [AllowNesting]
    public GameObject _requiredObject;

    bool showOrHide => Condition == ObjectiveConditions.INTERACT_WITH_OBJECT 
        || Condition == ObjectiveConditions.TALK_TO_SIDE_CHARACTER;

    public enum ObjectiveConditions
    {
        LEVEL_START, LEAVE_BEDROOM, INTERACT_WITH_OBJECT, TALK_TO_SIDE_CHARACTER, FINISH_COMBAT
    }

}