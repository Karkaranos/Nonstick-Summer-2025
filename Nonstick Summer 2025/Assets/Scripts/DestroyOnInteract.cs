/*************************************************
Author Names :          Cade
Date Created :          July 8, 2025
Date Modified :         July 8, 2025
Brief Description :     Handles functionality for interactable objects that are destroyed when interacted with
***************************************************/
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class DestroyOnInteract : MonoBehaviour, IInteractableObjective
{
    [HideInInspector] public bool InteractSuccessful = false;
    bool isObjective = false;
    bool canBeInteractedWith = false;
    OpenBossInteractable obi;

    [SerializeField] private bool applyShaders = false;
    [ShowIf("applyShaders"), SerializeField] private bool applyObjectiveShaders;
    [ShowIf("applyShaders"), SerializeField] private bool applyInteractableShaders;
    [ShowIf("applyObjectiveShaders"), SerializeField, Tooltip("Only show the shader if this is part of the current objective")] private bool applyIfActiveObjective = false;
    [ShowIf("applyObjectiveShaders"), SerializeField] private Material objectiveShader;
    [ShowIf("applyInteractableShaders"), SerializeField] private Material interactableShader;
    [ShowIf("applyShaders"), SerializeField] private GameObject[] affectedMeshes;

    private void Start()
    {
        obi = FindFirstObjectByType<OpenBossInteractable>(FindObjectsInactive.Include);

        if (applyShaders && !applyIfActiveObjective)
        {
            SetShader();
        }
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
        if (applyShaders && applyIfActiveObjective)
        {
            SetShader();
        }
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

    public void SetShader()
    {
        foreach (GameObject g in affectedMeshes)
        {
            Renderer mr = g.GetComponent<Renderer>();
            List<Material> allMats = new List<Material>();
            foreach (Material m in mr.materials)
            {
                allMats.Add(m);
            }
            if (applyObjectiveShaders && objectiveShader != null)
                allMats.Add(objectiveShader);
            if (applyInteractableShaders && interactableShader != null)
                allMats.Add(interactableShader);
            mr.SetMaterials(allMats);
        }
    }

    public void ClearAllShaders()
    {
        foreach (GameObject g in affectedMeshes)
        {
            Renderer mr = g.GetComponent<Renderer>();
            List<Material> baseMat = new List<Material>();
            foreach (Material m in mr.materials)
            {
                print(m.name);
                if (applyObjectiveShaders && (objectiveShader == null || m.name.Contains(objectiveShader.name)))
                {
                    continue;
                }
                if (applyInteractableShaders && (interactableShader == null || m.name.Contains(interactableShader.name)))
                {
                    continue;
                }
                baseMat.Add(m);
            }
            mr.SetMaterials(baseMat);
        }
    }
}
