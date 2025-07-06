using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
/*************************************************
* Author Names :          Sky 
* References   :          Kozmobot Games' tutorial on saving/loading
* Date Created :          July 6, 2025
* Date Modified :         July 6, 2025
* Brief Description :     Enacts saving and loading within the game using PlayerData variables.
*   
*   TODO: when to save, why to save, who to save, how to save, where to save
*   
***************************************************/
public class PlayerDataManager
{
    static string path = "Assets/Saves/PlayerData.json";

    /// <summary>
    /// Writes PlayerData variables to JSON file 
    /// </summary>
    [Button]
    public static void SaveGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.currentScene = SceneManager.GetActiveScene().buildIndex;

        string json = JsonUtility.ToJson(playerData);
        
        System.IO.File.WriteAllText(path, json);
    }

    /// <summary>
    /// Loads saved variables from JSON, implements their effects
    /// </summary>
    [Button]
    public static void LoadGame()
    {
        if (File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);


            //this is where everything gets set
            SceneManager.LoadScene(loadedData.currentScene);
            Debug.Log("hallo");
        }
        else
        {
            Debug.LogWarning("No File Found.");
        }
    }
}
