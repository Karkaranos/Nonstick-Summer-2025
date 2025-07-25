using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectBehavior : MonoBehaviour, IInteractableObjective
{
    [Required]
    public GameObject CanvasToOpen;

    [Header("Shaders")]
    [SerializeField] private bool applyShaders = false;
    [ShowIf("applyShaders"), SerializeField, Tooltip("Indicates this object is used in Objectives")] private bool applyObjectiveShaders;
    [ShowIf("applyShaders"), SerializeField, Tooltip("Indicates this object is interactable")] private bool applyInteractableShaders;
    [ShowIf("applyObjectiveShaders"), SerializeField, Tooltip("Only show the shader if this is part of the current objective")] private bool applyIfActiveObjective = true;
    [ShowIf("applyObjectiveShaders"), SerializeField] private Material objectiveShader;
    [ShowIf("applyInteractableShaders"), SerializeField] private Material interactableShader;
    [ShowIf("applyShaders"), SerializeField, Tooltip("GameObjects to affect")] private GameObject[] affectedMeshes;

    [Header("UI Text")]
    [SerializeField, Tooltip("Text that always appears when this object is selected")] private string _statement = "You are looking at an object";
    [SerializeField] private string _question = "Question not set.";
    [SerializeField] private PersonalityOption[] _options = new PersonalityOption[3];

    private bool hasGivenCard = false;
    private bool canBeInteractedWith = false;
    private bool isObjective = false;

    private GameObject openedCanvas;

    private OpenBossInteractable obi;
    [HideInInspector] public bool InteractSuccessful = false;

    private void Start()
    {
        obi = FindFirstObjectByType<OpenBossInteractable>(FindObjectsInactive.Include);

        if (applyShaders && ((applyObjectiveShaders && !applyIfActiveObjective) || applyInteractableShaders))
        {
            SetShader();
        }
    }



    public void Interact(GameObject player)
    {
        if((!isObjective || (isObjective && canBeInteractedWith)) && !hasGivenCard)
        {
            if (!GameManager.ObjectiveReference)
            {
                GameManager.ObjectiveReference = FindFirstObjectByType<Objectives>();
            }
            GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.INTERACT_WITH_OBJECT, gameObject);

            var canvas = UITransitionManager.OpenMenu(CanvasToOpen).GetComponent<InteractableObjectCanvas>();
            canvas.Initialize(_statement, _question, _options);

            InteractSuccessful = true;
            TryBoss();
            hasGivenCard = true;

            //Destroy(gameObject.GetComponent<InteractableObjectBehavior>());
            GetComponent<SpecialInteractBehavior>()?.CallSpecialInteraction();

            if(applyShaders)
                ClearAllShaders();
        }
    }


    /// <summary>
    /// Allows this object to be interacted with, if it is an objective
    /// </summary>
    public void ClearBlocker()
    {
        canBeInteractedWith = true;
        if (applyShaders && applyIfActiveObjective && !hasGivenCard)
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


   /* /// <summary>
    /// Gives player modifier cards based on their emotion choice
    /// </summary>
    /// <param name="emotion">The chosen emotion</param>
    public void OnClickInteractableObject(PersonalityOption PO)
    {
        foreach (ModifierData md in PO.ModifiersToGive)
        {
            ModifierManager.AddCard(md, true);
        }
    }*/


    public void TryBoss()
    {
        obi?.TryActivatingBoss(gameObject);
    }

    public void SetShader()
    {
        foreach(GameObject g in affectedMeshes)
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
                if(applyObjectiveShaders && (objectiveShader==null || m.name.Contains(objectiveShader.name)))
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

[System.Serializable]
/*************************************************
Author Names :          Cade Naylor
Date Created :          June 19, 2025
Date Modified :         June 19, 2025
Brief Description :     Stores information for interactable object questions
***************************************************/
public class PersonalityOption
{
    [Tooltip("Option text")]public string ButtonText = "not set";
    [Tooltip("An optional tint for the button. Leave white if not")]public Color ButtonColor = Color.white;
    [Tooltip("Insert all modifiers you want to give the player here based on certain emotions")] public List<ModifierData> ModifiersToGive;

}