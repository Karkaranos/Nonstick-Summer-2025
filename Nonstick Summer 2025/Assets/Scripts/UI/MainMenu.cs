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
