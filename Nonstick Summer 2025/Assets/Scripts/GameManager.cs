using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Transform playerTransformRef;
    public static Camera playerCameraRef;

    public static UITransitionManager UITransitionManagerReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        playerTransformRef = FindFirstObjectByType<PlayerMovement>().transform;
        playerCameraRef = FindFirstObjectByType<PlayerCamera>().playerCamera;

        UITransitionManagerReference = new UITransitionManager();
    }
}
