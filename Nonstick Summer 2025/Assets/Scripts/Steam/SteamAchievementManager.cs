/* 
 * Contributors: Toby Schamberger, Brenden Burtz, Zach Abbott
 * 
 */

using UnityEngine;
using static Unity.Collections.AllocatorManager;
using NaughtyAttributes;
using System;

public class SteamAchievementManager : Singleton<SteamAchievementManager>
{
    private const uint STEAM_APP_ID = 1234567; //TODO: Replace with actual steam ID when we get it!!
    
    [SerializeField, ReadOnly] private bool connectedToSteam = false;

    protected override void Awake()
    {
        TryUnlockAllAchievements();
        base.Awake();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        try
        {
            Steamworks.SteamClient.Init(STEAM_APP_ID);
            connectedToSteam = true;
            Debug.Log("<color=green>Successfully Connected to Steam!</color>");
        }
        catch (System.Exception exception)
        {
            Debug.LogError("Could not actually connect to steam");
            Debug.Log(exception);
            connectedToSteam = false;
        }
        if (connectedToSteam)
        {
            //ClearAchievements();
        }
        UnlockAchievement(SteamAchievement.OpenGame);
    }

    // Update is called once per frame
    void Update()
    {
        if(connectedToSteam)
        {
            Steamworks.SteamClient.RunCallbacks();
        }
        else
        {
            try
            {
                Steamworks.SteamClient.Init(STEAM_APP_ID);
                connectedToSteam = true;
                Debug.Log("<color=green>Successfully Connected to Steam!</color>");
            }
            catch (System.Exception exception)
            {
                //Debug.LogError("Could not actually connect to steam");
                connectedToSteam = false;
            }
        }
    }

    public void dissconnectFromSteam()
    {
        if(connectedToSteam)
        {
            Steamworks.SteamClient.Shutdown();
        }
    }

    private string GetInternalAchievementName(SteamAchievement achievement)
    {
        return $"Achievement_{achievement.ToString()}";
    }

    public void UnlockAchievement(SteamAchievement achievement)
    {
        Debug.Log($"Steam Achievement unlocked: <color=blue>{achievement.ToString()}</color>");

        string id = GetInternalAchievementName(achievement);

        // Update value in case the player is not connected to the internet or something
        PlayerPrefs.SetInt(id, 1);

        if (connectedToSteam)
        {
            var achivement = new Steamworks.Data.Achievement(id);
            achivement.Trigger();
        }
    }

    public void UpdateProgress(SteamAchievement achievement, int currentProgression, int maxProgression)
    {
        string id = GetInternalAchievementName(achievement);

        // Update value in case the player is not connected to the internet or something
        if (currentProgression >= maxProgression)
        {
            PlayerPrefs.SetInt(id, 1);
        }

        bool completed = false;
        if (connectedToSteam)
        {
            completed = Steamworks.SteamUserStats.IndicateAchievementProgress(id, currentProgression, maxProgression);
        }

        if (completed)
            Debug.Log($"Steam Achievement completed: <color=green>{achievement.ToString()}</color>: {currentProgression}/{maxProgression}");
        else
            Debug.Log($"Steam Achievement progress: <color=blue>{achievement.ToString()}</color>: {currentProgression}/{maxProgression}");
    }


    /// <summary>
    /// Tries to unlock all achievements in case the player wasnt connected to the internet or something.
    /// </summary>
    private void TryUnlockAllAchievements()
    {
        int achievementsInMemory = 0;

        SteamAchievement[] achievements = (SteamAchievement[])Enum.GetValues(typeof(SteamAchievement));
        foreach (var a in achievements)
        {
            var achievement = new Steamworks.Data.Achievement(GetInternalAchievementName(a));

            bool completionStatus = PlayerPrefs.GetInt(GetInternalAchievementName(a)) == 1;

            if (completionStatus == true)
            {
                achievement.Trigger();
                achievementsInMemory++;
            }
        }

        Debug.Log($"<color=cyan>{achievementsInMemory}/{achievements.Length} Steam Achievements are saved");
    }

    //this is strictly for testing so we can reset our achievements in case we need to test something
    [Button]
    public void ClearAchievements()
    {
        SteamAchievement[] achievements = (SteamAchievement[])Enum.GetValues(typeof(SteamAchievement));
        foreach (var a in achievements)
        {
            var achievement = new Steamworks.Data.Achievement(GetInternalAchievementName(a));
            achievement.Clear();
        }
    }

    
}
