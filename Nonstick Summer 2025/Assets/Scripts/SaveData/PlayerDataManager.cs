using NaughtyAttributes;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
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
public class PlayerDataManager : MonoBehaviour 
{
    static string path = "Assets/Saves/PlayerData.json";

    /// <summary>
    /// Writes PlayerData variables to JSON file 
    /// </summary>
    [Button]
    public static void SaveGame()
    {
        PlayerData playerData = new PlayerData();

        //all variables are found here
        playerData.CurrentScene = SceneManager.GetActiveScene().buildIndex;
        playerData.CousinRelationshipValue = RelationshipManager.characterRelationships[characters.Cousin].currentValue;
        playerData.GrandmaRelationshipValue = RelationshipManager.characterRelationships[characters.Grandma].currentValue;
        playerData.MomRelationshipValue = RelationshipManager.characterRelationships[characters.Mom].currentValue;
        playerData.UncleRelationshipValue = RelationshipManager.characterRelationships[characters.Uncle].currentValue;

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
            SceneManager.LoadScene(loadedData.CurrentScene);
            

            RelationshipManager.characterRelationships[characters.Cousin].currentValue = loadedData.CousinRelationshipValue;
            RelationshipManager.characterRelationships[characters.Mom].currentValue = loadedData.MomRelationshipValue;
            RelationshipManager.characterRelationships[characters.Uncle].currentValue = loadedData.UncleRelationshipValue;
            RelationshipManager.characterRelationships[characters.Grandma].currentValue = loadedData.GrandmaRelationshipValue;
            Debug.Log(loadedData.CousinRelationshipValue + "cousin");
            Debug.Log(loadedData.MomRelationshipValue + "mom");
            Debug.Log(loadedData.UncleRelationshipValue + "uncle");
            Debug.Log(loadedData.GrandmaRelationshipValue + "grandma");
            Debug.Log("hallo");
        }
        else
        {
            Debug.LogWarning("No File Found.");
        }
    }
}
