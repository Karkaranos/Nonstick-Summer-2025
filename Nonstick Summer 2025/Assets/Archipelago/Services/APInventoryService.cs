using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Helpers;
using static UnityEditor.Timeline.Actions.MenuPriority;
using NaughtyAttributes;
using UnityEditor;

public class APInventoryService : Service
{
    #region Singleton
    public static APInventoryService Instance;
    protected override void InitializeSingleton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }
    #endregion

    // Item : count of item
    private Dictionary <ArchipelagoItem, int> inventory = new();

    protected async override Task ThisInitialize()
    {
        ArchipelagoManager.Instance.OnArchipelagoConnected.AddListener(OnArchipelagoConnected);

        await Task.CompletedTask;
    }

    public async override Task DeInitialize()
    {
        await base.DeInitialize();
    }

    #region Connection

    void OnArchipelagoConnected()
    {
        inventory.Clear();

        ArchipelagoManager.Instance.session.Items.ItemReceived += OnItemsRecieved;

        RefreshAllItems();
        //OnItemsRecieved(ArchipelagoManager.Instance.session.Items);
    }

    private void RefreshAllItems()
    {
        Debug.Log($"<color=magenta>{ArchipelagoManager.Instance.session.Items.AllItemsReceived.Count} Archipelago items:");

        var items = ArchipelagoManager.Instance.session.Items;
        inventory.Clear();

        // Process any pre-collected items already waiting in the seed pool
        foreach (ItemInfo item in items.AllItemsReceived)
        {
            // this might break with games that arent mwg?

            AddItem(item);
        }

        while (items.Any())
        {
            var item = items.DequeueItem();

            AddItem(item);
        }

        Debug.Log(ArchipelagoManager.Instance.session.Items.AllItemsReceived.Count);

        APSaveDataService.Instance.UpdateItemCache(inventory);
        ArchipelagoManager.Instance.OnInventoryUpdated.Invoke();
    }

    public void OnItemsRecieved(IReceivedItemsHelper helper)
    {
        Debug.Log("<color=magenta>items recieved</color>");

        while (helper.Any())
        {
            AddItem(helper.DequeueItem());
        }

        APSaveDataService.Instance.UpdateItemCache(inventory);
        ArchipelagoManager.Instance.OnInventoryUpdated.Invoke();
    }

    private void AddItem(ItemInfo item)
    {
        var apItem = ArchipelagoItemNameMapping.GetItem(item.ItemName);

        inventory.TryAdd(apItem, 0);
        inventory[apItem]++;

        ArchipelagoManager.Instance.OnInventoryUpdated.Invoke();

        Debug.Log($"<color=magenta>Item:</color> {item.ItemName} : {apItem.ToString()} : x{inventory[apItem]}");
    }

    #endregion

    public bool IsItemCollected(ArchipelagoItem item)
    {
        return GetItemCount(item) >= 1;
    }

    public int GetItemCount(ArchipelagoItem item)
    {
        if (!inventory.ContainsKey(item))
        {
            inventory[item] = 0;
        }

        return inventory[item];
    }


    void Update()
    {
        if (!ArchipelagoManager.Instance.isConnected) return;

        // Process items on Unity's main thread loop
        while (ArchipelagoManager.Instance.session.Items.Any())
        {
            var item = ArchipelagoManager.Instance.session.Items.DequeueItem();
            AddItem(item);
        }
    }

    [Button]
    private void PrintItems()
    {
        foreach(var item_count in inventory)
        {
            if(item_count.Value != 0)
                Debug.Log($"{item_count.Key}: x{item_count.Value}");
        }
    }
}
