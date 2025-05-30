using UnityEngine;

public static class StaticUtilities
{
    #region Gameplay

    /// <summary>
    /// Most commonly used to transform player input (WASD) to 3D input, relative to the camera
    /// </summary>
    /// <param name="inputDirection">2D player input (WASD)</param>
    /// <param name="referencePoint">Usually the camera</param>
    /// <returns>Transformed Input Direction</returns>
    public static Vector3 TransformInputDirection(Vector2 inputDirection, Transform referencePoint)
    {
        return 
            ( referencePoint.forward * inputDirection.y 
            + referencePoint.up * inputDirection.x)
            .normalized;
    }

    #endregion

    #region UI
    public static void EnableCanvasGroup(CanvasGroup canvasgroup)
    {
        canvasgroup.alpha = 1;
        canvasgroup.interactable = true;
        canvasgroup.blocksRaycasts = true;
    }

    public static void DisableCanvasGroup(CanvasGroup canvasgroup)
    {
        canvasgroup.alpha = 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
    }

    public static void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public static void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
