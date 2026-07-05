using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.FilePathAttribute;

public class APLocationService : Service
{
    #region Singleton
    public static APLocationService Instance;
    protected override void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion

    private HashSet<ArchipelagoLocation> locations_checked = new();

    //private Queue<ArchipelagoLocation> locations_queue = new();

    protected async override Task ThisInitialize()
    {
        await Task.CompletedTask;
    }

    public async override Task DeInitialize()
    {
        await base.DeInitialize();
    }

    public bool IsLocationChecked(ArchipelagoLocation location)
    {
        // @TODO:
        return locations_checked.Contains(location);    
    }

    public void OnArchipelagoConnected()
    {
        CheckAllSavedLocations();
    }

    public async void CheckLocation(ArchipelagoLocation location)
    {
        //if (location == ArchipelagoLocation.None) return;

        locations_checked.Add(location);
        //locations_queue.Append(location);

        if (ArchipelagoManager.Instance.isConnected == false)
        {
            Debug.LogWarning("Archipelago not connected to client");
            return;
        }

        string locationName = ArchipelagoLocationNameMapping.GetLocationName(location);
        var locationId = ArchipelagoManager.Instance.session.Locations.GetLocationIdFromName(ArchipelagoManager.GAME_NAME, locationName);
        await ArchipelagoManager.Instance.session.Locations.CompleteLocationChecksAsync(new long[] { locationId } );

        ArchipelagoManager.Instance.OnLocationsUpdated.Invoke();

        Debug.Log($"<color=green>Location Checked: {locationName}</color>");
    }

    private async void CheckAllSavedLocations()
    {
        long[] location_ids = new long[locations_checked.Count];

        int i = 0;
        foreach (var location in locations_checked)
        {
            string locationName = ArchipelagoLocationNameMapping.GetLocationName(location);
            var locationId = ArchipelagoManager.Instance.session.Locations.GetLocationIdFromName(ArchipelagoManager.GAME_NAME, locationName);
            location_ids[i] = locationId;
            i++;
        }

        await ArchipelagoManager.Instance.session.Locations.CompleteLocationChecksAsync(location_ids);

        ArchipelagoManager.Instance.OnLocationsUpdated.Invoke();

        Debug.Log($"<color=green>Refreshed {location_ids.Count()} locations</color>");
    }
}
