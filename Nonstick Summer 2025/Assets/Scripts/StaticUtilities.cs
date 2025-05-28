using UnityEngine;

public static class StaticUtilities
{
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
}
