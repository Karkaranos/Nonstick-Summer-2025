/*****************************************************************************
 * File Name :         MouseInteractionEvents.cs
 * Author :            Toby
 * Creation Date :     June 8, 2025
 *
 * Brief Description : Provides util events and variables for other scripts to
 * provide complicated mouse behavior.
 *
 *****************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;
using System.Collections;
using UnityEngine.Events;

public class MouseInteractionEvents : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver { get; private set; }
    public static MouseInteractionEvents CurrentHoverObject;

    public UnityEvent OnMouseDown, OnMouseHoverStart, OnMouseHoverEnd, OnMouseHoverStay;
    private const float deselectCurrentlyHoveringDelay = 0.1f;

    private Coroutine _deselctCurrentlyHoveringCoroutine;

    // maybe not be the best use of update. this may be a moment of weakness.
    private void Update()
    {
        if(mouseOver)
            OnMouseHoverStay.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIClickSFX);
        OnMouseDown.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(CurrentHoverObject != null && CurrentHoverObject != this)
            CurrentHoverObject.DeselectHover();

        if (_deselctCurrentlyHoveringCoroutine != null)
            StopCoroutine(_deselctCurrentlyHoveringCoroutine);

        CurrentHoverObject = this;
        mouseOver = true;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.UIHoverSFX);

        OnMouseHoverStart.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        StartCoroutine(TryDeselectCurrentlyHovering());
    }

    public void DeselectHover()
    {
        if (CurrentHoverObject == this)
        {
            CurrentHoverObject = null;
        }

        if (_deselctCurrentlyHoveringCoroutine != null)
        {
            StopCoroutine(_deselctCurrentlyHoveringCoroutine);
            _deselctCurrentlyHoveringCoroutine = null;
        }

        mouseOver = false;
        OnMouseHoverEnd.Invoke();
    }

    /// <summary>
    /// Close the tooltip after a cooldown to give the player time to move their mouse over to the tooltip if they want.
    /// Cuz like, GOD FORBID their mouse leave the ui element for even a single second right
    /// </summary>
    /// <returns></returns>
    IEnumerator TryDeselectCurrentlyHovering()
    {
        yield return new WaitForSeconds(deselectCurrentlyHoveringDelay);

        if (mouseOver)
            yield break; // exit early

        _deselctCurrentlyHoveringCoroutine = null;
        DeselectHover();
    }
}
