using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Deck
{
    #region Variables
    private List<CardData> playerDeck;

    public List<CardData> PlayerDeck { get => playerDeck;}
    #endregion

    #region Functions

    /// <summary>
    /// Adds a card into the deck
    /// </summary>
    /// <param name="newCard">The card to add to the deck</param>
    public void Add(CardData newCard)
    {
        playerDeck.Add(newCard);
    }

    /// <summary>
    /// Removes a card from the deck
    /// </summary>
    /// <param name="toRemove">The card to remove from the deck</param>
    public void Remove(CardData toRemove)
    {
        playerDeck.Remove(toRemove);
    }

    /// <summary>
    /// When a card is updated, adjusts its emotion and intent to the new values
    /// </summary>
    /// <param name="oldCard">The values of the card, before change</param>
    /// <param name="newCard">The new card values</param>
    public void UpdateCard(CardData oldCard, CardData newCard)
    {
        int cardRef = playerDeck.FindIndex(x=> x == oldCard);
        playerDeck[cardRef] = newCard;
    }


    /// <summary>
    /// Pops the top card in the deck
    /// </summary>
    /// <returns></returns>
    public CardData GetTop()
    {
        CardData toReturn = playerDeck[0];
        playerDeck.RemoveAt(0);
        return toReturn;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Shuffle()
    {
        Deck preShuffle = GetCopy();
        int numOfElements = playerDeck.Count;
        int newIndex = 0;
        WipeDeckElements(this);
        for(int i=0; i<numOfElements; i++)
        {
            do
            {
                newIndex = Random.Range(0, numOfElements);

            } while (playerDeck[newIndex].Emotion != CardEmotion.NotSelected);
            playerDeck[newIndex] = preShuffle.playerDeck[newIndex];
        }
    }

    private void WipeDeckElements(Deck deck)
    {
        foreach(CardData c in deck.playerDeck)
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
        foreach(CardData c in playerDeck)
        {
            deckCopy.Add(c);
        }
        return deckCopy;
    }
    #endregion
}
