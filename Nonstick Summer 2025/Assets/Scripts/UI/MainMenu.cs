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
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Scene][SerializeField] private int MainGameplayScene = 1;

    [Header("Fade Transition Visuals")]
    [SerializeField][Required] private GameObject fadeToBlack;

    //maybe put cursor shenanigans here
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        var toDelete = FindObjectsByType<CardPickupManager>(sortMode: FindObjectsSortMode.None);
        foreach(CardPickupManager deleteMe in toDelete)
        {
            Destroy(deleteMe.gameObject);
        }
    }

    public void StartGame()
    {
        FadeTransition fade = fadeToBlack.GetComponent<FadeTransition>();

        DoFadeOut(fade);
        //Cursor.visible = false; CALEB CALEB CALEB CALEB CALEB CALEB
    }


    public void Quit()
    {
        //this quits the game
        Application.Quit();
    }

    public void DoFadeOut(FadeTransition fade)
    {
        Image image = fadeToBlack.GetComponentInChildren<Image>();

        if (fade != null)
        {
            fade.StartFadeOut(image, MainGameplayScene);
        }
    }
}
