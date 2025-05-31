/*
 * Tooltips display a bit of text when [the object this script is attached too] is hovered over.
 * In the future this script should dynamically generate tooltips based on certain criteria, for dynamic numbers and stuff.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string text;

    [Required]
    [SerializeField] private CanvasGroup tooltipGroup;
    private TMP_Text tooltipText;

    private static HoverTooltip currentTooltip;
    private bool mouseOver = false;

    void Start()
    {
        tooltipText = tooltipGroup.GetComponentInChildren<TMP_Text>();
        Close();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Open();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Close();
    }

    public void Open()
    {
        // disable other tooltip
        if (currentTooltip != null && currentTooltip != this)
            currentTooltip.Close();

        if(tooltipText != null)
            tooltipText.text = GetText() ;

        StaticUtilities.EnableCanvasGroup(tooltipGroup);

        mouseOver = true;
    }

    public void Close()
    { 
        if (currentTooltip == this)
            currentTooltip = null;

        StaticUtilities.DisableCanvasGroup(tooltipGroup);

        mouseOver = false;
    }

    public string GetText()
    {
        // better text getting system coming soon
        return text;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        // If user is selecting this gameobject
        var selected = UnityEditor.Selection.activeTransform;
        if (UnityEditor.Selection.activeTransform != null && (selected == this.transform || selected.IsChildOf(this.transform)))
            Open();
        else
            StaticUtilities.DisableCanvasGroup(tooltipGroup);

    }
}
