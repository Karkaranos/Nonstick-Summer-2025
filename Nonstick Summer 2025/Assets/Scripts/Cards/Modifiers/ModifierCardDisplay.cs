/*****************************************************************************
* File Name :         ModifierCardDisplay.cs
* Author :            Toby
* Creation Date :     June 20, 2025
*
* Brief Description : 
*****************************************************************************/

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(MouseInteractionEvents))]
public class ModifierCardDisplay : MonoBehaviour
{
    [BoxGroup("UI Components")][SerializeField] Image IconImage;

    public ModifierData modifierData { get { return _modifier; } }
    public UnityEvent OnModifierClicked => mouseInteraction.OnMouseDown; 

    [SerializeField, Expandable]
    [Tooltip("Set this for debug only")]
    private ModifierData _modifier;

    private MouseInteractionEvents mouseInteraction;

    private void Start()
    {
        if (_modifier != null) SetModifier(_modifier); // mostly for debugging

        mouseInteraction = GetComponent<MouseInteractionEvents>();

        mouseInteraction.OnMouseHoverStart.AddListener(OnMouseHoverStart);
        mouseInteraction.OnMouseHoverEnd.AddListener(OnMouseHoverEnd);
        mouseInteraction.OnMouseDown.AddListener(OnMouseDownStart);
    }

    private void OnMouseHoverStart() // this should be moved to another script
    {
        
    }

    private void OnMouseHoverEnd() // this should be moved to another script
    {
        
    }

    private void OnMouseDownStart()
    {

    }

    public void SetModifier(ModifierData newModifier)
    {
        if(newModifier== null)
        {
            Debug.LogError("Dont let this happen.");
            return;
        }

        //if (_modifier != null)
        //    ... something that happens when modifier is replaced

        _modifier = newModifier;

        RefreshDisplay();
    }

    [Button]
    public void RefreshDisplay()
    {
        if (_modifier == null)
        {
            Debug.LogWarning("No modifier is set.");
            return;
        }

        IconImage.sprite = _modifier.GetIcon();

        // maybe play a lil animation? (add a parameter?)
    }
}
