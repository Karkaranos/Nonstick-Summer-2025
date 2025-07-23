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
using UnityEngine.TextCore.Text;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DialogueUIController : Singleton<DialogueUIController>
{
    public bool PlayerReadAllNPCText => dialogueBox.PlayerReadAllDialogue;

    [Required][SerializeField] private EnergyBar energyBar;
    [Required][SerializeField] private DisplayPlayerCardDialogue playerDialogueBubble;
    [Required][SerializeField] private DeckDisplayer deckDisplay;
    [Tooltip("Relationship slider UI element")]
    [Required][SerializeField] private RelationshipSlider relationshipSlider;
    [Required, SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private DialogueTree dialogueTree;
    [Required, SerializeField] public  DialogueNPCPortraitDisplay portraitDisplay;
    [Required, SerializeField] private DrawButton drawButton;
    [Required, SerializeField] private DiscardButton discardButton;

    //i can make this a whole 'nother script if necessary but idk
    // TODO: ^
    [SerializeField] public Button playCardButton;
    [SerializeField] private TMP_Text playCardButtonText;

    public DeckDisplayer DeckDisplay { get { return deckDisplay; } }

    [Header("Progress Button Text")] 
    public string CardSelectedText;
    public string CardNotSelectedText;
    public string EndDialogueText;

    public CardData selectedCardData=> deckDisplay.FirstSelectedCard;
    private bool IfCloseCombat { get { return 
                DialogueManager.CurrentDialogueBranch == null
                || ( DialogueManager.CurrentDialogueBranch.End && PlayerReadAllNPCText); } }
    private bool _ui_interactable = true;

    private bool isBoss;
    private GameObject inWorldCharacter;

    public IEnumerator Initialize(DialogueBranch startBranch, characters character, bool isBoss = true, GameObject objRef = null)
    {
        DialogueManager.OnOpenCombatUI(startBranch, character);

        MusicManager.instance.StartCombat(0);

        Instance.isBoss = isBoss;
        inWorldCharacter = objRef;

        // all the rest of this ui initialization stuff is gonna run every time an npc combat encounter happens.
        // i think our game is not complicated enough that its gonna be a problem performance wise, 
        // but its gonna bug me that its happening extra times

        // initialize all components
        energyBar.Initalize();
        relationshipSlider.Initialize(RelationshipManager.characterRelationships[character].maxValue, RelationshipManager.characterRelationships[character].currentValue);
        deckDisplay.SetDisplayDeck(ref DeckManager.PlayerHand);
        DeckDisplay.DrawToDefaultHand();
        drawButton.Initialize();
        discardButton.Initialize();

        deckDisplay.OnCardsSelectedChanged.AddListener(OnSelectionUpdated);

        if(dialogueTree != null)
        {

            dialogueTree.Initialize(startBranch);

        }

        yield return ToggleUIForDialogueProgression(false);

        //yield return OpenCombatUI_Coroutine();

        yield return dialogueBox.Initialize(startBranch);
    }

    public IEnumerator OpenCombatUI_Coroutine()
    {
        yield return UpdateNextNPCDialogueDisplay();
    }

    #region Player Input 

    public void UpdateHoveringCard(CardData card)
    {
        // card is null, it hides the text bubble
        playerDialogueBubble.WriteText(card);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CardHoverSFX);
    }

    // TODO move a lot of this to play button script
    private void OnSelectionUpdated()
    {
        // Card movement animation is handled in DeckDisplayer / CardDisplay_PositionAnimator
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CardSelectSFX);

        if (selectedCardData == null)
        {
            // TODO move to play card button script
            playCardButtonText.text = CardNotSelectedText;

            playCardButton.SetColors(normalColor: Color.white, highlightedColor: Color.gray, selectedColor: Color.white, pressedColor: Color.gray);
        }
        else
        {
            playCardButtonText.text = CardSelectedText;

            var buttonColor = CardStyleManager.GetEmotionColor(selectedCardData);
            playCardButton.SetColors(normalColor: buttonColor, highlightedColor: buttonColor, selectedColor: buttonColor, pressedColor: buttonColor);

            playerDialogueBubble.WriteText(selectedCardData);
        }

        if (!DialogueManager.UserCanPlayCard)
        {
            // TODO dont hardcode this
            // TODO move to playcard button script
            playCardButtonText.text = "->";
            playCardButton.SetColors(normalColor: Color.white, highlightedColor: Color.gray, selectedColor: Color.white, pressedColor: Color.gray);

            return;
        }
    }

    //i can move this to a different script later if necessary but for now the play card button is tied to this
    //TODO move to play button script
    public void PlayCardPressed()
    {
        if(IfCloseCombat)
        {
            // TODO open a new menu?
            Debug.Log("Close combat!");
            UITransitionManager.CloseMenu();
            if(isBoss)
            {
                GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.FINISH_COMBAT);
                GameManager.ObjectiveReference.SetObjectiveVisibility(true);
            }
            else
            {
                GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.TALK_TO_SIDE_CHARACTER, inWorldCharacter);
                GameManager.ObjectiveReference.SetObjectiveVisibility(true);
                inWorldCharacter.GetComponent<SideCharacterInteractable>().GetModifier();
            }

            return;
        }

        // should this be playing EVERY time the button is pressed?
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CardPlaySFX);

        Debug.Log("Play button pressed");

        StartCoroutine(ToggleUIForDialogueProgression(false));

        //DialogueManager.PlayerHand.Remove(selectedCard);

        if (DialogueManager.UserCanPlayCard)
            // Play a card
            StartCoroutine(DialogueManager.ProcessPlayCard(selectedCardData));
        else
            // Next Dialogue pls
            StartCoroutine(UpdateNextNPCDialogueDisplay());
    }

    #endregion

    public IEnumerator UpdateNextNPCDialogueDisplay()
    {
        Debug.Log("progress dialogue");

        yield return dialogueBox.ProgressNPCDialogue(DialogueManager.CurrentDialogueBranch);

        // in case the npc text was only 1 blurb long. (updated in dialogueBox.ProgressNPCDialogue)
        if (PlayerReadAllNPCText && !DialogueManager.CurrentDialogueBranch.End)
        {
            DialogueManager.FinishReadingDialogue();
        }
    }
    public IEnumerator ResetNPCDialogue(DialogueOption option)
    {
        if (!DialogueManager.ReadUserInput)
        {
            Debug.Log("not while animations are playing!");

            yield return dialogueBox.LoadNewDialogue(DialogueManager.CurrentDialogueBranch, option);

            yield break;
        }

        Debug.Log("reset dialogue");

        // in case the npc text was only 1 blurb long. (updated in dialogueBox.LoadNewDialogue)
        if (PlayerReadAllNPCText)
        {
            if (DialogueManager.CurrentDialogueBranch.End)
            {
                playCardButtonText.text = EndDialogueText;
                yield return ToggleUIForDialogueProgression(false);
            }
            else
            {
                DialogueManager.FinishReadingDialogue();
            }
        }
    }

    public void MuffleText()
    {
        // hardcoded for now will fix so it can take a thing later
        dialogueBox.DisplayOneLine("What was that?");
    }
    public void UpdateDialogueTreeVisual(DialogueBranch branch)
    {

        dialogueTree.HighlightActiveNode(branch);

    }

    //TODO move to play button script
    public void ClosingOutCombat()
    {
        if (DialogueManager.CurrentDialogueBranch.End)
        {
            playCardButtonText.text = EndDialogueText;
            MusicManager.instance.StartHouse();

            if (isBoss)
            {
                var bed = FindFirstObjectByType<OpenConfirmationInteractable>();
                if (bed != null) bed.ClearBlocker();
            }
        }
    }

    public IEnumerator ToggleUIForDialogueProgression(bool interactable)
    {
        if (_ui_interactable == interactable)
            yield break;

        _ui_interactable = interactable;

        // TODO dont hardcode that text
        // TODO move to dedicated card play button script
        playCardButtonText.text = interactable ? CardNotSelectedText : "->";
        playCardButton.SetColors(normalColor: Color.white, highlightedColor: Color.gray, selectedColor: Color.white, pressedColor:Color.gray);

        //deckDisplay?.gameObject.SetActive(interactable);
        if (interactable)
            StaticUtilities.EnableCanvasGroup(deckDisplay.canvasGroup);
        else
            StaticUtilities.DisableCanvasGroup(deckDisplay.canvasGroup, alpha:0.2f);

        yield return null;

    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateEnergy(float? value=null)
    {
        yield return energyBar.SetValue(value ?? DialogueManager.CurrentEnergy);
    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateRelationship(float? value, characters character)


    {
        yield return relationshipSlider?.SetValue(value ?? RelationshipManager.characterRelationships[character].currentValue);
    }
}

