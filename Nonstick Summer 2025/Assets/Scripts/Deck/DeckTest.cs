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
            Instance.AddCard(c);
        }

        //print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i=0; i<= Instance.PlayerDeck.PlayerDeck.Count+1; i++)
        {
            CardData top = Instance.GetTopCard();
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }

        foreach (CardData c in testVals)
        {
            Instance.AddCard(c);
        }

        // Testing for shuffling deck

        /*
        Instance.ShuffleDeck(ref Instance.PlayerDeck);
        print("Shuffled");

        //print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i = 0; i <= Instance.PlayerDeck.PlayerDeck.Count+1; i++)
        {
            CardData top = Instance.GetTopCard();
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }

        foreach (CardData c in testVals)
        {
            Instance.AddCard(c);
        }*/

        //TODO: Fix Duplication

        /* 
        print("Adding a duplicate element");
        Instance.AddCard(testVals[0]);
        //Instance.RemoveCard(testVals[0]);

        print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i = 0; i <= Instance.PlayerDeck.PlayerDeck.Count+1; i++)
        {
            CardData top = Instance.GetTopCard(Instance.PlayerDeck);
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }*/


        // Testing removal from deck

        print("Removing an element");
        Instance.RemoveCard(testVals[0]);

        //print(Instance.PlayerDeck.PlayerDeck.Count);

        for (int i = 0; i <= Instance.PlayerDeck.PlayerDeck.Count + 1; i++)
        {
            CardData top = Instance.GetTopCard(Instance.PlayerDeck);
            print("Element " + i + ": Emotion-" + top.Emotion + " Intent-" + top.Intention);
        }
        


    }
}
