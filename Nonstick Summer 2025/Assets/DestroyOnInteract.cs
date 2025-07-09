/*************************************************
Author Names :          Cade
Date Created :          July 8, 2025
Date Modified :         July 8, 2025
Brief Description :     Handles functionality for interactable objects that are destroyed when interacted with
***************************************************/
using UnityEngine;

public class DestroyOnInteract : MonoBehaviour, IInteractableObj
{
    [HideInInspector] public bool InteractSuccessful = false;
    bool isObjective = false;
    bool canBeInteractedWith = false;
    OpenBossInteractable obi;

    private void Start()
    {
        obi = FindFirstObjectByType<OpenBossInteractable>(FindObjectsInactive.Include);
    }

    public void Interact(GameObject player)
    {
        if (!isObjective || (isObjective && canBeInteractedWith))
        {
            player.GetComponent<Objectives>().MetCondition(ObjectiveConditions.INTERACT_WITH_OBJECT, gameObject);
            InteractSuccessful = true;
            TryBoss();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Allows this object to be interacted with, if it is an objective
    /// </summary>
    public void ClearBlocker()
    {
        canBeInteractedWith = true;
    }

    /// <summary>
    /// Sets whether this object is part of objectives
    /// </summary>
    /// <param name="objectiveStatus"></param>
    public void SetIsObjective(bool objectiveStatus)
    {
        isObjective = objectiveStatus;
    }
    
    public void TryBoss()
    {
        obi?.TryActivatingBoss(gameObject);
    }
}
