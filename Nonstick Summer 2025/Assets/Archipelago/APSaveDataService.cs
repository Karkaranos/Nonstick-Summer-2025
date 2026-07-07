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

    #region Data Retrival

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

    public ArchipelagoConnectionConfiguration GetConnectionConfig()
    {
        return saveData.ConnectionConfiguration;
    }

    public RelationshipStatus GetRelationshipStats(int moment)
    {
        switch (moment)
        {
            case 1: return saveData.Moment1Relationships;
            case 2: return saveData.Moment2Relationships;
            case 3: return saveData.Moment3Relationships;
            case 4: return saveData.Moment4Relationships;
            case 5: return saveData.Moment5Relationships;
            default: return null;
        }
    }

    #endregion

    #region Saving

    public void SetConnectionConfiguation(string serverUrl, string slotName, string password)
    {
        saveData.ConnectionConfiguration.serverUrl = serverUrl;
        saveData.ConnectionConfiguration.slotName = slotName;
        saveData.ConnectionConfiguration.password = password;
    }

    public void UpdateLocationCache(HashSet<ArchipelagoLocation> locationCache)
    {
        saveData.locationsCache = locationCache.ToList();
        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    public void UpdateItemCache(Dictionary<ArchipelagoItem, int> itemCounts)
    {
        saveData.itemsCache = itemCounts.Select(d=> new SerializedKeyValuePair(d.Key,d.Value)).ToList();
        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    public void SetDeckInventory(Deck deck)
    {
        foreach(var card in deck.Cards)
        {
            saveData.deckCache.cards.Add(new SerializedCard(card));
        }
        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    public void SetModifierInventory(List<ModifierData> modifiers)
    {
        saveData.deckCache.modifiers = modifiers.Select(m=>m.ThisModifier).ToList();

        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    /// <summary>
    /// Reset all cached data except for connection config
    /// </summary>
    public void ResetArchipelagoCache()
    {
        var oldConfig = saveData.ConnectionConfiguration;

        saveData = new();
        saveData.ConnectionConfiguration = oldConfig;

        SaveArchipelagoCache();
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
