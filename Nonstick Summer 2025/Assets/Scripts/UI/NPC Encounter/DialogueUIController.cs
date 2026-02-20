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
using UnityEngine.SceneManagement;

public class DialogueUIController : Singleton<DialogueUIController>
{
    [Header("Components")]
    [Required][SerializeField] private EnergyBar energyBar;
    [Required][SerializeField] private DisplayPlayerCardDialogue playerDialogueBubble;
    [Required][SerializeField] private TMP_Text npcName;
    [Required][SerializeField] private DeckDisplayer deckDisplay;
    [Tooltip("Relationship slider UI element")]
    [Required][SerializeField] private RelationshipSlider relationshipSlider;
    [Required, SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private DialogueTree dialogueTree;
    [Required, SerializeField] public  DialogueNPCPortraitDisplay portraitDisplay;
    [Required, SerializeField] private DrawButton drawButton;
    [Required, SerializeField] private DiscardButton discardButton;
    [Required, SerializeField] private SilentButton silentButton;
    [Required, SerializeField] private PlayCardButton playCardButton;
    [Required, SerializeField] private NextDialogueButton nextDialogueButton;
    [Required, SerializeField] private TMP_Text objectiveText;

    public DeckDisplayer DeckDisplay { get { return deckDisplay; } }

    public CardData selectedCardData=> deckDisplay.FirstSelectedCard;
    public bool IfCloseCombat { get { return 
                DialogueManager.CurrentDialogueBranch == null
                || ( DialogueManager.CurrentDialogueBranch.End && PlayerReadAllNPCText); } }
    private bool _ui_interactable = true;

    [HideInInspector] public bool gainEnergy = false;

    private bool isBoss;
    private GameObject inWorldCharacter;

    [HideInInspector] public GameObject activeReaction;
    public GameObject EnabledButton;

    private Scene currentScene;
    [HideInInspector] public bool inSceneFive = false;
    public bool PlayerReadAllNPCText => dialogueBox.PlayerReadAllDialogue;

    public IEnumerator Initialize(DialogueBranch startBranch, Character character, bool isBoss = true, GameObject objRef = null)
    {
        DialogueManager.OnOpenCombatUI(startBranch, character);

        if(playCardButton == null) playCardButton = transform.GetComponentInChildren< PlayCardButton>();    

        //MusicManager.instance.StartCombat(0);

        Instance.isBoss = isBoss;
        inWorldCharacter = objRef;

        // all the rest of this ui initialization stuff is gonna run every time an npc combat encounter happens.
        // i think our game is not complicated enough that its gonna be a problem performance wise, 
        // but its gonna bug me that its happening extra times

        // initialize all components
        energyBar.Initalize();
        relationshipSlider.Initialize(RelationshipManager.characterRelationships[character].maxValue, RelationshipManager.characterRelationships[character].currentValue);
        deckDisplay.SetDisplayDeck(ref DeckManager.PlayerHand);
        deckDisplay.DrawToDefaultHand();
        deckDisplay.UpdateGroupEnabled(false);
        drawButton.Initialize();
        discardButton.Initialize();
        silentButton.Initialize();
        playCardButton.Initialize();
        nextDialogueButton.Initialize();

        deckDisplay.OnCardsSelectedChanged.AddListener(OnSelectionUpdated);

        npcName.text = character.ToString(); // none of the Character have spaces in their names, right?

        if(dialogueTree != null)
        {
            dialogueTree.Initialize(startBranch);
        }

        Debug.Log(TextUtilities.FilterText(GameManager.ObjectiveReference.GetObjective()));
        objectiveText.text = TextUtilities.FilterText( GameManager.ObjectiveReference.GetObjective());

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Moment_5")
        {

            inSceneFive = true;

        }
        else { inSceneFive = false; }

        yield return ToggleUIForDialogueProgression(false);

        //yield return OpenCombatUI_Coroutine();

        yield return dialogueBox.Initialize(startBranch, character);

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

        /*if(card!=null)
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CardHoverSFX);*/
    }

    private void OnSelectionUpdated()
    {
        // Card movement animation is handled in DeckDisplayer / CardDisplay_PositionAnimator

        AudioManager.instance.PlayOneShot(FMODEvents.instance.CardSelectSFX);

        // REALLY dont like putting this function here, since playCardButton should be autonomous but fuck it atp
        playCardButton.UpdateButtonEnabled();

        if(selectedCardData == null)
        {
            // nothing i think
        }
        else
        {
            playerDialogueBubble.WriteText(selectedCardData);

            if (Mathf.Abs(selectedCardData.EnergyCost) > DialogueManager.CurrentEnergy && inSceneFive == false)
            {

                EnabledButton.SetActive(false);

            }
            else
            {

                EnabledButton.SetActive(true);

            }
        }
    }

    //i can move this to a different script later if necessary but for now the play card button is tied to this
    //TODO move to play button script
    public void NextTextPressed()
    {
        if(IfCloseCombat)
        {
            // TODO open a new menu?
            Debug.Log("Close combat!");
            if(isBoss)
            {
                UITransitionManager.CloseMenu();
                GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.FINISH_COMBAT);

                //okay yes this is bad code but it was dereferencing again and this should fix it
                if (!GameManager.ObjectiveReference)
                    GameManager.ObjectiveReference = FindFirstObjectByType<Objectives>(FindObjectsInactive.Include);
                GameManager.ObjectiveReference.SetObjectiveVisibility(true);
            }
            else
            {
                UITransitionManager.CloseMenu(false, false);
                GameManager.ObjectiveReference.MetCondition(ObjectiveConditions.TALK_TO_SIDE_CHARACTER, inWorldCharacter);
                if (!GameManager.ObjectiveReference)
                    GameManager.ObjectiveReference = FindFirstObjectByType<Objectives>(FindObjectsInactive.Include);
                GameManager.ObjectiveReference.SetObjectiveVisibility(true);
                inWorldCharacter.GetComponent<SideCharacterInteractable>().FinishSideCombat();
            }

            return;
        }

        if(activeReaction != null)
        {

            DestroyImmediate(activeReaction);

        }

        if(gainEnergy)
        {

            DialogueManager.GainEnergyAfterTurn();
            gainEnergy = false;

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
        dialogueBox.MuffleTextPlayed("What was that?");
    }

    public void UpdateDialogueTreeVisual(DialogueBranch branch)
    {

        dialogueTree.HighlightActiveNode(branch);

    }

    public void ClosingOutCombat()
    {
        if (DialogueManager.CurrentDialogueBranch.End)
        {
            MusicManager.instance.StartHouse(); // house md???
        }
    }

    public IEnumerator ToggleUIForDialogueProgression(bool interactable)
    {
        if (_ui_interactable == interactable)
            yield break;

        _ui_interactable = interactable;

        deckDisplay.UpdateGroupEnabled(interactable);

        yield return null;

    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateEnergy(float? value=null)
    {
        yield return energyBar.SetValue(value ?? DialogueManager.CurrentEnergy);
    }

    // Coroutine to handle animation (in the future)
    public IEnumerator UpdateRelationship(float? value, Character character)
    {
        yield return relationshipSlider?.SetValue(value ?? RelationshipManager.characterRelationships[character].currentValue);
    }
}

