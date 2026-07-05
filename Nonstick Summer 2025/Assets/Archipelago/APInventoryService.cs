using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Helpers;
using static UnityEditor.Timeline.Actions.MenuPriority;

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

        // Process any pre-collected items already waiting in the seed pool
        foreach (ItemInfo item in items.AllItemsReceived)
        {
            // this might break with games that arent mwg?

            ArchipelagoItem ap_item = ArchipelagoItemNameMapping.GetItem(item.ItemName);

            if (inventory.ContainsKey(ap_item))
                inventory[ap_item] += 1;
            else
                inventory.Add(ap_item, 1);

            Debug.Log($"{item.ItemName} : {ap_item.ToString()} : x{inventory[ap_item]}");
        }

        /*while (items.Any())
        {
            var item = items.DequeueItem();

            // this might break with games that arent mwg?

            ArchipelagoItem ap_item = ArchipelagoItemNameMapping.GetItem(item.ItemName);

            if (inventory.ContainsKey(ap_item))
                inventory[ap_item] += 1;
            else
                inventory.Add(ap_item, 1);

            Debug.Log($"{item.ItemName} : {ap_item.ToString()} : x{inventory[ap_item]}");
        }*/

        ArchipelagoManager.Instance.OnInventoryUpdated.Invoke();
    }

    public void OnItemsRecieved(IReceivedItemsHelper items)
    {
        Debug.Log("<color=magenta>items recieved </color>");
        while (items.Any())
        {
            var item = items.DequeueItem();

            // this might break with games that arent mwg?

            ArchipelagoItem ap_item = ArchipelagoItemNameMapping.GetItem(item.ItemName);

            if (inventory.ContainsKey(ap_item))
                inventory[ap_item] += 1;
            else
                inventory.Add(ap_item, 1);

            Debug.Log($"{item.ItemName} : {ap_item.ToString()} : x{inventory[ap_item]}");
        }

        ArchipelagoManager.Instance.OnInventoryUpdated.Invoke();
    }

}
