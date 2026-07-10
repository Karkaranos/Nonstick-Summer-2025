using UnityEngine;

public class ShowIfLocationChecked : MonoBehaviour
{
    public ArchipelagoLocation location;

    public GameObject objectToShow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(objectToShow == null)
            objectToShow = gameObject;

        ArchipelagoManager.Instance.OnLocationsUpdated.AddListener(Refresh);

        Refresh();
    }

    void Refresh()
    {
        Debug.Log($"Updating location check");
        bool check = APLocationService.Instance.IsLocationChecked(location);
        objectToShow.SetActive(check);
    }
}
