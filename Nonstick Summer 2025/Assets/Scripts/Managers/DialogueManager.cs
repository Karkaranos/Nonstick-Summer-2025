/*****************************************************************************
* File Name :         DialogueManager.cs
* Author :            Toby, Sky
* Creation Date :     June 6, 2025
*
* Brief Description :  The big script that bridges all the modular components the combat system.
* (See documentation for better description)
*
* TODO:
* Processing _cards and dialogue progression (see task)
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

    public static DialogueBranch CurrentDialogueBranch { get; private set; }

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

        yield return TryEnergyLossDeath();
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
        _energyGainedIfSilent = energyGainedPerRound;
        MaxEnergy = maxEnergy;

        CurrentEnergy = defaultEnergy;
        DefaultCardsInHand = defaultCardsInHand;
    }

    public static void OnOpenCombatUI(DialogueBranch startDialogueBranch, characters character)
    {
        CurrentDialogueBranch = startDialogueBranch;
        currentCharacter = character;
    }

    /// <summary>
    /// Coroutine because i just know theres going to be animations later
    /// </summary>
    public static IEnumerator ProgressDialogue(CardData playedCard)
    {
        if (_currentEnergy <= 0 && playedCard != null)
            Debug.LogWarning("Card played with 0 energy");

        // reference for other programmers: 'yield return' stops this coroutine until the next coroutine is finished
        yield return SetCurrentEnergy(_currentEnergy + 
            (playedCard == null ? _energyGainedIfSilent: playedCard.EnergyCost)); // this could have been an if statement but noooooo i just had to be special


        var dialogueOption = CurrentDialogueBranch.ReturnDialogueOption(playedCard);
        yield return SetCurrentRelationshipStatus(CurrentRelationshipScore + dialogueOption.ChangeInRelationshipStatus);


        // TODO: see 'Progress Dialogue and Process Cards' task
        // hi jay

        yield return SetCurrentEnergy(_currentEnergy + _energyGainedPerRound);
    }

    /// <summary>
    /// TODO: Call this function at start of 'moment'
    /// </summary>
    public static void OnMomentStarted()
    {
        CurrentEnergy = _defaultEnergy;
        RemainingDeck = DeckManager.CopyDeck().Shuffled();
        PlayerHand.Clear();
    }

    private static IEnumerator TryEnergyLossDeath() // bad function name, but brain kinda blank on a better one rn
    {
        if(_currentEnergy > 0)
            yield break; // not dead :)

        throw new NotImplementedException();

        yield return null;
    }

    
}
