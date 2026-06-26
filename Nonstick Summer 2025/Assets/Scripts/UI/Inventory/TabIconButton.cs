using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TabIconButton : Singleton<TabIconButton>   
{
    [Required] public RectTransform rectTransform;

    [Required, SerializeField] private CanvasGroup notification;

    [Tooltip("Time for the animation to do one swing")]
    [SerializeField] private float oneShakeSeconds = 0.15f;

    bool animating;
    Quaternion defaultRotation;

    private void Start()
    {
        defaultRotation = transform.rotation;
        ModifierInventory.OnInventoryOpened.AddListener(OnInventoryOpened);
        ToggleNotification(false);
    }

    public IEnumerator CollectedCardShakeAnimation()
    {
        animating = true;

        yield return StaticUtilities.AnimateRotation(transform, new Vector3(0, 0,  15f), oneShakeSeconds);
        yield return StaticUtilities.AnimateRotation(transform, new Vector3(0, 0, -15f), oneShakeSeconds + 0.1f);
        yield return StaticUtilities.AnimateRotation(transform, Quaternion.identity,     oneShakeSeconds);
        transform.rotation = Quaternion.identity;
        animating = false;

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

    // running into a problem where the icon will be rotated for like, no reason?
    // sp f it, were just using update.
    void Update()
    {
        if(!animating)
        {
            float speed = 1 / oneShakeSeconds;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, speed * Time.deltaTime);
        }
    }
}
