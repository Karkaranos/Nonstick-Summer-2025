/*****************************************************************************
* File Name :         MainMenu.cs
* Author :            Toby, Cade, Sky, Jay, Caleb
* Creation Date :     tbd
*
* Brief Description : 
*
* TODO:
* 
*****************************************************************************/

using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Scene] [SerializeField] private int MainGameplayScene = 1;
    [SerializeField] [Required] private Image fadeToBlack;
    [SerializeField] private float lengthOfFade = 3;

    private Coroutine fadeOutCoroutine;

    //maybe put cursor shenanigans here
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        //added fade in
        if (fadeOutCoroutine == null)
        {
            fadeToBlack.enabled = true;
            fadeOutCoroutine = StartCoroutine(FadeOut(fadeToBlack));
        }
        
        //Cursor.visible = false; CALEB CALEB CALEB CALEB CALEB CALEB
    }


    public void Quit()
    {
        //this quits the game
        Application.Quit();
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

        UnityEngine.SceneManagement.SceneManager.LoadScene(MainGameplayScene);
        yield return null;
    }
}
