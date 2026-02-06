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
    private static Transform playerCamTransform;


    public static GameObject CurrentCanvasReference { get; private set; }
    public static GameObject WorldObjectReference { get; private set; }

    public UITransitionManager()
    {
        playerMesh = GameObject.FindFirstObjectByType<PlayerMovement>()?.GetComponentInChildren<MeshRenderer>();
    }

    //TODO rename this function
    public static bool OpenMenuIfNoOtherMenusAreOpenRightNow(GameObject canvasPrefab, out GameObject CanvasCreated, 
        Transform cameraAnchor = null, GameObject objectRef = null)
    {
        if (PlayerInMenu)
        {
            CanvasCreated = null;
            return false;
        }

        CanvasCreated = OpenMenu(canvasPrefab, cameraAnchor, objectRef);
        return true;
    }

    public static GameObject OpenMenu(GameObject canvasPrefab, Transform cameraAnchor = null, GameObject objectRef = null)
    {
        if(PlayerInMenu)
        {
            Debug.LogWarning("Player is already in a menu. Force closing current menu.");
            if(canvasPrefab.name.Contains("Setting") && CurrentCanvasReference.name.Contains("Pause"))
                CloseMenu(false, false);
            else
                CloseMenu(false);
        }

        PlayerInMenu = true;

        // hide player model
        if(playerMesh != null) playerMesh.enabled = false;

        if (playerCamTransform == null)
        {
            playerCamTransform = GetPlayerCam();
        }

        //two null checks...
        if (playerCamTransform == null)
        {
            return null;
        }

        // move camera
        oldCameraAnchorPoint = playerCamTransform.parent; // these still need to be set, even if there is no camera anchor
        oldCameraLocalPosition = playerCamTransform.localPosition;
        oldCameraLocalRotation = playerCamTransform.localRotation;

        if (cameraAnchor != null)
        {
            playerCamTransform.SetParent(cameraAnchor);
            playerCamTransform.localPosition = Vector3.zero;
            playerCamTransform.localRotation = Quaternion.identity;
        }


        StaticUtilities.EnableCursor();

        if(objectRef != null)
        {
            WorldObjectReference = objectRef;
        }
        else
        {
            Debug.Log("Removed");
        }

        CurrentCanvasReference = GameObject.Instantiate(canvasPrefab);
        return CurrentCanvasReference;
    }

    public static void CloseMenu(bool disableMouse = true, bool changeCam = true)
    {
        if (disableMouse)
        {
            StaticUtilities.DisableCursor();
        }

        PlayerInMenu = false;

        WorldObjectReference = null;

        // unhide player model
        if (playerMesh != null) playerMesh.enabled = true;

        // move camera back
        if(changeCam)
        {
            playerCamTransform.SetParent(oldCameraAnchorPoint);
            playerCamTransform.localPosition = oldCameraLocalPosition;
            playerCamTransform.localRotation = oldCameraLocalRotation;
            oldCameraAnchorPoint = null;
        }
        
        if(CurrentCanvasReference != null)
        {
            GameObject.Destroy(CurrentCanvasReference);
            CurrentCanvasReference = null;
        }

        if (GameManager.DeckManagerReference != null)
            GameManager.DeckManagerReference.RefreshDeck();
        else
            Debug.LogWarning("Deck manager is null");

    }

    public static void NullWorldReference()
    {
        WorldObjectReference = null;
    }

    private static Transform GetPlayerCam()
    {
        return GameManager.PlayerCameraRef == null ? GameObject.FindFirstObjectByType<PlayerCamera>()?.transform : GameManager.PlayerCameraRef?.transform;
    }
}
