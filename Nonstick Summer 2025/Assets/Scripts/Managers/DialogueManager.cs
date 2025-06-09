/*****************************************************************************
* File Name :         DialogueManager.cs
* Author :            Toby
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
    public static UnityEvent OnCardPlayed = new UnityEvent();
    public static DialogueBranch CurrentDialogueBranch { get; private set; }
    public static bool PlayerInCombat => DialogueUIController.Instance != null;

    // these variables might get moved to a different script. 
    // idk if a 'discarded' variable is necessary so im just gonna not do that.
    // i see a big problem where if the player modifies a card in their deck, which deck gets updated? how do we bridge the gaps between these multiple decks? 
    public static Deck PlayerHand, RemainingDeck;
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

    public static void OnOpenCombatUI(DialogueBranch startDialogueBranch)
    {
        ReadUserInput = true;
        CurrentDialogueBranch = startDialogueBranch;

        // testing only: please delete later
        PlayerHand.Add(CardData.NewCard(1, CardEmotion.Blue, CardIntention.Intention1));
        PlayerHand.Add(CardData.NewCard(0, CardEmotion.Yellow, CardIntention.Intention2));
        PlayerHand.Add(CardData.NewCard(-3, CardEmotion.Red, CardIntention.Intention3));
    }

    /// <summary>
    /// Coroutine because i just know theres going to be animations later
    /// TODO: call this coroutine
    /// </summary>
    public static IEnumerator ProgressDialogue(CardData playedCard)
    {
        ReadUserInput = false;

        if (_currentEnergy <= 0 && playedCard != null)
            Debug.LogWarning("Card played with 0 energy");

        // reference for other programmers: 'yield return' stops this coroutine until the next coroutine is finished
        yield return SetCurrentEnergy(_currentEnergy + 
            (playedCard == null ? _energyGainedIfSilent: playedCard.GetEnergyCost())); // this could have been an if statement but noooooo i just had to be special

        // TODO: see 'Progress Dialogue and Process Cards' task
        // hi jay

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

    private static IEnumerator TryEnergyLossDeath() // bad function name, but brain kinda blank on a better one rn
    {
        if(_currentEnergy > 0)
            yield break; // not dead :)

        throw new NotImplementedException();

        yield return null;
    }

    
}
