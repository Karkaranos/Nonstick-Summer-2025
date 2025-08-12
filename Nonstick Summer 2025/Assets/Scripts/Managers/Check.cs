using UnityEngine;

public class Check : MonoBehaviour
{
    public bool gameCompleted = false;
    private void Start()
    {
        if(FindObjectsByType<Check>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
}
