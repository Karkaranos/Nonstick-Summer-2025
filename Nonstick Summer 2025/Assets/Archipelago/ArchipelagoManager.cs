using UnityEngine;
using System;
using System.Collections.Concurrent;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using NaughtyAttributes;
using UnityEngine.Events;

public class ArchipelagoManager : Singleton<ArchipelagoManager>
{
    [Header("Server Connection Data")]
    public string serverUrl = "multiworld.gg:59641";
    public const string GAME_NAME = "Midwest Goodbye";
    public string slotName = "Player1";
    public string password = "";

    private string previousServerUrl;

    public ArchipelagoSession session { get; private set; }
    public bool isConnected { get; private set; } = false;

    [SerializeField] private Service[] servicePrefabs;

    public UnityEvent OnArchipelagoConnected = new();

    // Whenever any new item or new items are added (or technically removed) from the inventory
    // Called in Archipelago Inventory Service
    public UnityEvent OnInventoryUpdated = new();

    // When a location is checked (or on initialize)
    public UnityEvent OnLocationsUpdated = new();

    protected override void Awake()
    {
        base.Awake();

        InitializeServices();

        DontDestroyOnLoad(this.gameObject);

        var configuration = APSaveDataService.Instance.GetConnectionConfig();
        serverUrl = configuration.serverUrl;
        slotName = configuration.slotName;
        password = configuration.password;

        previousServerUrl = serverUrl;
    }

    private async void InitializeServices()
    {
        foreach (var servicePrefab in servicePrefabs)
        {
            var service = Instantiate(servicePrefab);
            service.transform.parent = this.transform;
            await service.Initialize();
        }
    }


    [Button]
    public void ConnectToArchipelago()
    {
        try
        {
            // Initialize the session using the C# multi-client factory
            session = ArchipelagoSessionFactory.CreateSession(serverUrl);

            // Set up listener for incoming items sent by other worlds
            //session.Items.ItemReceived += OnItemReceived;

            // Attempt login connection
            LoginResult result = session.TryConnectAndLogin(
                GAME_NAME,
                slotName,
                ItemsHandlingFlags.AllItems,
                new Version(0, 5, 0), // Match current AP server protocol version
                password: password
            );

            if (result is LoginSuccessful success)
            {
                isConnected = true;
                Debug.Log("Successfully connected to Archipelago server!");

                // only update saved configuration if connection successful
                APSaveDataService.Instance.SetConnectionConfiguation(serverUrl, slotName, password);

                if(previousServerUrl != serverUrl)
                {
                    Debug.LogWarning("Server connection is different, resetting cached data");
                    APSaveDataService.Instance.ResetArchipelagoCache();
                }

                previousServerUrl = serverUrl;

                OnArchipelagoConnected.Invoke();
            }
            else if (result is LoginFailure failure)
            {
                Debug.LogError($"Archipelago connection failed: {string.Join(", ", failure.Errors)}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Network exception while connecting to Archipelago: {ex.Message}");
        }
    }
}
