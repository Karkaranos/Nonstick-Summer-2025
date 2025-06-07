using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class DeckDisplayer : MonoBehaviour
{
    private static Deck PlayerDeckRef => GameManager.DeckManagerReference.PlayerDeck;
    private static int MaxDeckDisplaySize => GameManager.MaxCardsVisibleInDeck;

    private CardData[] _currentlyDisplayed;
    private List<GameObject> _visualDisplay = new List<GameObject>();
    [SerializeField] private Vector2[] spawnPositions;

    private Vector2 _dimensions;
    private Vector3 rectTransformCenter;
    //[SerializeField] private Vector2 _midpoint;

    [SerializeField] private float _bufferFromEdgeOfRegion = 10;
    [SerializeField] private GameObject _cardPrefab;

    private float scalar;

    /// <summary>
    /// This script had to be a monobehavior to get this
    /// </summary>
    private void Awake()
    {
        _dimensions = GetComponent<RectTransform>().sizeDelta;
        _dimensions.x -= 300;   // okay i know magic numbers are bad. this number made things work
        rectTransformCenter = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void DisplayAllCards()
    {
        ClearDisplay();

        Deck copy = PlayerDeckRef.GetCopy();
        Debug.Log(copy.PlayerDeck.Count);
        spawnPositions = new Vector2[copy.PlayerDeck.Count];

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
        spawnPositions = new Vector2[n];

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
        positions[start] =  new Vector2(rectTransformCenter.x - .5f * _dimensions.x + _bufferFromEdgeOfRegion, (.5f * _dimensions.y));
        positions[end] =    new Vector2(rectTransformCenter.x + .5f * _dimensions.x - _bufferFromEdgeOfRegion, (.5f * _dimensions.y));
        RecursivelyGeneratePositions(ref positions, start, end);
    }

    private Vector2[] RecursivelyGeneratePositions(ref Vector2[] positions, int start, int end)
    {


        int midpoint = start + (end - start) / 2;
        if (start >= end || midpoint == 0 || midpoint == positions.Length-1)
        {
            return positions;
        }
        positions[midpoint] = new Vector2((positions[start].x + positions[end].x) / 2, (.5f * _dimensions.y));

        Vector2[] leftHalf = RecursivelyGeneratePositions(ref positions, start, midpoint);
        Vector2[] rightHalf = RecursivelyGeneratePositions(ref positions, midpoint+1, end);

        return StaticUtilities.AddArrays(leftHalf, rightHalf);
    }

    private void SpawnCards(CardData[] cards, Vector2[] position)
    {
        _currentlyDisplayed = cards;
        for(int i=0; i<cards.Length; i++)
        {
            _visualDisplay.Add(Instantiate(_cardPrefab, Vector2.zero, Quaternion.identity, transform));
            _visualDisplay[i].GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            _visualDisplay[i].transform.localPosition = position[i];
            _visualDisplay[i].GetComponent<CardDisplay>().SetCard(cards[i]);
        }
    }



}
