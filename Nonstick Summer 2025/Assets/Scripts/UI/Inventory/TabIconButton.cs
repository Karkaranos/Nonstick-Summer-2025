using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TabIconButton : Singleton<TabIconButton>   
{
    [Required] public RectTransform rectTransform;

    [Required, SerializeField] private CanvasGroup notification;

    private void Start()
    {
        ModifierInventory.OnInventoryOpened.AddListener(OnInventoryOpened);
        ToggleNotification(false);
    }

    public IEnumerator CollectedCardShakeAnimation()
    {
        yield return StaticUtilities.AnimateRotation(transform, new Vector3(0, 0,  15f),  0.15f);
        yield return StaticUtilities.AnimateRotation(transform, new Vector3(0, 0, -15f), 0.25f);
        yield return StaticUtilities.AnimateRotation(transform, Quaternion.identity,     0.15f);
        transform.rotation = Quaternion.identity;

        ToggleNotification(true);
    }

    void OnInventoryOpened()
    {
        transform.rotation = Quaternion.identity;
        ToggleNotification(false);
    }

    public void ToggleNotification(bool enabled)
    {
        notification.gameObject.SetActive(enabled);

        if (enabled)
        {
            notification.alpha = 0;
            StaticUtilities.FadeToVisible(notification, 0.25f, unscaledTime: true);
        }
    }
}
