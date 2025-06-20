using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
    public static void EnableCanvasGroup(CanvasGroup canvasgroup, float alpha = 1, bool interactable = true, bool blocksRaycasts=true)
    {
        canvasgroup.alpha = alpha;
        canvasgroup.interactable = interactable;
        canvasgroup.blocksRaycasts = blocksRaycasts;
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

    /// <summary>
    /// Sets the colors of a selectable ui component.
    /// All color parameters are optional, so only set the ones you need to update.
    /// </summary>
    /// <param name="uiComponent"></param>
    public static void SetColors(this Selectable uiComponent,
        Color? normalColor = null, Color? highlightedColor=null, Color? pressedColor = null, Color? selectedColor=null, Color? disabledColor=null )
    {
        var colors = uiComponent.colors;
        colors.normalColor = normalColor ?? colors.normalColor;
        colors.highlightedColor = highlightedColor ?? colors.highlightedColor;
        colors.selectedColor = selectedColor ?? colors.selectedColor;
        colors.pressedColor = pressedColor ?? colors.pressedColor;
        colors.disabledColor = disabledColor ?? colors.disabledColor;
        uiComponent.colors = colors;
    }

    #endregion

    #region Math
    
    #endregion

    #region Lists

    /// <summary>
    /// Shuffles selected list
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    { //ty stack exchange <3
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    /// <summary>
    /// Combines two arrays of any type
    /// </summary>
    /// <typeparam name="T">Variable type for arrays</typeparam>
    /// <param name="arr1">The first array</param>
    /// <param name="arr2">The second array</param>
    /// <returns>The combined array, with elements from array 1 first</returns>
    public static T[] AddArrays<T>(T[] arr1, T[] arr2)
    {
        int index = 0;
        T[] result = new T[arr1.Length + arr2.Length];
        for (int i = 0; i < arr1.Length - 1; i++)
        {
            result[index] = arr1[i];
            index++;
        }
        for (int i = 0; i < arr2.Length - 1; i++)
        {
            result[index] = arr2[i];
            index++;
        }
        return result;
    }

    /// <summary>
    /// Converts a list of any type into an array
    /// </summary>
    /// <typeparam name="T">The data type</typeparam>
    /// <param name="list">The list to be converted</param>
    /// <returns>The list in array form</returns>
    public static T[] ListToArray<T>(List<T> list)
    {
        T[] result = new T[list.Count];
        for(int i=0; i<list.Count; i++)
        {
            result[i] = list.ElementAt(i);
        }
        return result;
    }

    #endregion

    #region Debug

    /// <summary>
    /// (Editor only) Returns true if the user is selecting parent, or any of its children
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static bool Editor_SelectingSelfOrChild(Transform parent)
    {
#if UNITY_EDITOR

        var selected = UnityEditor.Selection.activeTransform;
        return UnityEditor.Selection.activeTransform != null &&
            (selected == parent || selected.IsChildOf(parent));
#else
        return false;
#endif
    }
    #endregion
}
