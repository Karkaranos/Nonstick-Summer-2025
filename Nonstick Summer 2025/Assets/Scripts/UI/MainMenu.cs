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
using System.Collections;
public class MainMenu : MonoBehaviour
{
    [Scene] [SerializeField] private int MainGameplayScene=1;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject creditParent;
    [SerializeField] private GameObject creditScroll;
    [SerializeField] private GameObject mainMenu;

    [Header("Credit Controls")]
    [SerializeField] private float creditSpeed;
    [SerializeField] private float heightToReach;
    [SerializeField] private float pauseBeforeStartEnd = 1.5f;


    private GameObject openMenu;
    [SerializeField, ReadOnly] Vector3 creditStart;
    private Coroutine credits;

    //maybe put cursor shenanigans here
    private void Start()
    {
        creditStart = creditScroll.transform.localPosition;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainGameplayScene);
        //Cursor.visible = false; CALEB CALEB CALEB CALEB CALEB CALB
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
        yield return new WaitForSeconds(pauseBeforeStartEnd);
        Vector3 pos = creditScroll.transform.position;
        while(pos.y < heightToReach)
        {
            yield return null;
            pos.y += creditSpeed * Time.deltaTime * Mathf.Clamp(Screen.height/1280, 1, 3);
            creditScroll.transform.position = pos;
        }
        yield return new WaitForSeconds(pauseBeforeStartEnd*2);
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
}
