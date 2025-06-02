/*
 * Tooltips display a bit of text when [the object this script is attached too] is hovered over.
 * In the future this script should dynamically generate tooltips based on certain criteria, for dynamic numbers and stuff.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;
using System.Collections;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [ResizableTextArea]
    [SerializeField] protected string text;

    [Required]
    [SerializeField] private CanvasGroup tooltipGroup;
    private TMP_Text tooltipText;

    private static HoverTooltip currentTooltip;
    private bool mouseOver;

    private const float clooseTooltipCooldown = 0.05f;
    private Coroutine closingTooltipCoroutine;

    void Start()
    {
        tooltipText = tooltipGroup.GetComponentInChildren<TMP_Text>();
        Close();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (closingTooltipCoroutine != null)
            StopCoroutine(closingTooltipCoroutine);

        Open();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver=false;
        StartCoroutine(TryCloseTooltipDelay());
    }

    public void Open()
    {
        // disable other tooltip
        if (currentTooltip != null && currentTooltip != this)
            currentTooltip.Close();

        if(tooltipText != null)
            tooltipText.text = GetText() ;

        StaticUtilities.EnableCanvasGroup(tooltipGroup);
    }

    public void Close()
    { 
        if (currentTooltip == this)
            currentTooltip = null;

        StaticUtilities.DisableCanvasGroup(tooltipGroup);
    }

    /// <summary>
    /// Close the tooltip after a cooldown to give the player time to move their mouse over to the tooltip if they want.
    /// Cuz like, GOD FORBID their mouse leave the ui element for even a single second right
    /// </summary>
    /// <returns></returns>
    IEnumerator TryCloseTooltipDelay()
    {
        yield return new WaitForSeconds(clooseTooltipCooldown);

        if (mouseOver)
            yield break; // exit early

        Close();
        closingTooltipCoroutine = null;
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
