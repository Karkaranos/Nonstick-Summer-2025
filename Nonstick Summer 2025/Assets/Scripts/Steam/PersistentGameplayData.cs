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

    //sorry if i did anything wrong toby
    [ReadOnly] public bool BestMomEndingUnlocked;
    [ReadOnly] public bool BestCousinEndingUnlocked;
    [ReadOnly] public bool BestGrandmaEndingUnlocked;
    [ReadOnly] public bool BestUncleEndingUnlocked;

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

        BestMomEndingUnlocked = false;
        BestCousinEndingUnlocked = false;
        BestGrandmaEndingUnlocked = false;
        BestUncleEndingUnlocked = false;
    }
}
