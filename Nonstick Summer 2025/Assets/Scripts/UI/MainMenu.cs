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

public class MainMenu : MonoBehaviour
{
    [Scene] [SerializeField] private int MainGameplayScene=1;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainGameplayScene);
        //Cursor.visible = false; CALEB CALEB CALEB CALEB CALEB CALB
    }


    public void Quit()
    {
        //this quits the game
        Application.Quit();
    }
}
