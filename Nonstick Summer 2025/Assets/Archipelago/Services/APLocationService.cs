using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.FilePathAttribute;
using NaughtyAttributes;

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
        locations_checked = APSaveDataService.Instance.GetCachedLocations();

        ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(OnArchipelagoConnected);

        await Task.CompletedTask;
    }

    public bool IsLocationChecked(ArchipelagoLocation location)
    {
        if(location == ArchipelagoLocation.None)
        {
            Debug.LogWarning("Searching for none location");
            return true;
        }

        // @TODO:
        return locations_checked.Contains(location);    
    }

    public bool IsAnyLocationChecked(ArchipelagoLocation[] locations)
    {
        foreach (ArchipelagoLocation location in locations)
        {
            bool met = IsLocationChecked(location);
            if (met)
                return true;
        }
        return false;
    }

    public bool AreLocationsChecked(ArchipelagoLocation[] locations)
    {
        foreach (ArchipelagoLocation location in locations)
        {
            bool met = IsLocationChecked(location);
            if (!met)
                return false;
        }
        return true;
    }

    public void OnArchipelagoConnected()
    {
        CheckAllSavedLocations();
    }

    private async void CheckAllSavedLocations()
    {
        // TODO: get rid of this im just being precautious rn

        locations_checked = APSaveDataService.Instance.GetCachedLocations();

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

    public async void CheckLocation(ArchipelagoLocation location, bool updatePercolatingLocations=true)
    {
        if (location == ArchipelagoLocation.None)
        {
            Debug.LogError("Attempting to check \"none\" location");
            return;
        }

        locations_checked.Add(location);
        APSaveDataService.Instance.UpdateLocationCache(locations_checked);

        if (ArchipelagoManager.Instance.isConnected == false)
        {
            Debug.LogWarning("Archipelago not connected to client");
            return;
        }

        string locationName = ArchipelagoLocationNameMapping.GetLocationName(location);
        var locationId = ArchipelagoManager.Instance.session.Locations.GetLocationIdFromName(ArchipelagoManager.GAME_NAME, locationName);
        await ArchipelagoManager.Instance.session.Locations.CompleteLocationChecksAsync(new long[] { locationId });

        ArchipelagoManager.Instance.OnLocationsUpdated.Invoke();

        Debug.Log($"<color=green>Location Checked: {locationName}</color>");

        if(updatePercolatingLocations)
            UpdatePercolatingLocations();
    }

    [Button]
    private void UpdatePercolatingLocations()
    {
        Debug.Log(IsLocationChecked(ArchipelagoLocation.Moment5_Mom_Route_1));
        Debug.Log(IsLocationChecked(ArchipelagoLocation.Moment5_Grandma_Route_1));
            Debug.Log(IsLocationChecked(ArchipelagoLocation.Moment5_Cousin_Route_1));
        Debug.Log(IsLocationChecked(ArchipelagoLocation.Moment5_Uncle_Route_1));
        Debug.Log(IsLocationChecked(ArchipelagoLocation.Moment5_Complete));

        if (    IsLocationChecked(ArchipelagoLocation.Moment5_Mom_Route_1) 
            && IsLocationChecked(ArchipelagoLocation.Moment5_Grandma_Route_1) 
            && IsLocationChecked(ArchipelagoLocation.Moment5_Cousin_Route_1) 
            && IsLocationChecked(ArchipelagoLocation.Moment5_Uncle_Route_1)
            && IsLocationChecked(ArchipelagoLocation.Moment5_Complete))
        {
            Debug.Log("PLAYER WINS ARCHIPELAGO FOREVER!");
            CheckLocation(ArchipelagoLocation.Victory_Location, false);
        } 

        // actually i dont care
        if (IsLocationChecked(ArchipelagoLocation.Moment5_Mom_Route_1))
        {
            CheckLocation(ArchipelagoLocation.Moment5_Mom_Route_2, false);
            CheckLocation(ArchipelagoLocation.Moment5_Mom_Route_3, false);
        }
        if (IsLocationChecked(ArchipelagoLocation.Moment5_Mom_Route_2))
        {
            CheckLocation(ArchipelagoLocation.Moment5_Mom_Route_3, false);
        }

        if (IsLocationChecked(ArchipelagoLocation.Moment5_Grandma_Route_1))
        {
            CheckLocation(ArchipelagoLocation.Moment5_Grandma_Route_2, false);
            CheckLocation(ArchipelagoLocation.Moment5_Grandma_Route_3, false);
        }
        if (IsLocationChecked(ArchipelagoLocation.Moment5_Grandma_Route_2))
        {
            CheckLocation(ArchipelagoLocation.Moment5_Grandma_Route_3, false);
        }

    }
}
