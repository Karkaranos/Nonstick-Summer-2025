/*****************************************************************************
* File Name :         ModifierCardDisplay.cs
* Author :            Toby
* Creation Date :     June 20, 2025
*
* Brief Description : lots of shared code with dialogueCardDisplay, but they will be more 
* differenter in the future.
*****************************************************************************/

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//[RequireComponent(typeof(MouseInteractionEvents))]
public partial class ModifierCardDisplay : MonoBehaviour
{
    [BoxGroup("UI Components")][SerializeField] Image IconImage;
    [BoxGroup("UI Components")][SerializeField] RectTransform cardBackground;

    public ModifierData modifierData { get { return _modifier; } }
    public UnityEvent<ModifierCardDisplay> OnMouseDown => new UnityEvent<ModifierCardDisplay>(); 

    [SerializeField, Expandable]
    [Tooltip("Set this for debug only")]
    private ModifierData _modifier;

    [HideInInspector]
    public MouseInteractionEvents mouseInteraction;
    private RectTransform rectTransform;

    private void Start()
    {
        if (_modifier != null) SetCard(_modifier); // mostly for debugging

        mouseInteraction = GetComponent<MouseInteractionEvents>();
        rectTransform = GetComponent<RectTransform>();

        if(mouseInteraction != null)
            mouseInteraction.OnMouseDown.AddListener(OnMouseDownStart);
    }

    private void OnMouseDownStart()
    {
        Debug.Log(this.name + " clicked");
        OnMouseDown.Invoke(this);

        // TODO: repent
        // if you know me as a person at all, then you should know that i would only be writing
        // code like this as a very last resort.
        var deck = FindFirstObjectByType<ModifierDeckDisplay>();
        deck.OnCardClicked(this); 
    }

    public void SetCard(ModifierData newModifier)
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
