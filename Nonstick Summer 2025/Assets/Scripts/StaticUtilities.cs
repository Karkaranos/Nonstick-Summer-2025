using UnityEngine;
using UnityEngine.UIElements;

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
            + referencePoint.right * inputDirection.x)
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
        UnityEngine.Cursor.visible = true;
        // Free mouse if editor, locked to window if in a build. For the sake of debugging because oh my god
        UnityEngine.Cursor.lockState = Application.isEditor ? CursorLockMode.None : CursorLockMode.Confined;
    }

    public static void DisableCursor()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Math
    public static T[] AddArrays<T>(T[] arr1, T[] arr2)
    {
        int index = 0;
        T[] result = new T[arr1.Length + arr2.Length];
        for(int i=0; i<arr1.Length-1; i++)
        {
            result[index] = arr1[i];
            index++;
        }
        for (int i = 0; i < arr2.Length-1; i++)
        {
            result[index] = arr2[i];
            index++;
        }
        return result;
    }
    #endregion

#if UNITY_EDITOR
    #region Debug

    /// <summary>
    /// (Editor only) Returns true if the user is selecting parent, or any of its children
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static bool Editor_SelectingSelfOrChild(Transform parent)
    {
        var selected = UnityEditor.Selection.activeTransform;
        return UnityEditor.Selection.activeTransform != null &&
            (selected == parent || selected.IsChildOf(parent));
    }
    #endregion
#endif
}
