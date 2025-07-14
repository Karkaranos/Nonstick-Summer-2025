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

        Close();
    }

    public void Open()
    {
        if(tooltipText != null)
            tooltipText.text = GetRawText() ;

        StaticUtilities.EnableCanvasGroup(tooltipGroup);
    }

    public void Close()
    { 
        StaticUtilities.DisableCanvasGroup(tooltipGroup);
    }

    /// <summary>
    /// Text before its [Style Tags] have been applied
    /// </summary>
    /// <returns></returns>
    protected abstract string GetRawText();

    public string GetText()
    {
        string text = GetRawText();

        text
            .Replace("[Assertive]", $"<color={ColorUtility.ToHtmlStringRGB(CardStyleManager.AssertiveStyle.color)}>{CardStyleManager.AssertiveStyle.DisplayName}</color>")
            .Replace("[Charming]", $"<color={ColorUtility.ToHtmlStringRGB(CardStyleManager.CharmingStyle.color)}>{CardStyleManager.CharmingStyle.DisplayName}</color>");
            .Replace("[Sappy]", $"<color={ColorUtility.ToHtmlStringRGB(CardStyleManager.SappyStyle.color)}>{CardStyleManager.SappyStyle.DisplayName}</color>");
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
