using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField, Tooltip("Displays after interacting with this object before the response the player chose")] private string _response1 = "";
    [SerializeField, Tooltip("Displays after interacting with this object before the response the player chose")] private string _response2 = "";
    [SerializeField, Tooltip("Displays if object cannot be interacted with")] private string _cannotInteract = "You cannot interact with this yet.";
    [SerializeField] private PersonalityOption[] _options = new PersonalityOption[3];
    [SerializeField] private bool HideObjectAfterInteraction = false;

    private bool hasGivenCard = false;
    private bool canBeInteractedWith = false;
    private bool isObjective = false;

    private GameObject openedCanvas;

    private OpenBossInteractable obi;
    [HideInInspector] public bool InteractSuccessful = false;

     public string chosenOption;

    //hi fridge magnets
    public UnityEvent OnClickEvent1;
    public UnityEvent OnClickEvent2;
    public UnityEvent OnClickEvent3;

    private Dictionary<GameObject, Material[]> originalMaterials = new();

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
        var canvas = UITransitionManager.OpenMenu(CanvasToOpen).GetComponent<InteractableObjectCanvas>();
        if ((!isObjective || (isObjective && canBeInteractedWith)) && !hasGivenCard)
        {
            if (!GameManager.ObjectiveReference)
            {
                GameManager.ObjectiveReference = FindFirstObjectByType<Objectives>();
            }
            GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.INTERACT_WITH_OBJECT, gameObject);

            if (gameObject.name.Contains("Toy Box"))
            {

                GameObject.Find("Player Door").GetComponent<Animator>().enabled = true;

            }

            canvas.Initialize(_statement, _question, _options, gameObject);

            canvas.Button1.onClick.AddListener(() => OnClickEvent1.Invoke());
            canvas.Button2.onClick.AddListener(() => OnClickEvent2.Invoke());
            canvas.Button3.onClick.AddListener(() => OnClickEvent3.Invoke());

            InteractSuccessful = true;
            TryBoss();
            hasGivenCard = true;

            //Destroy(gameObject.GetComponent<InteractableObjectBehavior>());
            GetComponent<SpecialInteractBehavior>()?.CallSpecialInteraction();

            if(applyShaders)
                ClearAllShaders();
        }
        else if (!hasGivenCard)
        {
            canvas.InitializeWithBlocker(_statement, _cannotInteract);

        }
        else
        {
            canvas.InitializeAfterModifier(_statement, _response1, _response2, chosenOption);
        }

        GetComponent<SpecialInteractBehavior>()?.CallSpecialInteraction();
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
            List<Material> materials = new List<Material>();

            if(!originalMaterials.ContainsKey(g))
                originalMaterials.Add(g, mr.materials);

            for(int i=0; i<mr.materials.Length; i++)
            {
                // not sure if applyObjectiveShader or applyInteractableShader should b used here
                var shaderToUse = interactableShader == null ? objectiveShader : interactableShader;
                var newMaterial = new Material(shaderToUse);
                var originalMaterial = originalMaterials[g][i];

                newMaterial.SetTexture("_MainTex", originalMaterial.mainTexture);
                newMaterial.SetColor("_BaseColor", originalMaterial.color);
                newMaterial.SetInt("_Cull", originalMaterial.GetInt("_Cull")); // face mode

                materials.Add(newMaterial);
            }
            mr.SetMaterials(materials);

            /*foreach (Material m in mr.materials)
            {
                allMats.Add(m);
            }
            if (applyObjectiveShaders && objectiveShader != null)
                allMats.Add(objectiveShader);
            if (applyInteractableShaders && interactableShader != null)
                allMats.Add(interactableShader);
            mr.SetMaterials(allMats);*/
        }
    }

    public void ClearAllShaders()
    {
        foreach (GameObject g in affectedMeshes)
        {
            Renderer mr = g.GetComponent<Renderer>();
            mr.SetMaterials(originalMaterials[g].ToList());

            /*List<Material> baseMat = new List<Material>();
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
            mr.SetMaterials(baseMat);*/

            // hide object
            if (HideObjectAfterInteraction)
            {
                /*var filter = g.GetComponent<MeshFilter>();
                if (filter != null)
                    filter.mesh = null;*/
                //mr.enabled = false;
                //Destroy(mr.gameObject);
                //Destroy(gameObject);
                StartCoroutine(DestroyAnimation());
            }
        }

        
    }

    public IEnumerator DestroyAnimation()
    {
        GetComponentsInChildren<Collider>().ForEach(x => x.enabled = false);

        yield return StaticUtilities.AnimateScale(transform, transform.localScale * 1.25f, 0.3f, unscaledTime: true, lockBottomToYPosition: true, tExponent: 0.25f);
        yield return StaticUtilities.AnimateScale(transform, Vector3.zero, 0.5f, unscaledTime: true, lockBottomToYPosition: false, tExponent: 0.9f);
        yield return new WaitForEndOfFrame();

        Destroy(this.gameObject);
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