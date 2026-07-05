using UnityEngine;

public class ShowIfItemCollected : MonoBehaviour
{
    public ArchipelagoItem item;

    public GameObject objectTohide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (objectTohide == null)
            objectTohide = gameObject;

        ArchipelagoManager.Instance.OnLocationsUpdated.AddListener(Refresh);

        Refresh();
    }

    void Refresh()
    {
        bool hide = APInventoryService.Instance.IsItemCollected(item);
        objectTohide.SetActive(!hide);
    }
}
