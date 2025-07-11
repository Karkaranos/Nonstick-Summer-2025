/*****************************************************************************
* File Name :         CardPickupManager.cs
* Author :            Toby
* Creation Date :     July 11, 2025
*
* Brief Description : Manages card pickups between scenes, and when saving/loading.
* 
* Singleton and DontDestroy on load.
* 
* This might be the one of the most demanding scripts ive ever made. As in, it
* has its demands. And it whips you into shape if you dont follow them.
* 
*****************************************************************************/

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CardPickupManager : Singleton<CardPickupManager>
{
    //        <Card Hash Code, Is Collected>
    public Dictionary<int, bool> PickupCollectedStatus = new Dictionary<int, bool>();

    private RectTransform? rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is only called ONCE with DontDestroyOnLoad
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Called by every single CardPickup on start
    /// </summary>
    public void InitializePickup(CardPickupInteractable pickup)
    {
        int hash = pickup.Hash;

        // Record pickup
        if (PickupCollectedStatus.ContainsKey(hash)) 
            Debug.LogWarning($"Duplicate Pickup recorded with the card \"{pickup.name}\" (hash: {hash})");
        else
            PickupCollectedStatus.Add(hash, false);
    }

    public void UpdatePickupCollected(CardPickupInteractable pickup)
    {
        int hash = pickup.Hash;

        if(PickupCollectedStatus.ContainsKey(hash))
        {
            PickupCollectedStatus[hash] = true;
        }
        else
        {
            Debug.LogWarning("Undocumented card pickup has been collected");
            PickupCollectedStatus.Add(hash, true);
        }
    }

    /// <summary>
    /// TODO: call this from save data manager script
    /// </summary>
    public void LoadSaveData(Dictionary<int,bool> loadedData)
    {
        var pickups = Resources.FindObjectsOfTypeAll<CardPickupInteractable>();

        foreach (var pickup in pickups)
        {
            int hash = pickup.Hash;
            if (loadedData.ContainsKey(hash))
            {
                PickupCollectedStatus[hash] = loadedData[hash];
            }
            else
            {
                Debug.LogWarning($"Loaded save file does not contain data for card pickup \"{pickup.name}\" (hash: {hash})");
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        rectTransform = rectTransform ?? GetComponent<RectTransform>();
        // dont fucking touch it
        rectTransform.position = Vector3.zero;
        rectTransform.rotation = Quaternion.identity;
    }
}
