using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class APSaveDataService : Service
{
    #region Singleton
    public static APSaveDataService Instance;
    protected override void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion

    private string path => Application.streamingAssetsPath + "/APCache.json";

    [SerializeField] APSaveData saveData;

    protected async override Task ThisInitialize()
    {
        LoadArchipelagoCache();

        await Task.CompletedTask;
    }

    [Button]
    public void LoadArchipelagoCache()
    {
        Debug.Log("Loading Archipelago Cache");
        if (File.Exists(path) == false)
        {
            Debug.LogWarning($"No Save File exists at {path}");
            return;
        }

        string file = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<APSaveData>(file);

        Debug.Log("<color=green>File Loaded Successfully</color>");
        Debug.Log(file);
    }

    public HashSet<ArchipelagoLocation> GetCachedLocations ()
    {
        return saveData.locationsCache.ToHashSet();
    }

    public Dictionary<ArchipelagoItem, int> GetCachedItems()
    {
        return saveData.itemsCache.ToDictionary(t => t.Key, t => t.Value);
    }

    #region Saving

    public void UpdateLocationCache(HashSet<ArchipelagoLocation> locationCache)
    {
        saveData.locationsCache = locationCache.ToList();
        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    public void UpdateItemCache(Dictionary<ArchipelagoItem, int> itemCounts)
    {
        saveData.itemsCache = itemCounts.ToList();
        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    [Button]
    private void SaveArchipelagoCache()
    {
        string s = JsonUtility.ToJson(saveData, prettyPrint: true);

        File.WriteAllText(path, s);

        // Copy paste
        GUIUtility.systemCopyBuffer = path;

        Debug.Log($"<color=green>File Saved Successfully at </color>{path}");
        Debug.Log(s);
    }

    #endregion

    public async override Task DeInitialize()
    {
        await base.DeInitialize();
    }
}
