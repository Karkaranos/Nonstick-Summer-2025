/*****************************************************************************
* File Name :         DialogueManager.cs
* Author :            Toby, Sky
* Creation Date :     June 6, 2025
*
* Brief Description :  The big script that bridges all the modular components the combat system.
* (See documentation for better description)
*
* TODO:
* Processing Cards and dialogue progression (see task)
* Visual feedback
* Exiting combat
* literally everything else
* Call OnMomentStarted function at start of 'OnMomentStarted'
* 
*****************************************************************************/

using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager
{
    public static UITransitionManager Instance => GameManager.UITransitionManagerReference;

    public static bool ReadUserInput;
    public static bool UserCanPlayCard=>ReadUserInput && DialogueUIController.Instance.PlayerReadAllNPCText;
    public static UnityEvent OnCardPlayed = new UnityEvent();
    public static DialogueBranch CurrentDialogueBranch { get; private set; }
    public static bool PlayerInCombat => DialogueUIController.Instance != null;

    // these variables might get moved to a different script. 
    // idk if a 'discarded' variable is necessary so im just gonna not do that.
    // i see a big problem where if the player modifies a card in their deck, which deck gets updated? how do we bridge the gaps between these multiple decks? 
    public static Deck PlayerHand, RemainingDeck;
    private static characters currentCharacter;
    public static float CurrentRelationshipScore => RelationshipManager.characterRelationships[currentCharacter].currentValue;
    public static int CurrentEnergy { 
        get { return _currentEnergy; }
        set { SetCurrentEnergy(value); }
    }
    private static int _currentEnergy;

    // parameters
    private static int _defaultEnergy, _energyGainedPerRound, _energyGainedIfSilent;
    public static int MaxEnergy;
    public static int DefaultCardsInHand { get; private set; }

    #region Getters and setters

    public static IEnumerator SetCurrentEnergy(int energy)
    {
        energy = Mathf.Min(energy, MaxEnergy);
        if (_currentEnergy == energy) yield break;

        _currentEnergy = energy;
        if (DialogueUIController.Instance != null)
            yield return DialogueUIController.Instance.UpdateEnergy(energy); // wait for animation to finish
    }

    public static IEnumerator SetCurrentRelationshipStatus(float relationshipScore)
    {
        if (CurrentRelationshipScore == relationshipScore)
        {
            yield break;
        }

        RelationshipManager.characterRelationships[currentCharacter].currentValue = relationshipScore;

        if(DialogueUIController.Instance != null)
        {
            yield return DialogueUIController.Instance.UpdateRelationship(relationshipScore, currentCharacter);
        }
    }

    #endregion

    public DialogueManager(int defaultEnergy, int energyGainedPerRound, int energyGainedIfSilent, int maxEnergy, int defaultCardsInHand)
    {
        _defaultEnergy = defaultEnergy;
        _energyGainedPerRound = energyGainedPerRound;
        _energyGainedIfSilent = energyGainedIfSilent;
        MaxEnergy = maxEnergy;
        DefaultCardsInHand = defaultCardsInHand;

        CurrentEnergy = defaultEnergy;
        PlayerHand = new Deck();
    }

    public static void OnOpenCombatUI(DialogueBranch startDialogueBranch, characters character)
    {
        ReadUserInput = false;
        CurrentDialogueBranch = startDialogueBranch;
        currentCharacter = character;


        // testing only: please delete later
        //PlayerHand.Clear();
        PlayerHand.Add(CardData.NewCard(1, CardEmotion.Blue, CardIntention.Intention1));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Yellow, CardIntention.Intention2));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Red, CardIntention.Intention3));
        PlayerHand.Add(CardData.NewCard(2, CardEmotion.Red, CardIntention.Intention1));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Blue, CardIntention.Intention2));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Yellow, CardIntention.Intention3));
        PlayerHand.Add(CardData.NewCard(1, CardEmotion.Yellow, CardIntention.Intention1));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Red, CardIntention.Intention2));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Blue, CardIntention.Intention3));
        Debug.LogWarning("Added 1 of each hard-coded test cards to hand.");
        PlayerHand.Shuffle();

        GameManager.Instance.StartCoroutine(OpenCombatUI_Coroutine()); // it needs A monobehavior to start a coroutine, and i didnt know which else
    }

    private static IEnumerator OpenCombatUI_Coroutine()
    {
        yield return DialogueUIController.Instance.UpdateNPCDialogueDisplay();
    }

    /// <summary>
    /// The big function that ties together everything. updates ui and processes a card
    /// </summary>
    public static IEnumerator ProcessPlayCard(CardData playedCard)
    {
        Debug.Log("Player playing card");
        ReadUserInput = false;

        yield return DialogueUIController.Instance.ToggleUIForDialogueProgression(false);

        if (_currentEnergy <= 0 && playedCard != null)
            Debug.LogWarning("Card played with 0 energy");

        yield return SetCurrentEnergy(_currentEnergy + 
            (playedCard == null ? _energyGainedIfSilent: playedCard.GetEnergyCost())); // this could have been an if statement but noooooo i just had to be special

        var dialogueOption = CurrentDialogueBranch.ReturnDialogueOption(playedCard);

        // progress dialogue
        CurrentDialogueBranch = dialogueOption.BranchingDialogue;
        yield return DialogueUIController.Instance.UpdateNPCDialogueDisplay();

        yield return SetCurrentRelationshipStatus(CurrentRelationshipScore + dialogueOption.ChangeInRelationshipStatus);

        yield return DialogueUIController.Instance.ToggleUIForDialogueProgression(DialogueUIController.Instance.PlayerReadAllNPCText);

        yield return SetCurrentEnergy(_currentEnergy + _energyGainedPerRound);

        Debug.Log("Completed processing card");
        ReadUserInput = true;
    }

    /// <summary>
    /// TODO: Call this function at start of 'moment'
    /// </summary>
    public static void OnMomentStarted()
    {
        ReadUserInput = false;
        CurrentEnergy = _defaultEnergy;
        RemainingDeck = DeckManager.CopyDeck().Shuffled();
        PlayerHand.Clear();
    }
    
}
