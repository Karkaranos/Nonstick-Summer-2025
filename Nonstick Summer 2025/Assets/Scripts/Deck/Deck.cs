/*****************************************************************************
// File Name :          Deck.cs
// Author :             Cade R. Naylor
// Creation Date :      June 2, 2025
//
// Brief Description :  Basic functionality for the game deck
                            - Card Addition
                            - Card Removal
                            - Card Updating
                            - Shuffling
                            - Deck Copying
                            - Top Retrieval
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Deck
{
    #region Variables
    [SerializeField] private List<CardData> _cards = new List<CardData>();

    public List<CardData> Cards { get => _cards;}
    #endregion

    #region Functions

    /// <summary>
    /// Adds a card into the deck
    /// </summary>
    /// <param name="newCard">The card to add to the deck</param>
    public void Add(CardData newCard)
    {
        _cards.Add(newCard/*.CopyCard(newCard)*/);  // commenting out CopyCard to bridge disconnect between copies of deck (in case player modifies a card)
    }


    public void Add(CardData[] newCards)
    {
        foreach(CardData c in newCards)
        {
            _cards.Add(c);
        }
    }

    /// <summary>
    /// Removes a card from the deck
    /// </summary>
    /// <param name="toRemove">The card to remove from the deck</param>
    public void Remove(CardData toRemove)
    {
        if (_cards.Count > 0)
        {
            _cards.Remove(toRemove);
            return;
        }
        throw new System.Exception("No cards in Deck");
    }

    /// <summary>
    /// When a card is updated, adjusts its emotion and intent to the new values
    /// </summary>
    /// <param name="oldCard">The values of the card, before change</param>
    /// <param name="newCard">The new card values</param>
    public void UpdateCard(CardData oldCard, CardData newCard)
    {
        int cardRef = _cards.FindIndex(x=> x == oldCard);
        _cards[cardRef] = newCard;
    }

    /// <summary>
    /// Shuffles the deck
    /// </summary>
    public void Shuffle()
    {
        // refactored with O(n) shuffle. old implementation (still exists in Shuffled) could have (in theory) run forever i think?
        _cards.Shuffle();
    }

    /// <summary>
    /// Returns the deck shuffled
    /// </summary>
    /// <returns>Returns the shuffled Deck as type Deck</returns>
    public Deck Shuffled()
    {
        Deck preShuffle = GetCopy();
        int numOfElements = _cards.Count;
        int newIndex = 0;
        bool[] usedSpace = new bool[numOfElements];
        for (int i = 0; i < numOfElements; i++)
        {
            do
            {
                newIndex = Random.Range(0, numOfElements);

            } while (usedSpace[newIndex] == true);
            _cards[newIndex] = preShuffle._cards[i];
            usedSpace[newIndex] = true;
        }
        return this;
    }

    private void WipeDeckElements(Deck deck)
    {
        foreach(CardData c in deck._cards)
        {
            c.Emotion = CardEmotion.NotSelected;
            c.Intention = CardIntention.NotSelected;
            c.EnergyCost = 0;
        }
    }

    /// <summary>
    /// Creates a copy of the current deck
    /// </summary>
    /// <returns>The copied deck, as type Deck</returns>
    public Deck GetCopy()
    {
        Deck deckCopy = new Deck();
        foreach(CardData c in _cards)
        {
            deckCopy.Add(c);
        }
        return deckCopy;
    }

    // Peeks and Pops
    #region Viewing Cards

    /// <summary>
    /// Pops the top card in the deck
    /// </summary>
    /// <returns></returns>
    public CardData Pop()
    {
        if (Cards.Count >= 0)
        {
            CardData toReturn = Cards[0];
            Cards.RemoveAt(0);
            //Debug.Log(Cards.Count + " cards left");
            return toReturn;
        }
        throw new System.Exception("No cards in Deck");
    }

    public CardData Peek()
    {
        if (Cards.Count >= 0)
        {
            return Cards[0];
        }
        throw new System.Exception("No cards in Deck");
    }

    /// <summary>
    /// Peeks and returns the top n cards
    /// Will always return at least one card
    /// </summary>
    /// <param name="n">Number of cards to be returned</param>
    /// <returns>An array with the top n cards, in order</returns>
    public CardData[] PeekNCards(int n)
    {
        // Return at least the top card if n<0
        if (n < 0)
        {
            return new CardData[] { Peek() };
        }

        // Returning n cards
        if (Cards.Count >= n)
        {
            CardData[] result = new CardData[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = Cards[i];
            }
            return result;
        }

        // Returning <n cards
        else if (Cards.Count >= 0)
        {
            CardData[] result = new CardData[Cards.Count - 1];
            for (int i = 0; i < Cards.Count; i++)
            {
                result[i] = Cards[i];
            }
            return result;
        }

        throw new System.Exception("No cards in Deck");
    }

    /// <summary>
    /// Pops and returns the top n cards
    /// Cards are removed from the deck
    /// Will always return at least one card
    /// </summary>
    /// <param name="n">Number of cards to be returned</param>
    /// <returns>An array with the top n cards, in order</returns>
    public CardData[] PopNCards(int n)
    {
        // Return at least the top card if n<0
        if (n < 0)
        {
            return new CardData[] { Pop() };
        }

        // Returning n cards
        if (Cards.Count >= n)
        {
            CardData[] result = new CardData[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = Pop();
            }
            return result;
        }

        // Returning <n cards
        else if (Cards.Count >= 0)
        {
            CardData[] result = new CardData[Cards.Count - 1];
            for (int i = 0; i < Cards.Count; i++)
            {
                result[i] = Pop();
            }
            return result;
        }

        throw new System.Exception("No cards in Deck");
    }

    #endregion


    public void Clear()
    {
        _cards.Clear();
    }
    #endregion
}
