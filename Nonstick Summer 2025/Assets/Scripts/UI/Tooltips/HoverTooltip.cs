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
using UnityEngine.UI;

[RequireComponent(typeof(MouseInteractionEvents))]
public abstract class HoverTooltip : MonoBehaviour
{
    //[ResizableTextArea]
    //[SerializeField] protected string text;

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

        Refresh();
    }

    public void Close()
    {
        tooltipGroup.gameObject.SetActive(false);
        if(tooltipGroup != null)
            StaticUtilities.DisableCanvasGroup(tooltipGroup);
    }

    private void Refresh()
    {
        var rectTransform = tooltipGroup.GetComponent<RectTransform>();
        if(rectTransform != null )
        {
            Debug.LogError("How is this null bruh");
            return;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        if(rectTransform.parent!=null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform.parent as RectTransform);
        }
    }

    public void RefreshTooltipText()
    {
        tooltipGroup.gameObject.SetActive(true);
        if (tooltipGroup != null)
            StaticUtilities.EnableCanvasGroup(tooltipGroup);

        var newText = GetText();
        if (tooltipText != null && newText != tooltipText.text)
            tooltipText.text = newText;

        StartCoroutine(DelayedLayoutUpdate());
    }

    /// <summary>
    /// this is fucking bullshit. it HAS to be the next frame.
    /// </summary>
    private IEnumerator DelayedLayoutUpdate()
    {
        yield return null; 

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tooltipGroup.transform);
    }

    protected virtual void OnPlayerClickComponent()
    {
        // Crickets...
        // why didnt i make this abstract bruhh
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

#if UNITY_EDITOR
        // If user is selecting this gameobject
        if (StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            Open();
        else if(tooltipGroup!= null)
            StaticUtilities.DisableCanvasGroup(tooltipGroup);
#endif
    }

    
}
