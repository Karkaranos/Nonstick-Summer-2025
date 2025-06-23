/*****************************************************************************
* File Name :         CardDisplay_PositionAnimator.cs
* Author :            Toby
* Creation Date :     June 22, 2025
*
* Brief Description : Partial Class for CardDisplay. Has util functions for other
* scripts to animate its position
* 
* This script takes advantage of the fact that the entire card display is childed under
* an empty parent.
* 
* TODO: if this script is giving problems, make sure that the card display background
* is centered at 0,0. otherwise, you will need to store the card backgrounds original position.
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public partial class CardDisplay : MonoBehaviour
{
    [Header("Position Animation")]

    [Tooltip("Canvas units/sec")]
    [SerializeField] private float MovementSpeed = 1000;

    private Vector2 basePosition, positionOffset;
    private Vector2 targetPosition => basePosition + positionOffset; // may be expensive to calc this every time.
    private Coroutine translatePositionCoroutine;

    public void UpdatePosition()
    {
        basePosition = cardBackground.anchoredPosition;
    }

    public void SetPosition(Vector2 position, Vector2? offset=null)
    {
        // real problem that happens sometimes
        if (this == null) return;

        basePosition = position;
        positionOffset = offset ?? position;

        if (translatePositionCoroutine != null)
            return; // targetposition is updated, and the coroutine is moving towards that, so no need to start new coroutine.

        translatePositionCoroutine = StartCoroutine(TranslatePosition());
    }

    /// <summary>
    /// Animates the cards position
    /// </summary>
    public void SetPositionOffset(Vector2 offset)
    {
        if (this == null) return;

        positionOffset = offset;
        if (translatePositionCoroutine != null)
            return; // targetposition is updated, and the coroutine is moving towards that, sooo who cares?

        translatePositionCoroutine = StartCoroutine(TranslatePosition());
    }

    /// <summary>
    /// Animates the cards offset to 0,0,0
    /// </summary>
    public void ResetOffset()
    {
        if (this == null) return;

        positionOffset = Vector2.zero;
        if (translatePositionCoroutine != null)
            return; // targetposition is updated, and the coroutine is moving towards that, so no need to start new coroutine.

        translatePositionCoroutine = StartCoroutine(TranslatePosition());
    }

    private IEnumerator TranslatePosition()
    {
        // idek why i use var so much. i just see people smarter than me use it so that makes me wanna use it.
        var current = cardBackground.anchoredPosition;

        while (current != targetPosition)         // just learned using == on vectors actually does an approximate equals. so thats good thats what we want.
        {
            current = cardBackground.anchoredPosition;
            cardBackground.anchoredPosition = Vector2.MoveTowards(current, targetPosition, MovementSpeed * Time.deltaTime);
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
