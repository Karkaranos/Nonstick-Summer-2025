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
        return new Dictionary<ArchipelagoItem, int>();
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
            case 0: return new RelationshipStatus(-1);
            case 1: return saveData.Moment1Relationships;
        }

        RelationshipStatus result;

        switch (moment)
        {
            case 2: result = saveData.Moment2Relationships; break;
            case 3: result = saveData.Moment3Relationships; break;
            case 4: result = saveData.Moment4Relationships; break;
            case 5: result = saveData.Moment5Relationships; break;
            default: return null;
        }

        if (result.set)
            return result;

        // find previously set status
        return GetRelationshipStats(moment - 1);

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
        saveData.deckCache.modifiers = modifiers?.Select(m=>m.ThisModifier)?.ToList();

        SaveArchipelagoCache(); //todo: queue this function so it only happens once per frame
    }

    public void SetRelationshipStatus(RelationshipStatus relationshipStatus, int moment)
    {
        relationshipStatus.set = true;
        switch (moment)
        {
            case 1: 
                saveData.Moment1Relationships = relationshipStatus;
                saveData.Moment2Relationships.Reset();
                saveData.Moment3Relationships.Reset();
                saveData.Moment4Relationships.Reset();
                saveData.Moment5Relationships.Reset();
                return;
            case 2:
                saveData.Moment2Relationships = relationshipStatus;
                saveData.Moment3Relationships.Reset();
                saveData.Moment4Relationships.Reset();
                saveData.Moment5Relationships.Reset();
                return;
            case 3:
                saveData.Moment3Relationships = relationshipStatus;
                saveData.Moment4Relationships.Reset();
                saveData.Moment5Relationships.Reset(); 
                return;
            case 4:
                saveData.Moment4Relationships = relationshipStatus;
                saveData.Moment5Relationships.Reset();
                return;
            case 5:
                saveData.Moment5Relationships = relationshipStatus;
                return;
        }

        SaveArchipelagoCache();
    }

    /// <summary>
    /// Reset all cached data except for connection config
    /// </summary>
    public void ResetArchipelagoCache()
    {
        var oldConfig = saveData.ConnectionConfiguration;

        saveData = new();
        saveData.ConnectionConfiguration = oldConfig;

        saveData.Moment1Relationships = new RelationshipStatus(1);
        saveData.Moment2Relationships = new RelationshipStatus(2);
        saveData.Moment3Relationships = new RelationshipStatus(3);
        saveData.Moment4Relationships = new RelationshipStatus(4);
        saveData.Moment5Relationships = new RelationshipStatus(5);

        saveData.locationsCache = new();

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
