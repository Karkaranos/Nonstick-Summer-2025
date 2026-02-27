/*************************************************
Author Names :          Cade
Date Created :          July 8, 2025
Date Modified :         July 8, 2025
Brief Description :     Handles functionality for interactable objects that are destroyed when interacted with
***************************************************/
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

    private Dictionary<GameObject, Material[]> originalMaterials = new();

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
            StartCoroutine(DestroyAnimation());

        }
    }
    public IEnumerator DestroyAnimation()
    {
        GetComponentsInChildren<Collider>().ForEach(x=> x.enabled = false);

        yield return StaticUtilities.AnimateScale(transform, Vector3.one * 1.25f, 0.3f, unscaledTime: true, lockBottomToYPosition: true, tExponent:0.25f);
        yield return StaticUtilities.AnimateScale(transform, Vector3.zero,        0.5f, unscaledTime: true, lockBottomToYPosition: true, tExponent:0.9f);
        yield return new WaitForEndOfFrame();

        Destroy(this.gameObject);
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
        var shaderToUse = interactableShader == null ? objectiveShader : interactableShader;

        if(shaderToUse == null)
        {
            Debug.LogError("No shader on " + gameObject.name);
        }

        foreach (GameObject g in affectedMeshes)
        {
            Renderer mr = g.GetComponent<Renderer>();
            List<Material> materials = new List<Material>();

            if (!originalMaterials.ContainsKey(g))
                originalMaterials.Add(g, mr.materials);

            for (int i = 0; i < mr.materials.Length; i++)
            {
                // not sure if applyObjectiveShader or applyInteractableShader should b used here
                var newMaterial = new Material(shaderToUse);
                var originalMaterial = originalMaterials[g][i];

                newMaterial.SetTexture("_MainTex", originalMaterial.mainTexture);
                newMaterial.SetColor("_BaseColor", originalMaterial.color);
                newMaterial.SetInt("_Cull", originalMaterial.GetInt("_Cull")); // face mode

                materials.Add(newMaterial);
            }
            mr.SetMaterials(materials);
        }
    }

    public void ClearAllShaders()
    {
        foreach (GameObject g in affectedMeshes)
        {
            Renderer mr = g.GetComponent<Renderer>();
            mr.SetMaterials(originalMaterials[g].ToList());
        }
    }
}
