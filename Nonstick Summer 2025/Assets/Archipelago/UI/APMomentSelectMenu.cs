using NaughtyAttributes;
using UnityEngine;

public class APMomentSelectMenu : MonoBehaviour
{
    [SerializeField, Required] private CanvasGroup group;
    [SerializeField, Required] private GameObject disconnectedOverlay;
    [SerializeField] private APMomentButton[] momentButtons;

    private void Start()
    {
        disconnectedOverlay.SetActive(true);
        ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(() =>
        {
            disconnectedOverlay.SetActive(false);
        });

        Close();
    }

    public void Open()
    {
        disconnectedOverlay.SetActive(!ArchipelagoManager.Instance.isConnected);

        foreach (var button in momentButtons)
        {
            button.RefreshButtonDisplay();
        }

        StaticUtilities.EnableCanvasGroup(group);
    }

    public void Close()
    {
        StaticUtilities.DisableCanvasGroup(group);
    }
}
