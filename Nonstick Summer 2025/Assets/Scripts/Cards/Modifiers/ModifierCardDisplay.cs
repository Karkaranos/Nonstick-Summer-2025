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
using System.Linq;

//[RequireComponent(typeof(MouseInteractionEvents))]
public partial class ModifierCardDisplay : MonoBehaviour
{
    [BoxGroup("UI Components"), SerializeField] Image IconImage;
    [BoxGroup("UI Components"), SerializeField] RectTransform cardBackground;
    [BoxGroup("UI Components"), SerializeField, Required] TMP_Text modifierHeader;
    [BoxGroup("UI Components"), Required] public RectTransform applyButtonAnchor;

    public ModifierData modifierData { get { return _modifier; } }
    public UnityEvent<ModifierCardDisplay> OnMouseDown = new UnityEvent<ModifierCardDisplay>(); 

    [SerializeField, Expandable]
    [Tooltip("Set this for debug only")]
    private ModifierData _modifier;

    [HideInInspector]
    public MouseInteractionEvents mouseInteraction;

    [HideInInspector]
    public int TargetSiblingIndex;
    [HideInInspector]
    public bool MarkedToBeDestroyed = false; // if the destruction animation is playing, pretty much.

    bool canPlayHover = true;
    private RectTransform rectTransform;

    private float randomSpriteRotation; // polish...


    private void Start()
    {
        // FUCK random.range what even is that? 
        randomSpriteRotation = Mathf.Lerp(-5, 5, Random.value);

        if (_modifier != null) SetCard(_modifier); // mostly for debugging

        mouseInteraction = GetComponent<MouseInteractionEvents>();
        mouseInteraction.OnMouseHoverStart.AddListener(OnMouseHoverStart);
        mouseInteraction.OnMouseHoverEnd.AddListener(OnMouseHoverEnd);
        rectTransform = GetComponent<RectTransform>();

        if (mouseInteraction != null)
        {
            mouseInteraction.OnMouseDown.AddListener(OnMouseDownStart);
        }
    }

    private void OnMouseDownStart()
    {
        Debug.Log(this.name + " clicked");
        OnMouseDown.Invoke(this);

        // TODO: repent
        // if you know me as a person at all, then you should know that i would only be writing
        // code like this as a very last resort.
        var decks = FindObjectsByType<ModifierDeckDisplay>(FindObjectsSortMode.None);
        foreach(var deck in decks)
        {
            deck.OnCardClicked(this);
        }
    }

    private void OnMouseHoverStart()
    {
        if (canPlayHover)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CardHoverSFX);
            canPlayHover = false;
        }

        // TODO: repent
        // if you know me as a person at all, then you should know that i would only be writing
        // code like this as a very last resort.
        var decks = FindObjectsByType<ModifierDeckDisplay>(FindObjectsSortMode.None);
        foreach (var deck in decks)
        {
            if(deck.filteredPlayerModifiers.Contains(this.modifierData))
            {
                deck.OnAnyCardHovered();
            }
        }

        // bring to front
        if (ModifierDeckDisplay.selectedCard != null)
        {
            // bring to (sploiler warning) 2nd to front
            ModifierDeckDisplay.selectedCard.transform.SetAsLastSibling();
        }
        transform.SetAsLastSibling();

        if (ModifierDeckDisplay.selectedCard != this)
        {
            StartCoroutine(HoverOverCardAnimation());
        }

    }

    private void OnMouseHoverEnd()
    {
        if (ModifierDeckDisplay.selectedCard == this)
        {
            return;
        }

        // bring to front
        transform.SetSiblingIndex(TargetSiblingIndex);

        hoverAnimationPlayed = false;
        canPlayHover = true;
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

        modifierHeader.text = _modifier.ModifierType.ToString().ToUpper();
        IconImage.sprite = _modifier.GetIcon();

        var rot = IconImage.transform.eulerAngles;
        IconImage.transform.eulerAngles = new Vector3(rot.x,rot.y, randomSpriteRotation);

        // maybe play a lil animation? (add a parameter?)
    }
}
