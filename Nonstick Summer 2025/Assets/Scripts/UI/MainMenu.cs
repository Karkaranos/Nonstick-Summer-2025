using UnityEngine;
using NaughtyAttributes;

public class MainMenu : MonoBehaviour
{
    [Scene] private int MainGameplayScene;

    public void StartGame()
    {

    }


    public void Quit()
    {
        Application.Quit();
    }
}
