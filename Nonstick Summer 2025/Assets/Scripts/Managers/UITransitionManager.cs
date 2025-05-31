using UnityEngine;

public class UITransitionManager
{
    public static bool PlayerInMenu = false;

    private static GameObject currentCanvasReference;
    private static Transform oldCameraAnchorPoint;
    private static Vector3 oldCameraLocalPosition;
    private static Quaternion oldCameraLocalRotation;

    private static MeshRenderer playerMesh;
    private static Transform playerCamTransform => GameManager.playerCameraRef.transform;

    public UITransitionManager()
    {
        playerMesh = GameObject.FindFirstObjectByType<PlayerMovement>().GetComponentInChildren<MeshRenderer>();
    }

    public static void OpenMenu(GameObject canvasPrefab, Transform cameraAnchor = null)
    {
        if(PlayerInMenu)
        {
            Debug.LogWarning("Player is already in a menu. Force closing current menu.");
            CloseMenu();
        }

        PlayerInMenu = true;

        // hide player model
        if(playerMesh != null) playerMesh.enabled = false;

        // move camera
        oldCameraAnchorPoint = playerCamTransform.parent; // these still need to be set, even if there is no camera anchor
        oldCameraLocalPosition = playerCamTransform.localPosition;
        oldCameraLocalRotation = playerCamTransform.localRotation;
        if (cameraAnchor != null)
        {
            GameManager.playerCameraRef.transform.SetParent(cameraAnchor);
            playerCamTransform.localPosition = Vector3.zero;
            playerCamTransform.localRotation = Quaternion.identity;
        }


        StaticUtilities.EnableCursor();

        currentCanvasReference = GameObject.Instantiate(canvasPrefab);
    }

    public static void CloseMenu()
    {
        PlayerInMenu = false;

        // unhide player model
        if (playerMesh != null) playerMesh.enabled = true;

        // move camera back
        GameManager.playerCameraRef.transform.SetParent(oldCameraAnchorPoint);
        playerCamTransform.localPosition = oldCameraLocalPosition;
        playerCamTransform.localRotation = oldCameraLocalRotation;
        oldCameraAnchorPoint = null;
        
        if(currentCanvasReference != null)
        {
            GameObject.Destroy(currentCanvasReference);
            currentCanvasReference = null;
        }

        StaticUtilities.DisableCursor();
    }
}
