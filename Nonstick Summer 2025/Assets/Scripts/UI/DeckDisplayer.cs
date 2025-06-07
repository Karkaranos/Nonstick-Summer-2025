using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class DeckDisplayer : MonoBehaviour
{
    private static Deck PlayerDeckRef => GameManager.DeckManagerReference.PlayerDeck;
    private static int MaxDeckDisplaySize => GameManager.MaxCardsVisibleInDeck;

    [SerializeField]private CardData[] _currentlyDisplayed;
    [SerializeField]private List<GameObject> _visualDisplay = new List<GameObject>();

    [SerializeField] private Vector2 _dimensions;
    //[SerializeField] private Vector2 _midpoint;

    [SerializeField] private float _bufferFromEdgeOfRegion = 10f;
    [SerializeField] private GameObject _cardPrefab;

    /// <summary>
    /// This script had to be a monobehavior to get this
    /// </summary>
    private void Awake()
    {
        _dimensions = .5f*GetComponent<RectTransform>().sizeDelta;
    }

    public void DisplayAllCards()
    {
        ClearDisplay();

        Deck copy = PlayerDeckRef.GetCopy();
        Debug.Log(copy.PlayerDeck.Count);
        Vector2[] spawnPositions = new Vector2[copy.PlayerDeck.Count];

        GeneratePositions(ref spawnPositions, 0, copy.PlayerDeck.Count-1);

        SpawnCards(copy.PeekNCards(copy.PlayerDeck.Count), spawnPositions);

    }

    public void DisplayNCards(int n=0)
    {
        ClearDisplay();
        if(n==0)
        {
            n = MaxDeckDisplaySize;
        }

        Deck copy = PlayerDeckRef.GetCopy();
        Vector2[] spawnPositions = new Vector2[n];

        GeneratePositions(ref spawnPositions, 0, n-1);

        SpawnCards(copy.PeekNCards(n), spawnPositions);
    }

    public void ClearDisplay()
    {
        for(int i=0; i<_visualDisplay.Count; i++)
        {
            Destroy(_visualDisplay[i]);
        }
        _visualDisplay.Clear();
    }

    private void GeneratePositions(ref Vector2[] positions, int start, int end)
    {
        Debug.Log(positions.Length + " v " + end);
        positions[start] = new Vector2(GetComponent<RectTransform>().offsetMin.x, (.5f * _dimensions.y));
        positions[end] = new Vector2(transform.position.x+ _dimensions.x-_bufferFromEdgeOfRegion, (.5f * _dimensions.y));
        RecursivelyGeneratePositions(ref positions, start, end);
    }

    private Vector2[] RecursivelyGeneratePositions(ref Vector2[] positions, int start, int end)
    {
        if(start >= end)
        {
            return positions;
        }

        int midpoint = start + (end - start) / 2;
        positions[midpoint] = new Vector2((positions[start].x + positions[end].x) / 2, positions[start].y);

        Vector2[] leftHalf = RecursivelyGeneratePositions(ref positions, start, midpoint-1);
        Vector2[] rightHalf = RecursivelyGeneratePositions(ref positions, midpoint+1, end);

        return StaticUtilities.AddArrays(leftHalf, rightHalf);
    }

    private void SpawnCards(CardData[] cards, Vector2[] position)
    {
        _currentlyDisplayed = cards;
        for(int i=0; i<cards.Length; i++)
        {
            _visualDisplay.Add(Instantiate(_cardPrefab, position[i], Quaternion.identity, transform));
            _visualDisplay[i].GetComponent<CardDisplay>().SetCard(cards[i]);
        }
    }



}
