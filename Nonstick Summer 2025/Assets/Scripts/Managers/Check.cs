using UnityEngine;

public class Check : MonoBehaviour
{
    public bool gameCompleted = false;
    private void Start()
    {
        var all = FindObjectsByType<Check>(FindObjectsSortMode.None);
        if (all.Length > 1)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            {
                foreach (Check c in all)
                {
                    if (c.gameCompleted)
                    {
                        FindFirstObjectByType<MainMenu>()?.OpenCredits(false);
                    }
            }
            }
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
}
