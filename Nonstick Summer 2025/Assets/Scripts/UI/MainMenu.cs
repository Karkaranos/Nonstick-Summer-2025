using UnityEngine;
using NaughtyAttributes;

public class MainMenu : MonoBehaviour
{
    [Scene] private int MainGameplayScene;

    public void Quit()
    {
        Application.Quit();
    }
}
