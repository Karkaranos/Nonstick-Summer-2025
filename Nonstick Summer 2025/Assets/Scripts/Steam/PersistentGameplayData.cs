/*
 * Data that persists between scenes, but is reset when the user enters the main menu or quits the game.
 * 
 * This script is "reset" from the main menu script.
 * 
 * Contributors: Toby
 * Creation Date: 6/25/2026
 */

using NaughtyAttributes;
using UnityEngine;

public class PersistentGameplayData : Singleton<PersistentGameplayData>
{
    // Please reset all variables in the ResetPersistentValues() function
    [ReadOnly] public bool PlayerTalkedToUncle;

    void Start()
    {
        // Detach and roll out.
        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
        
        ResetPersistentValues();
    }

    public void ResetPersistentValues()
    {
        PlayerTalkedToUncle = false ;
    }
}
