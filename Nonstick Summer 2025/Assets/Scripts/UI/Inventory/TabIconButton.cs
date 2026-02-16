using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class TabIconButton : Singleton<TabIconButton>   
{
    [Required] public RectTransform rectTransform;

    [Required, SerializeField] private RectTransform notification;

    private void Start()
    {
        ModifierInventory.OnInventoryOpened.AddListener(OnInventoryOpened);
        ToggleNotification(false);
    }

    public IEnumerator CollectedCardShakeAnimation()
    {
        yield return StaticUtilities.AnimateRotation(transform, new Vector3(0, 0, 20f),  0.15f);
        yield return StaticUtilities.AnimateRotation(transform, new Vector3(0, 0, -20f), 0.25f);
        yield return StaticUtilities.AnimateRotation(transform, Quaternion.identity,     0.15f);

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
    }
}
