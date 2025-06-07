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
    [SerializeField] private List<CardData> cards = new List<CardData>();

    public List<CardData> Cards { get => cards;}
    #endregion

    #region Functions

    /// <summary>
    /// Adds a card into the deck
    /// </summary>
    /// <param name="newCard">The card to add to the deck</param>
    public void Add(CardData newCard)
    {
        cards.Add(newCard/*.CopyCard(newCard)*/);  // commenting out CopyCard to bridge disconnect between copies of deck (in case player modifies a card)
        Debug.Log(cards.Count + " cards in player deck");
    }

    /// <summary>
    /// Removes a card from the deck
    /// </summary>
    /// <param name="toRemove">The card to remove from the deck</param>
    public void Remove(CardData toRemove)
    {
        if (cards.Count > 0)
        {
            cards.Remove(toRemove);
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
        int cardRef = cards.FindIndex(x=> x == oldCard);
        cards[cardRef] = newCard;
    }


    /// <summary>
    /// Pops the top card in the deck
    /// </summary>
    /// <returns></returns>
    public CardData GetTop()
    {
        if(cards.Count >= 0)
        {
            CardData toReturn = cards[0];
            cards.RemoveAt(0);
            //Debug.Log(cards.Count + " cards left");
            return toReturn;
        }
        throw new System.Exception("No cards in Deck");
    }


    /// <summary>
    /// Shuffles the deck
    /// </summary>
    public void Shuffle()
    {
        // refactored with O(n) shuffle. old implementation (still exists in Shuffled) could have (in theory) run forever i think?
        cards.Shuffle();
    }

    /// <summary>
    /// Returns the deck shuffled
    /// </summary>
    /// <returns>Returns the shuffled Deck as type Deck</returns>
    public Deck Shuffled()
    {
        Deck preShuffle = GetCopy();
        int numOfElements = cards.Count;
        int newIndex = 0;
        bool[] usedSpace = new bool[numOfElements];
        for (int i = 0; i < numOfElements; i++)
        {
            do
            {
                newIndex = Random.Range(0, numOfElements);

            } while (usedSpace[newIndex] == true);
            cards[newIndex] = preShuffle.cards[i];
            usedSpace[newIndex] = true;
        }
        return this;
    }

    private void WipeDeckElements(Deck deck)
    {
        foreach(CardData c in deck.cards)
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
        foreach(CardData c in cards)
        {
            deckCopy.Add(c);
        }
        return deckCopy;
    }

    public void Clear()
    {
        cards.Clear();
    }
    #endregion
}
