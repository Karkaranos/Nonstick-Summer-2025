using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    [Tooltip("How long the fade lasts")]
    [SerializeField] private float lengthOfFade = 3;
    [Tooltip("How long fade from black is delayed before starting the fade")]
    [SerializeField] private float delayLength = 2;

    [SerializeField] private GameObject credits;

    public void StartFadeOut(Image image, int nextScene)
    {
        if (fadeCoroutine == null)
        {
                GameObject player = FindFirstObjectByType<PlayerMovement>()?.gameObject;

                //no more bug
                if (player != null)
                {
                    Destroy(player.GetComponent<PlayerCamera>());
                    Destroy(player.GetComponent<PlayerMovement>());
                    Destroy(player.GetComponent<Interact>());
                }

            image.enabled = true;
            fadeCoroutine = StartCoroutine(FadeOut(image, nextScene));
        }
    }

    /// <summary>
    /// Fades out selected image
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(Image image, int nextScene)
    {
        float alpha = image.color.a;
        Color color = image.color;
        float timeElapsed = 0;

        while (alpha < 1)
        {
            float t = timeElapsed / lengthOfFade;
            timeElapsed += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, t);
            color.a = alpha;
            image.color = color;
            yield return new WaitForEndOfFrame();
        }

        Destroy(image);
        SceneManager.LoadScene(nextScene);
        if (nextScene == 0)
        {
            Instantiate(credits);
        }

        yield return null;
    }


    public void StartFadeIn(Image image)
    {
        if (fadeCoroutine == null)
        {
            image.enabled = true;
            fadeCoroutine = StartCoroutine(FadeIn(image));
        }
    }

    /// <summary>
    /// Fades out selected image
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(Image image)
    {
        float alpha = image.color.a;
        Color color = image.color;

        float timeStarted = Time.time;
        float timeElapsed = Time.time - timeStarted;
        
        yield return new WaitForSeconds(delayLength);
        while (alpha > 0)
        {
            float t = timeElapsed / lengthOfFade;
            timeElapsed += Time.deltaTime;

            alpha = Mathf.Lerp(1, 0, t);
            color.a = alpha;
            image.color = color;
            yield return new WaitForEndOfFrame();
        }

        Destroy(image);

        yield return null;
    }
}
