using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    private float lengthOfFade;

    public void StartFadeOut(Image image)
    {
        if (fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(FadeOut(image));
        }
    }

    /// <summary>
    /// Fades out selected image
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(Image image)
    {
        float alpha = image.color.a;
        Color color = image.color;

        while (image.color.a < 1)
        {
            float t = Time.time / lengthOfFade;
            alpha = Mathf.Lerp(0, 1, t);
            color.a = alpha;
            image.color = color;
            yield return new WaitForEndOfFrame();
        }

        Destroy(image);

        yield return null;
    }
}
