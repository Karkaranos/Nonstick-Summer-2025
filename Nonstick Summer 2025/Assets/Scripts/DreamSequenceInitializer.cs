using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;
using System.Collections;
/*****************************************************************************
* File Name :         DreamSequence.cs
* Author :            Sky
* Creation Date :     July 11, 2025
*
* Brief Description :  Controls canvas anD actions During Dream Sequence.
* 
*****************************************************************************/
public class DreamSequenceInitializer : MonoBehaviour
{
    [Required]
    public GameObject CanvasToOpen;


    [SerializeField][Required] private Image ImageToFade;
    [SerializeField] private float lengthOfFade = 3;
    private Coroutine fadeInCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fadeInCoroutine == null)
        {
            ImageToFade.enabled = true;
            fadeInCoroutine = StartCoroutine(FadeIn(ImageToFade));
        }

        UITransitionManager.OpenMenu(CanvasToOpen);
    }


    /// <summary>
    /// Fades in selected image (starts at alpha 1, ends at alpha 0)
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(Image image)
    {
        float alpha = image.color.a;
        Color color = image.color;

        while (image.color.a > 0)
        {
            float t = Time.time / lengthOfFade;
            alpha = Mathf.Lerp(1, 0, t);
            color.a = alpha;
            image.color = color;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}