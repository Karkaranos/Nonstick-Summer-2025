/*****************************************************************************
// File Name :         DeckTest.cs
// Author :            Cade R. Naylor
// Creation Date :     June 3, 2025
//
// Brief Description :  Tests Deck functionality
*****************************************************************************/
using UnityEngine;

public class DeckTest : MonoBehaviour
{
    public CardData[] testVals;
    public static DeckManager Instance => GameManager.DeckManagerReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(CardData c in testVals)
        {
            DeckManager.AddCard(c);
        }

        //print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i=0; i<= Instance.PlayerDeck.PlayerDeck.Count+1; i++)
        {
            CardData top = DeckManager.GetTopCard();
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }

        foreach (CardData c in testVals)
        {
            DeckManager.AddCard(c);
        }

        // Testing for shuffling deck

        
        DeckManager.ShuffleDeck(ref Instance.PlayerDeck);
        print("Shuffled");

        //print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i = 0; i <= Instance.PlayerDeck.PlayerDeck.Count+1; i++)
        {
            CardData top = DeckManager.GetTopCard();
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }

        foreach (CardData c in testVals)
        {
            DeckManager.AddCard(c);
        }

        //TODO: Fix Duplication

        /* 
        print("Adding a duplicate element");
        DeckManager.AddCard(testVals[0]);
        //DeckManager.RemoveCard(testVals[0]);

        print(DeckManager.PlayerDeck.PlayerDeck.Count);

        for (int i = 0; i <= Instance.PlayerDeck.PlayerDeck.Count+1; i++)
        {
            CardData top = DeckManager.GetTopCard(Instance.PlayerDeck);
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }*/


        // Testing removal from deck

        /*
        print("Removing an element");
        DeckManager.RemoveCard(testVals[0]);

        //print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i = 0; i <= Instance.PlayerDeck.PlayerDeck.Count + 1; i++)
        {
            CardData top = DeckManager.GetTopCard(Instance.PlayerDeck);
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }
        */


    }
}
