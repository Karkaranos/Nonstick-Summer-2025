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
    public static float CurrentEnergy { 
        get { return _currentEnergy; }
        set { SetCurrentEnergy(value); }
    }
    private static float _currentEnergy;

    // parameters
    private static float _defaultEnergy, _energyGainedPerRound, _energyGainedIfSilent;
    public static float MaxEnergy;
    public static int DefaultCardsInHand { get; private set; }

    #region Getters and setters

    public static IEnumerator SetCurrentEnergy(float energy)
    {
        energy = Mathf.Clamp(energy, 0, MaxEnergy);
        if (_currentEnergy == energy) yield break;

        Debug.Log($"set energy to {_currentEnergy}");
        _currentEnergy = energy;
        if (DialogueUIController.Instance != null)
            yield return DialogueUIController.Instance.UpdateEnergy(_currentEnergy); // wait for animation to finish
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

    public DialogueManager(float defaultEnergy, float energyGainedPerRound, float energyGainedIfSilent, float maxEnergy, int defaultCardsInHand)
    {
        _defaultEnergy = defaultEnergy;
        _energyGainedPerRound = energyGainedPerRound;
        _energyGainedIfSilent = energyGainedIfSilent;
        MaxEnergy = maxEnergy;
        DefaultCardsInHand = defaultCardsInHand;

        _currentEnergy = defaultEnergy;
        PlayerHand = new Deck();
    }

    public static void OnOpenCombatUI(DialogueBranch startDialogueBranch, characters character)
    {
        ReadUserInput = false;
        CurrentDialogueBranch = startDialogueBranch;
        currentCharacter = character;

        // testing only: please delete later
        //PlayerHand.Clear();
        PlayerHand.Add(CardData.NewCard(1, CardEmotion.Sappy, CardIntention.Expression));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Charming, CardIntention.Observation));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Assertive, CardIntention.Question));
        PlayerHand.Add(CardData.NewCard(2, CardEmotion.Assertive, CardIntention.Expression));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Sappy, CardIntention.Observation));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Charming, CardIntention.Question));
        PlayerHand.Add(CardData.NewCard(1, CardEmotion.Charming, CardIntention.Expression));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Assertive, CardIntention.Observation));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Sappy, CardIntention.Question));
        Debug.LogWarning("Added 1 of each hard-coded test cards to hand.");
        PlayerHand.Shuffle();
    }

    /// <summary>
    /// The big function that ties together everything. updates ui and processes a card
    /// </summary>
    public static IEnumerator ProcessPlayCard(CardData playedCard)
    {
        if (playedCard == null)
            Debug.Log("Played silent card");
        else
            Debug.Log($"playing card: {playedCard.Emotion.ToString()}, {playedCard.Intention.ToString()}");

        ReadUserInput = false;

        playedCard.TryTriggerStampEffect(StampTriggerConditions.BeforeCardPlayed);
        //TODO wait for potential _modifier animations to finish

        yield return DialogueUIController.Instance.ToggleUIForDialogueProgression(false);

        if (CurrentEnergy <= 0 && playedCard != null)
            Debug.LogWarning("Card played with 0 energy");

        Debug.Log("before set");
        GameManager.Instance.StartCoroutine(SetCurrentEnergy(_currentEnergy + 
            (playedCard == null ? _energyGainedIfSilent: playedCard.GetEnergyCost()))); // this could have been an if statement but noooooo i just had to be special
        Debug.Log("after set");

        // progress dialogue:
        var dialogueOption = CurrentDialogueBranch.ReturnDialogueOption(playedCard);

        float relationshipChange = playedCard.GetRelationshipChange(dialogueOption);
        yield return SetCurrentRelationshipStatus(CurrentRelationshipScore + relationshipChange);

        // progress dialogue:
        if (RelationshipManager.characterRelationships[currentCharacter].currentValue >= dialogueOption.RelationshipRequirement)
        {
            Debug.Log("Player has enough RP for good branch");
            CurrentDialogueBranch = dialogueOption.BranchingDialogue; 
        }
        else 
        {
            Debug.Log("Player has not met RP requirement");
            CurrentDialogueBranch = dialogueOption.AlternateBranch; 
        }

        yield return DialogueUIController.Instance.ResetNPCDialogue();

        // TODO: move this to AFTER player reads all text, and can play cards again
        GameManager.Instance.StartCoroutine(SetCurrentEnergy(_currentEnergy + _energyGainedPerRound));

        playedCard.TryTriggerStampEffect(StampTriggerConditions.AfterCardPlayed);
        //TODO wait for potential _modifier animations to finish

        Debug.Log("Completed processing card");
        // only keep reading user input if theres more
        ReadUserInput = true;//!CurrentDialogueBranch.End; 

        DialogueManager.PlayerHand.Remove(playedCard);

        MoodManager.UpdateMood(playedCard.Emotion);
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
