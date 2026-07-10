using NaughtyAttributes;
using UnityEngine;

public class ShowIfItemCollected : MonoBehaviour
{
    public ArchipelagoItem item;

    public GameObject[] ObjectsToHide;

    [Foldout("Advanced")]
    public ArchipelagoLocation HideIfLocationChecked = ArchipelagoLocation.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (objectTohide == null)
        //    objectTohide = gameObject;

        ArchipelagoManager.Instance.OnInventoryUpdated.AddListener(Refresh);

        Refresh();
    }

    [Button]
    void Refresh()
    {
        bool show = APInventoryService.Instance.IsItemCollected(item);

        if(HideIfLocationChecked != ArchipelagoLocation.None)
        {
            bool locationChecked = APLocationService.Instance.IsLocationChecked(HideIfLocationChecked);
            if (locationChecked)
                show = false;
        }

        foreach(var obj in ObjectsToHide)
        {
            obj.SetActive(show);
        }
    }
}
