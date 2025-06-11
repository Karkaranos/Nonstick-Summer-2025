/*****************************************************************************
// File Name :         DeckManager.cs
// Author :            Cade R. Naylor
// Creation Date :     June 3, 2025
//
// Brief Description :  Instance to handle all deck events for any deck
*****************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class DeckManager
{
    public static DeckManager Instance => GameManager.DeckManagerReference;

    public Deck PlayerDeck = new Deck();

    private static GameObject _deckDisplayObj;
    
    #region Initialization
    /// <summary>
    /// Runs after the first scene has finished loading
    /// Snags a reference to the deck display location
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSceneLoad()
    {
        _deckDisplayObj = GameObject.Find("DeckDisplay");
    }

    #endregion

    #region Function References
    /// <summary>
    /// Adds a card to any deck
    /// </summary>
    /// <param name="c">The card to be added</param>
    /// <param name="d">The deck to add to. Leave blank for Player's Deck</param>
    public static void AddCard(CardData c, Deck d = null)
    {
        CheckDeck(ref d);
        d.Add(c);
    }

    /// <summary>
    /// Removes a card from any deck
    /// </summary>
    /// <param name="c">The card to be removed</param>
    /// <param name="d">The deck to be removed from. Leave blank for Player's Deck</param>
    public static void RemoveCard(CardData c, Deck d = null)
    {
        CheckDeck(ref d);
        d.Remove(c);
    }

    /// <summary>
    /// Updates a card in any deck
    /// </summary>
    /// <param name="oldCard">The old values of the card</param>
    /// <param name="newCard">The new values of the card</param>
    /// <param name="d">The deck to update the card in. Leave blank for Player's Deck</param>
    public static void UpdateCard(CardData oldCard, CardData newCard, Deck d = null)
    {
        CheckDeck(ref d);
        d.UpdateCard(oldCard, newCard);
    }

    /// <summary>
    /// Creates a copy of any deck
    /// </summary>
    /// <param name="d">The deck to create a copy of. Leave blank for Player's Deck</param>
    /// <returns>The copied deck</returns>
    public static Deck CopyDeck(Deck d = null)
    {
        CheckDeck(ref d);
        return d.GetCopy();
    }

    /// <summary>
    /// Gets the top card of any deck
    /// </summary>
    /// <param name="d">The deck to get the top card of. Leave blank for Player's Deck</param>
    /// <returns>The top card of the specified deck</returns>
    public static CardData PopTopCard(Deck d = null)
    {
        CheckDeck(ref d);
        return d.Pop();
    }

    /// <summary>
    /// Shuffles any deck
    /// </summary>
    /// <param name="d">The deck to be shuffled. If using Player's Deck, must specify it</param>
    public static void ShuffleDeck(ref Deck d)
    {
        CheckDeck(ref d);
        d.Shuffle();
    }

    /// <summary>
    /// 
    /// </summary>
    public static void DisplayDeck()
    {

    }

    #endregion

    #region Helper Functions
    /// <summary>
    /// Checks whether a deck was passed in
    /// If no deck was provided, set the affected deck to the Player's Deck
    /// </summary>
    /// <param name="d">The optional deck</param>
    private static void CheckDeck(ref Deck d)
    {
        if (d!=null)
        {
            return;
        }
        d = Instance.PlayerDeck;
    }
    #endregion

  


}