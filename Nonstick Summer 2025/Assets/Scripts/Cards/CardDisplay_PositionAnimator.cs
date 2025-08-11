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
using Unity.VisualScripting;

public partial class CardDisplay : MonoBehaviour
{
    [Header("Position Animation")]

    [Tooltip("Canvas units/sec")]
    [SerializeField] private float MovementSpeed = 1500;
    [SerializeField] private float RefreshCardTime = 0.4f;
    [SerializeField] private bool animateWaves = true;
    [ShowIf(nameof(animateWaves)), SerializeField] float waveHeight = 3;
    [ShowIf(nameof(animateWaves)), SerializeField] float waveSpeed = 1;

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

        while (currentBasePosition != basePosition || currentOffset != positionOffset || animateWaves) // just learned using == on vectors actually does an approximate equals. so thats good thats what we want.
        {
            currentBasePosition = rectTransform.anchoredPosition;
            currentOffset = cardBackground.anchoredPosition;

            rectTransform.anchoredPosition = Vector2.MoveTowards(currentBasePosition, basePosition, speed.Value * Time.deltaTime);
            if (animateWaves)
                cardBackground.anchoredPosition = Vector2.MoveTowards(currentOffset, positionOffset + new Vector2(0, Mathf.Sin((Time.time * waveSpeed) + TargetSiblingIndex) * waveHeight), speed.Value * Time.deltaTime);
            else
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

    #region Specific hardcoded animations

    [Foldout("Destroy Card Animation"), SerializeField] float targetRotation = -135f;
    [Foldout("Destroy Card Animation"), SerializeField] float destroyAnimationSeconds = 0.5f;
    public IEnumerator UseCardAnimation(bool destroyAfter = true)
    {
        if (destroyAfter)
            MarkedToBeDestroyed = true;

        if (translatePositionCoroutine != null)
            StopCoroutine(translatePositionCoroutine);

        // some kind of dithering / burning shader would be sooooo cool here 

        var startRotation = transform.eulerAngles;
        var startScale = transform.localScale;

        float timeStarted = Time.time;
        float t;

        do
        {
            t = (Time.time - timeStarted) / destroyAnimationSeconds;

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
            transform.eulerAngles = new Vector3(startRotation.x, startRotation.y, Mathf.Lerp(startRotation.z, targetRotation, t));

            yield return null;
        }
        while(t < 1 && transform != null);


        if(destroyAfter)
        {
            Destroy(gameObject);
        }
    }

    [Foldout("Hover Card Animation"), SerializeField] float hoverTargetRotation = -2f;
    [Foldout("Hover Card Animation"), SerializeField] float hoverCardAnimationSeconds = 0.2f;
    private bool hoverAnimationPlayed = false;
    private bool hoverAnimationRunning = false;
    public IEnumerator HoverOverCardAnimation()
    {
        if(hoverAnimationRunning || hoverAnimationPlayed)
            yield break;

        hoverAnimationRunning = true;
        hoverAnimationPlayed = true;

        // some kind of dithering / burning shader would be sooooo cool here 

        var startRotation = transform.eulerAngles;

        float timeStarted = Time.time;
        float t;

        do
        {
            t = (Time.time - timeStarted) / (hoverCardAnimationSeconds/2);
            transform.eulerAngles = new Vector3(startRotation.x, startRotation.y, Mathf.Lerp(startRotation.z, hoverTargetRotation, t));

            yield return null;
        }
        while (t < 1 && transform != null);

        timeStarted = Time.time;

        do
        {
            t = (Time.time - timeStarted) / (hoverCardAnimationSeconds / 2);
            transform.eulerAngles = new Vector3(startRotation.x, startRotation.y, Mathf.Lerp(startRotation.z, 0, t));

            yield return null;
        }
        while (t < 1 && transform != null);

        hoverAnimationRunning = false;
    }

    #endregion
}
