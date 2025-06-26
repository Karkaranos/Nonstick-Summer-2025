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
        
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainGameplayScene);
    }


    public void Quit()
    {
        //this quits the game
        Application.Quit();
    }
}
