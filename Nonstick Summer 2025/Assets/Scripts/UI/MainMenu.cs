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
    [Header("Main Menu")]
    [Scene] [SerializeField] private int MainGameplayScene=1;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject creditParent;
    [SerializeField] private GameObject creditScroll;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private CreditBehavior creditObj;

    [Header("Credit Controls")]
    [SerializeField] private float creditSpeed;
    /*[SerializeField]*/ private float heightToReach=7200;
    [SerializeField] private float pauseBeforeStartEnd = 1.5f;


    private GameObject openMenu;
    [SerializeField, ReadOnly] Vector3 creditStart;
    private Coroutine credits;
    
    [Header("Fade Transition Visuals")]
    [Tooltip ("Fade to black prefab in scene")]
    [SerializeField][Required] private GameObject fadeToBlack;
    [SerializeField][Required] private RectTransform creditsEndingFrame;
    

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

        if(FindFirstObjectByType<Check>().gameCompleted)
        {
            OpenCredits(false);
        }
    }

    public void StartGame()
    {
        FadeTransition fade = fadeToBlack.GetComponent<FadeTransition>();

        FindFirstObjectByType<Check>().gameCompleted = false;
        DoFadeOut(fade);
        //Cursor.visible = false; CALEB CALEB CALEB CALEB CALEB CALEB Toby!
    }

    public void OpenControls()
    {
        CloseMenu();
        openMenu = Instantiate(controls);
    }


    public void OpenCredits(bool buttonActive)
    {
        if(creditObj!=null)
        {
            mainMenu.SetActive(false);
            creditObj.OpenCredits(buttonActive);
        }
    }

    public void CloseCredits()
    {
        creditParent.SetActive(false);
        mainMenu.SetActive(true);
        creditObj.ResetCredits();
    }
    public void CloseMenu()
    {
        if(openMenu != null)
        {
            Destroy(openMenu);
        }
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
            fadeToBlack.SetActive(true);
            fade.StartFadeOut(image, MainGameplayScene);
        }
    }
}
