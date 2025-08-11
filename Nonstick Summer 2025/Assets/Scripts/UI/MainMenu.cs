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
    [SerializeField][Required] private Image creditsFadeToBlack;
    

    //maybe put cursor shenanigans here
    private void Start()
    {
        creditStart = creditScroll.transform.localPosition;
        heightToReach = creditScroll.GetComponent<RectTransform>().rect.height;

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

    public void OpenControls()
    {
        CloseMenu();
        openMenu = Instantiate(controls);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditParent.SetActive(true);
        credits = StartCoroutine(ScrollCredits());
    }

    private void ResetCredits()
    {
        StopCoroutine(credits);
        creditScroll.transform.localPosition = creditStart;
    }

    private IEnumerator ScrollCredits()
    {
        creditsFadeToBlack.color = Color.clear;

        yield return new WaitForSeconds(pauseBeforeStartEnd);
        Vector3 pos = creditScroll.transform.position;
        while(pos.y < heightToReach)
        {
            yield return null;
            pos.y += creditSpeed * Time.deltaTime * Mathf.Clamp(Screen.height/1280, 1, 3);
            creditScroll.transform.position = pos;
        }
        yield return new WaitForSeconds(pauseBeforeStartEnd);

        float timeElapsed = 0;
        while(timeElapsed < 2)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / 2;
            creditsFadeToBlack.color = new Color(0, 0, 0, t);
            yield return null;
        }

        yield return new WaitForSeconds(pauseBeforeStartEnd * 2);

        CloseCredits();

    }

    public void CloseCredits()
    {
        creditParent.SetActive(false);
        mainMenu.SetActive(true);
        ResetCredits();
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
