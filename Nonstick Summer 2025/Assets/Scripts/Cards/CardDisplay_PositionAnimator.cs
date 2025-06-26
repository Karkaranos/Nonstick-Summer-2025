/*****************************************************************************
* File Name :         CardDisplay_PositionAnimator.cs
* Author :            Toby
* Creation Date :     June 22, 2025
*
* Brief Description : Cards have a base position, and a offset position. 
* Useful for cases when you might want a card to move up an inch when it is selected,
* but still remember where it will go when it is deselected.
* 
* Use these utility functions to automatically animate cards.
* 
* This script takes advantage of the fact that the entire card display is childed under
* an empty parent.
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public partial class CardDisplay : MonoBehaviour
{
    [Header("Position Animation")]

    [Tooltip("Canvas units/sec")]
    [SerializeField] private float MovementSpeed = 1500;

    private Vector2 basePosition, positionOffset=default;
    private Coroutine translatePositionCoroutine;

    public void SetCurrentPositionAsBase()
    {
        basePosition = cardBackground.anchoredPosition;
        positionOffset = Vector2.zero;
    }

    public void SetPositionAndOffsetNoAnimation(Vector2? position = null, Vector2? offset = null)
    {
        // real problem that happens sometimes
        if (this == null) return;

        rectTransform = rectTransform ?? GetComponent<RectTransform>();

        basePosition = position.HasValue ? position.Value : basePosition;
        positionOffset = offset.HasValue ? offset.Value : positionOffset;

        rectTransform.anchoredPosition = basePosition;
        cardBackground.anchoredPosition = positionOffset;
    }

    public void SetPosition(Vector2 position, float? speed = null)
    {
        // real problem that happens sometimes
        if (this == null) return;

        basePosition = position;

        if (translatePositionCoroutine != null)
        {
            if (speed != null)
                StopCoroutine(translatePositionCoroutine);
            else
                return;
        }

        translatePositionCoroutine = StartCoroutine(TranslatePosition(speed));
    }

    public void SetPositionAndOffset(Vector2? position = null, Vector2? offset = null, float? speed = null)
    {
        // real problem that happens sometimes
        if (this == null) return;

        basePosition = position.HasValue ? position.Value : basePosition;
        positionOffset = offset.HasValue ? offset.Value : positionOffset;

        if (translatePositionCoroutine != null)
        {
            if (speed != null)
                StopCoroutine(translatePositionCoroutine);
            else
                return;
        }

        // another real problem that happens sometimes
        this.gameObject.SetActive(true);

        translatePositionCoroutine = StartCoroutine(TranslatePosition(speed));
    }

    /// <summary>
    /// Animates the cards offset to 0,0,0
    /// </summary>
    public void ResetOffset(float? speed = null)
    {
        if (this == null) return;

        positionOffset = Vector2.zero;
        if (translatePositionCoroutine != null)
        {
            if (speed != null)
                StopCoroutine(translatePositionCoroutine);
            else
                return;
        }

        translatePositionCoroutine = StartCoroutine(TranslatePosition(speed));
    }

    private IEnumerator TranslatePosition(float? speed = null)
    {
        rectTransform = rectTransform ?? GetComponent<RectTransform>();

        speed = speed ?? MovementSpeed;

        // idek why i use var so much. i just see people smarter than me use it so that makes me wanna use it.
        var currentBasePosition = rectTransform.anchoredPosition;
        var currentOffset = cardBackground.anchoredPosition;

        while (currentBasePosition != basePosition || currentOffset != positionOffset)         // just learned using == on vectors actually does an approximate equals. so thats good thats what we want.
        {
            currentBasePosition = rectTransform.anchoredPosition;
            currentOffset = cardBackground.anchoredPosition;

            rectTransform.anchoredPosition = Vector2.MoveTowards(currentBasePosition, basePosition, speed.Value * Time.deltaTime);
            cardBackground.anchoredPosition = Vector2.MoveTowards(currentOffset, positionOffset, speed.Value * Time.deltaTime);
            yield return null;
        }

        translatePositionCoroutine = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
