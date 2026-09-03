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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager
{
    public static UITransitionManager Instance => GameManager.UITransitionManagerReference;

    public static bool ReadUserInput;
    public static UnityEvent OnPlayerFinishReadingDialogue = new UnityEvent();
    public static UnityEvent OnCardPlayedStarted = new UnityEvent();
    public static UnityEvent OnCardPlayedFinished = new UnityEvent();
    public static DialogueBranch CurrentDialogueBranch { get; private set; }
    public static bool UserCanPlayCard => ReadUserInput && DialogueUIController.Instance.PlayerReadAllNPCText;
    public static bool PlayerInCombat => DialogueUIController.Instance != null;

    private static Character currentCharacter;
    public static float CurrentRelationshipScore => RelationshipManager.characterRelationships[currentCharacter].currentValue;
    public static float CurrentEnergy { 
        get { return _currentEnergy; }
        set { if (GameManager.Instance == null) return;
            GameManager.Instance.StartCoroutine(SetCurrentEnergy(value)); }
    }
    private static float _currentEnergy;

    private static bool continueCardProcessing = false;

    // parameters
    private static float _defaultEnergy, _energyGainedPerRound;
    public static float MaxEnergy, DrawButtonEnergyCost, EnergyGainedPerDiscard, EnergyGainedIfSilent;
    public static int DefaultCardsInHand, CardsDrawnPerRound;

    public static bool hasBeenSilentEveryTurn = true;

    #region calculation variables

    private static bool playedCardSinceOpeningCombat = false;

    #endregion

    #region Getters and setters

    public static IEnumerator SetCurrentEnergy(float energy)
    {
        if(!DialogueUIController.Instance.inSceneFive)
        {
            energy = Mathf.Clamp(energy, 0, MaxEnergy);
            if (_currentEnergy == energy) yield break;

            Debug.Log($"set energy to {_currentEnergy}");
            _currentEnergy = energy;
            if (DialogueUIController.Instance != null)
                yield return DialogueUIController.Instance.UpdateEnergy(_currentEnergy); // wait for animation to finish
        }
    }

    public static IEnumerator SetCurrentRelationshipStatus(float relationshipScore)
    {
        if (CurrentRelationshipScore == relationshipScore)
        {
            yield break;
        }

        if (CurrentRelationshipScore > relationshipScore)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.NegRelationSFX);
            Debug.Log("NegativeSFX played");
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PosRelationSFX);
        }

        if(relationshipScore > RelationshipManager.characterRelationships[currentCharacter].maxValue)
        {
            relationshipScore = RelationshipManager.characterRelationships[currentCharacter].maxValue;
        }
        RelationshipManager.characterRelationships[currentCharacter].currentValue = relationshipScore;

        if(relationshipScore < 0)
        {

            relationshipScore = 0;

        }

        if(DialogueUIController.Instance != null)
        {
            yield return DialogueUIController.Instance.UpdateRelationship(relationshipScore, currentCharacter);
        }
    }

    #endregion

    public DialogueManager(float defaultEnergy, float energyGainedPerRound, float energyGainedIfSilent, float maxEnergy, 
        int defaultCardsInHand, int cardsDrawnPerRound, float drawButtonEnergyCost, float energyGainedPerDiscard)
    {
        _defaultEnergy = defaultEnergy;
        _energyGainedPerRound = energyGainedPerRound;
        EnergyGainedIfSilent = energyGainedIfSilent;
        MaxEnergy = maxEnergy;
        DefaultCardsInHand = defaultCardsInHand;
        CardsDrawnPerRound = cardsDrawnPerRound;
        DrawButtonEnergyCost = drawButtonEnergyCost;
        EnergyGainedPerDiscard = energyGainedPerDiscard;

        _currentEnergy = defaultEnergy;
    }

    //so apparently this only gets called in the very first scene??
    //anyhow BOOYAH LET'S GO BUG FIXING
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnMomentStarted()
    {
        Debug.Log("on moment started");
        ReadUserInput = false;
        CurrentEnergy = _defaultEnergy;
    }

    public static void OnOpenCombatUI(DialogueBranch startDialogueBranch, Character character)
    {
        ReadUserInput = false;
        CurrentDialogueBranch = startDialogueBranch;
        currentCharacter = character;
        playedCardSinceOpeningCombat = false;
        hasBeenSilentEveryTurn = true;

        // testing only: please delete later
        // ok i did it
    }

    public static void StopCardProcessing()
    {
        continueCardProcessing = true;

        //hardcoded for now; fix so any string works later
        DialogueUIController.Instance.MuffleText();

        OnCardPlayedFinished.Invoke();
    }

    /// <summary>
    /// The big function that ties together everything. updates ui and processes a card
    /// </summary>
    public static IEnumerator ProcessPlayCard(CardData playedCard)
    {
        if (playedCard != null)
            hasBeenSilentEveryTurn = false;

        DialogueUIController.Instance.UpdateHoveringCard(playedCard);

        continueCardProcessing = false;
        playedCardSinceOpeningCombat = true;
        ReadUserInput = false;
        OnCardPlayedStarted.Invoke();

        if (playedCard == null)
            Debug.Log("Played silent card");
        else
            Debug.Log($"playing card: {playedCard.Emotion.ToString()}, {playedCard.Intention.ToString()}");

        if (playedCard != null)
            playedCard.TryTriggerStampEffect(StampTriggerConditions.BeforeCardPlayed);

        //TODO wait for potential _modifier animations to finish


        if (playedCard != null)
            DialogueUIController.Instance.deckDisplay.DiscardCard(playedCard);


        if (continueCardProcessing)
        {
            yield break;
        }

        yield return DialogueUIController.Instance.ToggleUIForDialogueProgression(false);

        if (CurrentEnergy <= 0 && playedCard != null)
            Debug.LogWarning("Card played with 0 energy");

        Debug.Log("before set");
        GameManager.Instance.StartCoroutine(SetCurrentEnergy(_currentEnergy + 
            (playedCard == null ? EnergyGainedIfSilent: playedCard.GetEnergyCost()))); // this could have been an if statement but noooooo i just had to be special
        Debug.Log("after set");

        DialogueUIController.Instance.gainEnergy = true;

        // progress dialogue:
        var dialogueOption = CurrentDialogueBranch.GetDialogueOption(playedCard);

        float relationshipChange = playedCard == null ? dialogueOption.ChangeInRelationshipStatus : playedCard.GetRelationshipChange(dialogueOption);
        //float relationshipChange = playedCardSinceOpeningCombat.GetRelationshipChange(dialogueOption);
        //yield return SetCurrentRelationshipStatus(CurrentRelationshipScore + relationshipChange);
        GameManager.Instance.StartCoroutine(SetCurrentRelationshipStatus(CurrentRelationshipScore + relationshipChange));

        // redundant if statement?
        if((CurrentRelationshipScore + relationshipChange) < 0)
            RelationshipManager.characterRelationships[currentCharacter].currentValue = 0;

        // progress dialogue:
        if (dialogueOption.RelationshipCheckRequired == false || RelationshipManager.characterRelationships[currentCharacter].currentValue > dialogueOption.RelationshipRange.y)
        {
            Debug.Log("Player has enough RP for good branch");
            CurrentDialogueBranch = dialogueOption.BranchingDialogueHigh; 

            if(DialogueUIController.Instance.inSceneFive)
            {
                DialogueUIController.Instance.bestEndingForCharacterReached = true;
            }
        }
        else if(dialogueOption.RelationshipCheckRequired = true && RelationshipManager.characterRelationships[currentCharacter].currentValue <= dialogueOption.RelationshipRange.y && RelationshipManager.characterRelationships[currentCharacter].currentValue >= dialogueOption.RelationshipRange.x)
        {
            Debug.Log("Player has met RP requirement");
            CurrentDialogueBranch = dialogueOption.BranchingDialogueNeutral; 
        }
        else if(dialogueOption.RelationshipCheckRequired = true && RelationshipManager.characterRelationships[currentCharacter].currentValue < dialogueOption.RelationshipRange.x)
        {

            Debug.Log("Player has not met RP requirement");
            CurrentDialogueBranch = dialogueOption.BranchingDialogueLow;

        }

        DialogueUIController.Instance.UpdateDialogueTreeVisual(CurrentDialogueBranch);

        yield return DialogueUIController.Instance.ResetNPCDialogue(dialogueOption);

        // TODO: move this to AFTER player reads all text, and can play cards again
        //GameManager.Instance.StopCoroutine(SetCurrentEnergy(_currentEnergy));
        //GameManager.Instance.StartCoroutine(SetCurrentEnergy(_currentEnergy + _energyGainedPerRound));

        if(playedCard != null)
            playedCard.TryTriggerStampEffect(StampTriggerConditions.AfterCardPlayed);
        //TODO wait for potential _modifier animations to finish

        Debug.Log("Completed processing card");
        // only keep reading user input if theres more
        ReadUserInput = true;//!CurrentDialogueBranch.End; 

        //if(playedCard != null)
            //MoodManager.UpdateMood(playedCard.Emotion);

        OnCardPlayedFinished.Invoke();
    }

    public static void GainEnergyAfterTurn()
    {

        GameManager.Instance.StartCoroutine(SetCurrentEnergy(_currentEnergy + _energyGainedPerRound));

    }

    /// <summary>
    /// When player has read all sets of dialogue in a branch
    /// </summary>
    public static void FinishReadingDialogue()
    {
        DialogueUIController.Instance.UpdateHoveringCard(null);
        DialogueUIController.Instance.StartCoroutine(DialogueUIController.Instance.ToggleUIForDialogueProgression(true));
        ReadUserInput = true;
        DrawCards();
        OnPlayerFinishReadingDialogue.Invoke();
    }

    public static void DrawCards(int? N=null, bool forceDraw = false)
    {
        N = N ?? CardsDrawnPerRound;

        if (!playedCardSinceOpeningCombat && !forceDraw)
            return;

        Debug.Log("drawing now");
        DialogueUIController.Instance.deckDisplay.DeselectAllCards();   

        for (int i = 0; i < N; i++)
        {
            if (DeckManager.RemainingDeck.Count >= 1)
            {
                //var nextCard = DeckManager.RemainingDeck.Pop();
                //PlayerHand.Add(nextCard, false);
                DialogueUIController.Instance.deckDisplay.DrawOneCard();
            }
            else
            {
                DialogueUIController.Instance.deckDisplay.DisplayAllCards();
                Debug.Log("No cards left to draw!");
            }
            // Called in draw one card
            //DialogueUIController.Instance.deckDisplay.DisplayAllCards();
        }
    }
}
