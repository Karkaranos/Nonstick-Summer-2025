/*****************************************************************************
* File Name :         DialogueUIController.cs
* Author :            Toby
* Creation Date :     June 6, 2025
*
* Brief Description : "FrontEnd" script for dialogue controller. See DialogueManager
* for backend.
* This script is a singleton for easy access, although this script will not always be
* present in the scene.
*
* TODO:
* a lot
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;

public class DialougeUIController : Singleton<DialougeUIController>
{
    [SerializeField] private Slider energyBar;

    public void Initialize(DialogueBranch startBranch)
    {
        DialogueManager.OnOpenCombatUI(startBranch);

        // all the rest of this ui initialization stuff is gonna run every time an npc combat encounter happens.
        // i think our game is not complicated enough that its gonna be a problem performance wise, 
        // but its gonna bug me that its happening extra times

        energyBar.minValue = 0;
        energyBar.maxValue = DialogueManager.MaxEnergy;
        energyBar.value = DialogueManager.CurrentEnergy;
    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateEnergy(int? value)
    {
        energyBar.value = value ?? DialogueManager.CurrentEnergy;
        yield return null; // yield return null instead of yield break because fuck it we ball
        // questionable code is okay if you say "fuck it we ball" btw.
        // ok no but my actual reason for doing this is to make sure the future animation stuff wont break if not everything is happening at the same frame.
        // sorry for making this comment so long
    }


}
