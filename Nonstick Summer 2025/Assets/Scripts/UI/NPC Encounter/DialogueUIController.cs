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
using TMPro;

public class DialogueUIController : Singleton<DialogueUIController>
{
    [Required][SerializeField] private EnergyBar energyBar;
    [Required][SerializeField] private DisplayPlayerCardDialogue playerDialogueBubble;
    [Required][SerializeField] private DeckDisplayer deckDisplay;
    [Tooltip("Relationship slider UI element")]
    [Required][SerializeField] private RelationshipSlider relationshipSlider;
    [Required][SerializeField] private DialogueBox dialogueBox;

    //i can make this a whole 'nother script if necessary but idk
    [SerializeField] private TMP_Text playCardButtonText;

    [Header("Progress Button Text")]
    public string CardSelectedText;
    public string CardNotSelectedText;
    public string EndDialogueText;


    private CardData selectedCard;
    private CardDisplay selectedDisplay;
    private bool closeCombat;


    [HideInInspector] public int NumberInList = 0;

    public void Initialize(DialogueBranch startBranch, characters character)
    {
        DialogueManager.OnOpenCombatUI(startBranch, character);

        // all the rest of this ui initialization stuff is gonna run every time an npc combat encounter happens.
        // i think our game is not complicated enough that its gonna be a problem performance wise, 
        // but its gonna bug me that its happening extra times

        energyBar.Initalize(DialogueManager.MaxEnergy);
        relationshipSlider.Initialize(RelationshipManager.characterRelationships[character].maxValue, RelationshipManager.characterRelationships[character].currentValue);
        deckDisplay.SetDeck(ref DialogueManager.PlayerHand);
        dialogueBox.Initialize(startBranch);

    }
    
    public void UpdateHoveringCard(CardData card)
    {
        // card is null, it hides the text bubble
        playerDialogueBubble.WriteText(card);

    }

    public void UpdateSelection(CardData card, bool cardSelected, CardDisplay display)
    {

        if(cardSelected)
        {

            playCardButtonText.text = CardSelectedText;

            if (selectedCard != null)
            {

                selectedDisplay.selected = false;

            }

            selectedCard = card;
            selectedDisplay = display;

        }
        else if (!cardSelected)
        {

            playCardButtonText.text = CardNotSelectedText;
            selectedCard = null;
            selectedDisplay = null;

        }

    }

    //i can move this to a different script later if necessary but for now the play card button is tied to this
    public void ProgressDialogue()
    {

        if(closeCombat)
        {

            UITransitionManager.CloseMenu();
            return;

            //this definitely duplicates cards atm but i'm assuming this won't be an issue once the ui isn't automatically generating cards. otherwise i can fix this

        }

        if(selectedCard != null)
        {

            StartCoroutine(DialogueManager.ProgressDialogue(selectedCard));

            playCardButtonText.text = CardNotSelectedText;

        }
        else { StartCoroutine(DialogueManager.ProgressDialogue(null)); }

    }

    public void UpdateDialogueDisplay(DialogueBranch branch, int numberInList)
    {

        dialogueBox.ProgressDialogue(branch, numberInList);
        selectedCard = null;

    }

    public void ClosingOutCombat()
    {

        playCardButtonText.text = EndDialogueText;
        closeCombat = true;

    }

    public void HideDeck()
    {
        if(deckDisplay.gameObject)
        {

            deckDisplay.gameObject.SetActive(false);

        }

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
