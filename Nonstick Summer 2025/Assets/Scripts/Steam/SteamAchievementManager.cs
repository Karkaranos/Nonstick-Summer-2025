/* 
 * Contributors: Toby Schamberger, Brenden Burtz, Zach Abbott
 * 
 */

using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class SteamAchievementManager : Singleton<SteamAchievementManager>
{
    private const uint STEAM_APP_ID = 1234567; //TODO: Replace with actual steam ID when we get it!!
    
    [SerializeField, ReadOnly] private bool connectedToSteam = false;

    #region Init
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshAllAchievements();
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
        if (connectedToSteam)
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
        if (connectedToSteam)
        {
            Steamworks.SteamClient.Shutdown();
        }
    }

    void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    #endregion

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
    private void RefreshAllAchievements()
    {
        int achievementsInMemory = 0;

        SteamAchievement[] achievements = (SteamAchievement[])Enum.GetValues(typeof(SteamAchievement));
        foreach (var a in achievements)
        {
            var achievement = new Steamworks.Data.Achievement(GetInternalAchievementName(a));

            bool completionStatus = PlayerPrefs.GetInt(GetInternalAchievementName(a)) == 1;

            if (completionStatus == true)
            {
                if(connectedToSteam)
                    achievement.Trigger();
                achievementsInMemory++;
            }
        }

        Debug.Log($"<color=cyan>{achievementsInMemory}/{achievements.Length} Steam Achievements are already completed. Use the \"Clear Achievements\" button on Steam Achievement Manager gameobject if you would like to reset.");
    }

    #region Debug Buttons

    //this is strictly for testing so we can reset our achievements in case we need to test something
    [Button]
    public void ClearAchievements()
    {
        SteamAchievement[] achievements = (SteamAchievement[])Enum.GetValues(typeof(SteamAchievement));
        foreach (var a in achievements)
        {
            string id = GetInternalAchievementName(a);
            PlayerPrefs.SetInt(id, 0);

            if (connectedToSteam)
            {
                var achievement = new Steamworks.Data.Achievement(id);
                achievement.Clear();
            }
        }
    }

    [Button]
    public void PrintAchievementCompletionStatus()
    {
        int numCompleted = 0;

        SteamAchievement[] achievements = (SteamAchievement[])Enum.GetValues(typeof(SteamAchievement));
        foreach (var a in achievements)
        {
            bool completionStatus = PlayerPrefs.GetInt(GetInternalAchievementName(a)) == 1;

            string completionText = completionStatus ?
                $"<color=green>Complete</color>" :
                $"<color=red>Incomplete</color>";

            Debug.Log($"{a.ToString()}: {completionText}");

            if(completionStatus == true)
                numCompleted ++;
        }
        Debug.Log($"<color=cyan>{numCompleted}/{achievements.Length} Steam Achievements are already completed. Use the \"Clear Achievements\" button on Steam Achievement Manager gameobject if you would like to reset.");
    }

    #endregion
}
