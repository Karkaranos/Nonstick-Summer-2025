/*
 * Tooltips display a bit of text when [the object this script is attached too] is hovered over.
 * In the future this script should dynamically generate tooltips based on certain criteria, for dynamic numbers and stuff.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;
using System.Collections;

[RequireComponent(typeof(MouseInteractionEvents))]
public class HoverTooltip : MonoBehaviour
{
    [ResizableTextArea]
    [SerializeField] protected string text;

    [Required]
    [SerializeField] private CanvasGroup tooltipGroup;
    private TMP_Text tooltipText;

    private MouseInteractionEvents mouseInteraction;


    void Start()
    {
        mouseInteraction = GetComponent<MouseInteractionEvents>();
        tooltipText = tooltipGroup.GetComponentInChildren<TMP_Text>();
        Close();

        mouseInteraction.OnMouseHoverStart.AddListener(Open);
        mouseInteraction.OnMouseHoverEnd.AddListener(Close);
    }

    public void Open()
    {
        if(tooltipText != null)
            tooltipText.text = GetText() ;

        StaticUtilities.EnableCanvasGroup(tooltipGroup);
        Debug.Log("open");
    }

    public void Close()
    { 
        StaticUtilities.DisableCanvasGroup(tooltipGroup);
        Debug.Log("close");

    }

    public virtual string GetText()
    {
        // better text getting system coming soon
        return text;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        // If user is selecting this gameobject
        if (StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            Open();
        else
            StaticUtilities.DisableCanvasGroup(tooltipGroup);

    }

    
}
