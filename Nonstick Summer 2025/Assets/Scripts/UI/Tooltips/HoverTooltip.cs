/*************************************************
* Author Names :          Toby
* Date Created :          ?
* Brief Description :     Tooltips display a bit of text when [the object this script is attached too] is hovered over.
 * In the future this script should dynamically generate tooltips based on certain criteria, for dynamic numbers and stuff.
*   
***************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;
using System.Collections;
using System.Text.RegularExpressions;

[RequireComponent(typeof(MouseInteractionEvents))]
public abstract class HoverTooltip : MonoBehaviour
{
    //[ResizableTextArea]
    //[SerializeField] protected string text;

    [InfoBox("Possible style tags: [Assertive] [Charming] [Sappy] [Observation] [Question] [Expression]\n[Confidence]")]
    [Required("Make sure the tooltip has a canvas group attached")]
    [SerializeField] private CanvasGroup tooltipGroup;
    private TMP_Text tooltipText;

    private MouseInteractionEvents mouseInteraction;
    protected virtual void Start()
    {
        mouseInteraction = GetComponent<MouseInteractionEvents>();
        tooltipText = tooltipGroup.GetComponentInChildren<TMP_Text>();

        mouseInteraction.OnMouseHoverStart.AddListener(Open);
        mouseInteraction.OnMouseHoverEnd.AddListener(Close);
        mouseInteraction.OnMouseDown.AddListener(OnPlayerClickComponent);

        Close();
    }

    public void Open()
    {
        if(!CanOpenTooltip())
        {
            Close();
            return;
        }

        RefreshTooltipText();
    }

    public void Close()
    {
        tooltipGroup.gameObject.gameObject.SetActive(false);
        StaticUtilities.DisableCanvasGroup(tooltipGroup);
    }

    public void RefreshTooltipText()
    {
        if (tooltipText != null)
            tooltipText.text = GetText();

        tooltipGroup.gameObject.gameObject.SetActive(true);
        StaticUtilities.EnableCanvasGroup(tooltipGroup);
    }

    protected virtual void OnPlayerClickComponent()
    {
        // Crickets...
    }

    /// <summary>
    /// Text before its [Style Tags] have been applied
    /// </summary>
    /// <returns></returns>
    protected abstract string GetRawText();

    protected virtual bool CanOpenTooltip()
    {
        return true;
    }

    /// <summary>
    /// Reads through raw text and replaces certain keywords to have certain styles
    /// </summary>
    public string GetText()
    {
        string text = GetRawText();
        return TextUtilities.FilterText(text);
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
