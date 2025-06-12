using UnityEngine;
using NaughtyAttributes;

public class MainMenu : MonoBehaviour
{
    [Scene] private int MainGameplayScene;

    public void StartGame(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }


    public void Quit()
    {
        Application.Quit();
    }
}
