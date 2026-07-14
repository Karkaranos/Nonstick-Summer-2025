using UnityEngine;

public class HideIfLocationChecked : MonoBehaviour
{
    public ArchipelagoLocation location;

    public GameObject objectTohide;

    public bool OnlyCheckAtStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(objectTohide == null)
            objectTohide = gameObject;

        if(!OnlyCheckAtStart)
            ArchipelagoManager.Instance.OnLocationsUpdated.AddListener(Refresh);

        Refresh();
    }

    void Refresh()
    {
        bool hide = APLocationService.Instance.IsLocationChecked(location);
        objectTohide.SetActive(!hide);
    }
}
