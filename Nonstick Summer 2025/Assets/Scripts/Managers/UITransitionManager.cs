using UnityEngine;

public class UITransitionManager
{
    public static bool PlayerInMenu = false;

    private static GameObject currentCanvasReference;
    private static Transform oldCameraAnchorPoint;

    public UITransitionManager()
    {

    }

    public static void OpenMenu(GameObject canvasPrefab, Transform cameraAnchor = null)
    {
        if(PlayerInMenu)
        {
            Debug.LogWarning("Player is already in a menu. Force closing current menu.");
            CloseMenu();
        }

        PlayerInMenu = true;
        oldCameraAnchorPoint = GameManager.playerCameraRef.transform.parent;

        if(cameraAnchor != null)
            GameManager.playerCameraRef.transform.SetParent(cameraAnchor);

        StaticUtilities.EnableCursor();

        currentCanvasReference = GameObject.Instantiate(canvasPrefab);
    }

    public static void CloseMenu()
    {
        PlayerInMenu = false;

        GameManager.playerCameraRef.transform.SetParent(oldCameraAnchorPoint);
        oldCameraAnchorPoint = null;
        
        if(currentCanvasReference != null)
        {
            GameObject.Destroy(currentCanvasReference);
            currentCanvasReference = null;
        }

        StaticUtilities.DisableCursor();
    }
}
