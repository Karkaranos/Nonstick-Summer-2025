using UnityEngine;

public class HideIfLocationChecked : MonoBehaviour
{
    public ArchipelagoLocation location;

    public GameObject objectTohide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(objectTohide == null)
            objectTohide = gameObject;

        ArchipelagoManager.Instance.OnLocationsUpdated.AddListener(Refresh);
        Refresh();
    }

    void Refresh()
    {
        Debug.Log($"Updating location check");
        bool hide = APLocationService.Instance.IsLocationChecked(location);
        objectTohide.SetActive(!hide);
    }
}
