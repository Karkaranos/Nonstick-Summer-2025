using NaughtyAttributes;
using UnityEngine;

public class ShowIfItemCollected : MonoBehaviour
{
    public ArchipelagoItem item;

    public GameObject[] ObjectsToHide;

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
        foreach(var obj in ObjectsToHide)
        {
            obj.SetActive(show);
        }
    }
}
