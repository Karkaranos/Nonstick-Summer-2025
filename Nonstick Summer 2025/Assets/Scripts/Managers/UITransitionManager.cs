using UnityEngine;

public class UITransitionManager
{
    public static UITransitionManager Instance => GameManager.UITransitionManagerReference;

    public static bool PlayerInMenu = false;

    private static GameObject currentCanvasReference;
    private static Transform oldCameraAnchorPoint;
    private static Vector3 oldCameraLocalPosition;
    private static Quaternion oldCameraLocalRotation;

    private static MeshRenderer playerMesh;
    private static Transform playerCamTransform => GameManager.playerCameraRef.transform;


    public static GameObject CurrentCanvasReference { get; private set; }
    public static GameObject WorldObjectReference { get; private set; }

    public UITransitionManager()
    {
        playerMesh = GameObject.FindFirstObjectByType<PlayerMovement>()?.GetComponentInChildren<MeshRenderer>();
    }

    public static GameObject OpenMenu(GameObject canvasPrefab, Transform cameraAnchor = null, GameObject objectRef = null)
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

        if(objectRef != null)
        {
            WorldObjectReference = objectRef;
        }

        CurrentCanvasReference = GameObject.Instantiate(canvasPrefab);
        return CurrentCanvasReference;
    }

    public static void CloseMenu()
    {
        PlayerInMenu = false;

        WorldObjectReference = null;

        // unhide player model
        if (playerMesh != null) playerMesh.enabled = true;

        // move camera back
        GameManager.playerCameraRef.transform.SetParent(oldCameraAnchorPoint);
        playerCamTransform.localPosition = oldCameraLocalPosition;
        playerCamTransform.localRotation = oldCameraLocalRotation;
        oldCameraAnchorPoint = null;
        
        if(CurrentCanvasReference != null)
        {
            GameObject.Destroy(CurrentCanvasReference);
            CurrentCanvasReference = null;
        }

        StaticUtilities.DisableCursor();
    }
}
