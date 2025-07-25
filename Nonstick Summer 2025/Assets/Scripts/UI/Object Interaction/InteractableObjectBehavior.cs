using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectBehavior : MonoBehaviour, IInteractableObjective
{
    [Required]
    public GameObject CanvasToOpen;

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
    }


    public void Interact(GameObject player)
    {
        if((!isObjective || (isObjective && canBeInteractedWith)) && !hasGivenCard)
        {
            GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.INTERACT_WITH_OBJECT, gameObject);
            var canvas = UITransitionManager.OpenMenu(CanvasToOpen).GetComponent<InteractableObjectCanvas>();
            canvas.Initialize(_statement, _question, _options);

            InteractSuccessful = true;
            TryBoss();
            hasGivenCard = true;

            //Destroy(gameObject.GetComponent<InteractableObjectBehavior>());
            GetComponent<SpecialInteractBehavior>()?.CallSpecialInteraction();
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