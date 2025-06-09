/*****************************************************************************
* File Name :         DialogueUIController.cs
* Author :            Toby
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
    [Required][SerializeField] private DisplayPlayerCardDialogue playerDialogueBubble;
    [Required][SerializeField] private DeckDisplayer deckDisplay;

    public void Initialize(DialogueBranch startBranch)
    {
        DialogueManager.OnOpenCombatUI(startBranch);

        // all the rest of this ui initialization stuff is gonna run every time an npc combat encounter happens.
        // i think our game is not complicated enough that its gonna be a problem performance wise, 
        // but its gonna bug me that its happening extra times

        energyBar.Initalize(DialogueManager.MaxEnergy);
        deckDisplay.SetDeck(ref DialogueManager.PlayerHand);
    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateEnergy(int? value)
    {
        yield return energyBar?.SetValue((float)(value ?? DialogueManager.CurrentEnergy));
    }

    public void UpdateHoveringCard(CardData card)
    {
        // card is null, it hides the text bubble
        playerDialogueBubble.WriteText(card);
    }
}
