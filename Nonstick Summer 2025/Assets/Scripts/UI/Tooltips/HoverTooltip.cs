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

        Close();
    }

    public void Open()
    {
        if(!CanOpenTooltip())
        {
            Close();
            return;
        }

        if(tooltipText != null)
            tooltipText.text = GetText() ;

        tooltipGroup.gameObject.gameObject.SetActive(true);
        StaticUtilities.EnableCanvasGroup(tooltipGroup);
    }

    public void Close()
    {
        tooltipGroup.gameObject.gameObject.SetActive(false);
        StaticUtilities.DisableCanvasGroup(tooltipGroup);
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
        // TODO: move this function to different script (so it can be used with npc text)

        string text = GetRawText();

        // i hope this is not extremely slow
        text = text
            // emotions
            .Replace("[Assertive]", $"<color=#{ColorUtility.ToHtmlStringRGB(CardStyleManager.AssertiveStyle.color)}>{CardStyleManager.AssertiveStyle.DisplayName}</color>")
            .Replace("[Charming]", $"<color=#{ColorUtility.ToHtmlStringRGB(CardStyleManager.CharmingStyle.color)}>{CardStyleManager.CharmingStyle.DisplayName}</color>")
            .Replace("[Sappy]", $"<color=#{ColorUtility.ToHtmlStringRGB(CardStyleManager.SappyStyle.color)}>{CardStyleManager.SappyStyle.DisplayName}</color>")
            // intentions
            .Replace("[Expression]", $"<color=#{ColorUtility.ToHtmlStringRGB(CardStyleManager.ExpressionStyle.color)}>{CardStyleManager.ExpressionStyle.DisplayName}</color>")
            .Replace("[Observation]", $"<color=#{ColorUtility.ToHtmlStringRGB(CardStyleManager.ObservationStyle.color)}>{CardStyleManager.ObservationStyle.DisplayName}</color>")
            .Replace("[Question]", $"<color=#{ColorUtility.ToHtmlStringRGB(CardStyleManager.QuestionStyle.color)}>{CardStyleManager.QuestionStyle.DisplayName}</color>")
            // stamps
            .ReplaceTagColor("Stamp", ColorUtility.ToHtmlStringRGB(GameManager.Instance.StampTooltipColor));

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
