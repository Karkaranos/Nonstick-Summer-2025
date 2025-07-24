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

    [Header ("Fade Transition Visuals")]
    [SerializeField] [Required] private GameObject fadeToBlack;

    //maybe put cursor shenanigans here
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        DoFadeOut();
        //Cursor.visible = false; CALEB CALEB CALEB CALEB CALEB CALEB
    }


    public void Quit()
    {
        //this quits the game
        Application.Quit();
    }

    public void DoFadeOut()
    {
        var canvas = UITransitionManager.OpenMenu(fadeToBlack);
        var fade = canvas.GetComponent<FadeTransition>();
        var image = canvas.GetComponent<Image>();

        if (fade != null)
        {
            fade.StartFadeOut(image);
        }
    }
}
