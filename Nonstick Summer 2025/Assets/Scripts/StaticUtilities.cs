using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
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

    #region VFX

    /// <summary>
    /// Instiates a particle system, and destroys it after its done playing.
    /// If a particle is set to loop, it will play forever
    /// </summary>
    public static void PlayAndDestroyParticle(GameObject particleSystemPrefab, Vector3 position, Vector3? scale=null, Quaternion? rotation=null)
    {
        if(particleSystemPrefab == null) return;
        scale = scale ?? Vector3.one;
        rotation = rotation ?? Quaternion.identity;

        // Destroy
        var ps = particleSystemPrefab.GetComponentInChildren<ParticleSystem>();
        if (ps == null)
        {
            Debug.LogWarning("Tried to spawn a particle, but no ParticleSystem was attached");
            return;
        }

        // Build
        var particleGameObject = GameObject.Instantiate(particleSystemPrefab, position, rotation.Value);
        if(!ps.main.playOnAwake)
            ps.Play();

        // Destroy
        if (!ps.main.loop)
        {
            float time = ps.main.startLifetime.constantMax;
            GameObject.Destroy(particleGameObject, ps.main.duration);
        }
           
    }

    #endregion

    #region UI

    public static void ToggleCanvasGroup(CanvasGroup canvasgroup, bool enabled, float? alpha = null, bool? ignoreParentGroups = null)
    {
        if (enabled)
            EnableCanvasGroup(canvasgroup, alpha:alpha, ignoreParentGroups: ignoreParentGroups);
        else
            DisableCanvasGroup(canvasgroup, ignoreParentGroups: ignoreParentGroups);
    }
    public static void EnableCanvasGroup(CanvasGroup canvasgroup, float? alpha = null, bool interactable = true, bool blocksRaycasts=true, bool? ignoreParentGroups = null)
    {
        canvasgroup.alpha = alpha ?? 1;
        canvasgroup.interactable = interactable;
        canvasgroup.blocksRaycasts = blocksRaycasts;
        canvasgroup.ignoreParentGroups = ignoreParentGroups ?? canvasgroup.ignoreParentGroups;
    }

    public static void DisableCanvasGroup(CanvasGroup canvasgroup, float? alpha = null, bool? ignoreParentGroups = null)
    {
        canvasgroup.alpha = alpha ?? 0;
        canvasgroup.interactable = false;
        canvasgroup.blocksRaycasts = false;
        canvasgroup.ignoreParentGroups = ignoreParentGroups ?? canvasgroup.ignoreParentGroups;
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

    public static Vector3 WorldPosition(this RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return corners.Average();
    }

    #endregion

    #region UI Fade

    public static Coroutine FadeToVisible(CanvasGroup group, float seconds, bool unscaledTime = true,
        UnityAction afterFadeCallback = null, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(FadeOpacityCoroutine(group, a: 1, seconds: seconds, unscaledTime: unscaledTime,
            afterFadeCallback: afterFadeCallback));
    }

    public static Coroutine FadeToHidden(CanvasGroup group, float seconds, bool unscaledTime = true,
        UnityAction afterFadeCallback = null, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(FadeOpacityCoroutine(group, a: 0, seconds: seconds, afterFadeCallback: afterFadeCallback, unscaledTime: unscaledTime));
    }

    public static Coroutine FadeOpacity(CanvasGroup group, float a, float seconds, bool unscaledTime = true,
        UnityAction afterFadeCallback = null, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(FadeOpacityCoroutine(group, a: a, seconds: seconds, afterFadeCallback: afterFadeCallback, unscaledTime: unscaledTime));
    }

    private static IEnumerator FadeOpacityCoroutine(CanvasGroup group, float a, float seconds, UnityAction afterFadeCallback = null, bool unscaledTime = true)
    {
        float startOpacity = group.alpha;

        float startTime = unscaledTime ? Time.unscaledTime : Time.time;
        float time = startTime;
        while (time - startTime < seconds)
        {
            time = unscaledTime ? Time.unscaledTime : Time.time;
            float t = (time - startTime) / seconds;

            group.alpha = Mathf.Lerp(startOpacity, a, t);

            yield return null;
        }
        // apply one more time just in case.
        group.alpha = a;

        if (afterFadeCallback != null)
            afterFadeCallback();
    }

    #endregion

    #region Animations

    /// <summary>
    /// Smooth transform's current scale to endScale;
    /// </summary>
    public static Coroutine AnimateUIPosition(RectTransform transform, Vector3 endPosition, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(AnimateUIPositionCoroutine(transform, transform.position, endPosition, seconds, unscaledTime, currentCoroutineToCancel));
    }

    private static IEnumerator AnimateUIPositionCoroutine(RectTransform transform, Vector3 startPosition, Vector3 endPosition, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        float startTime = unscaledTime ? Time.unscaledTime : Time.time;
        float time = startTime;
        while (time - startTime < seconds)
        {
            time = unscaledTime ? Time.unscaledTime : Time.time;
            float t = (time - startTime) / seconds;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            Debug.Log($"{startPosition} -> {transform.position} -> {endPosition}");

            yield return null;
        }
        // apply one more time just in case.
        transform.position = endPosition;
    }



    /// <summary>
    /// Smooth transform's current scale to endScale;
    /// </summary>
    public static Coroutine AnimateScale(Transform transform, Vector3 endScale, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(AnimateScaleCoroutine(transform, transform.localScale, endScale, seconds, unscaledTime, currentCoroutineToCancel));
    }

    /// <summary>
    /// Smooths the transforms scale from startScale to endScale;
    /// </summary>
    public static Coroutine AnimateScale(Transform transform, Vector3 startScale, Vector3 endScale, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(AnimateScaleCoroutine(transform, startScale, endScale, seconds, unscaledTime, currentCoroutineToCancel));
    }

    private static IEnumerator AnimateScaleCoroutine(Transform transform, Vector3 startScale, Vector3 endScale, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        float startTime = unscaledTime ? Time.unscaledTime : Time.time;
        float time = startTime;
        while (time - startTime < seconds)
        {
            time = unscaledTime ? Time.unscaledTime : Time.time;
            float t = (time - startTime) / seconds;

            transform.localScale = Vector3.Lerp(startScale, endScale, t);


            yield return null;
        }
        // apply one more time just in case.
        transform.localScale = endScale;
    }

    /// <summary>
    /// Smooth current rotation towards endEulerAngles;
    /// </summary>
    public static Coroutine AnimateRotation(Transform transform, Vector3 endEulerAngles, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(
            AnimateRotationCoroutine(transform, transform.rotation, Quaternion.Euler(endEulerAngles), seconds,
                                     unscaledTime, currentCoroutineToCancel)
        );
    }

    /// <summary>
    /// Smooth current rotation towards endRotation;
    /// </summary>
    public static Coroutine AnimateRotation(Transform transform, Quaternion endRotation, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        if (currentCoroutineToCancel != null)
            GameManager.Instance.StopCoroutine(currentCoroutineToCancel);

        return GameManager.Instance.StartCoroutine(
            AnimateRotationCoroutine(transform, transform.rotation, endRotation, seconds,
                                     unscaledTime, currentCoroutineToCancel)
        );
    }

    private static IEnumerator AnimateRotationCoroutine(Transform transform, Quaternion startRotation, Quaternion endRotation, float seconds,
        bool unscaledTime = true, Coroutine currentCoroutineToCancel = null)
    {
        float startTime = unscaledTime ? Time.unscaledTime : Time.time;
        float time = startTime;
        while (time - startTime < seconds)
        {
            time = unscaledTime ? Time.unscaledTime : Time.time;
            float t = (time - startTime) / seconds;

            transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);

            yield return null;
        }
        // apply one more time just in case.
        transform.localRotation = endRotation;
    }

    #endregion

    #region Math

    public static Vector3 Average(this Vector3[] vectors)
    {
        Vector3 total = Vector3.zero;
        foreach (var v in vectors)
        {
            total += v;
        }
        return total / vectors.Length;
    }

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

    #region Linq

    public static void ForEach<T>(this IEnumerable<T> source, UnityAction<T> action)
    {
        //source.ThrowIfNull("source");
        //action.ThrowIfNull("action");
        foreach (T element in source)
        {
            action(element);
        }
    }

    #endregion

    #region Color

    public static string ToHex(this Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
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
        return selected != null && (selected == parent || selected.IsChildOf(parent));
#else
        return false;
#endif
    }

    /// <summary>
    /// (Editor only) Returns true if the user is selecting parent, or any of its children
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static bool Editor_SelectingTransform(Transform transform)
    {
#if UNITY_EDITOR

        var selected = UnityEditor.Selection.activeTransform;
        return selected != null && selected == transform;
#else
        return false;
#endif
    }
    #endregion
}
