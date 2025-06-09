/*****************************************************************************
* File Name :         DialogueUIController.cs
* Author :            Toby, Sky
* Creation Date :     June 6, 2025
*
* Brief Description : "FrontEnd" script for dialogue controller. See DialogueManager
* for backend.
* This script should be listening to player input, handing it off to DialogueManager.
* DialogueManager then tells this script when to update ui stuff.
* Should ideally be bridging the gap between a lot of modular UI scripts, instead
* of handling any actual UI logic.
* This script is a singleton for easy access, although this script will not always be
* present in the scene.
*
* TODO:
* a lot
* 
* ...
* * Disable selecting cards if energy is <= 0 (may be in another script)
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections;

public class DialogueUIController : Singleton<DialogueUIController>
{
    [Required][SerializeField] private EnergyBar energyBar;
    [Tooltip("Relationship slider UI element")]
    [Required][SerializeField] private RelationshipSlider relationshipSlider;
    [SerializeField] private DialogueBox dialogueBox;

    public void Initialize(DialogueBranch startBranch, characters character)
    {
        DialogueManager.OnOpenCombatUI(startBranch, character);

        // all the rest of this ui initialization stuff is gonna run every time an npc combat encounter happens.
        // i think our game is not complicated enough that its gonna be a problem performance wise, 
        // but its gonna bug me that its happening extra times

        energyBar.Initalize(DialogueManager.MaxEnergy);
        relationshipSlider.Initialize(RelationshipManager.characterRelationships[character].maxValue, RelationshipManager.characterRelationships[character].currentValue);
        dialogueBox.Initialize(startBranch);

    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateEnergy(int? value)
    {
        yield return energyBar?.SetValue((float)(value ?? DialogueManager.CurrentEnergy));
    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateRelationship(float? value, characters character)
    {
        yield return relationshipSlider?.SetValue((value ?? RelationshipManager.characterRelationships[character].currentValue));
    }
}
