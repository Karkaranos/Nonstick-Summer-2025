/*************************************************
Author Names :          Cade, Naylor, Toby
Date Created :          June 6, 2025
Date Modified :         June 10, 2025
Brief Description :     Handles visual display for the deck

TODO :                  Create functions for easier updating
***************************************************/
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using NaughtyAttributes;

// This script needed to be a Monobehavior to get some of the references needed
public class DeckDisplayer : MonoBehaviour
{
    #region DISPLAY

    #region Variables
    //private static Deck PlayerDeckRef => GameManager.DeckManagerReference.PlayerDeck;
    [SerializeField] private Deck DeckRef; // changed to be generalized, because deck will not always be the players.
    private static int DefaultHandSize => GameManager.DefaultCardsInHand;
    private static int MaxHandSize => GameManager.MaxCardsVisibleInDeck;

    public List<GameObject> VisualDisplay { get => _visualDisplay; private set => _visualDisplay = value; }

    [SerializeField, Tooltip("Adjusts horizontal space between cards and edge of display")]
    private float _bufferFromEdgeOfRegion = 10;
    [SerializeField, Tooltip("A reference to the visual Card Prefab")] private GameObject _cardPrefab;

    private Vector2 _dimensions;    // Dimensions of the rectTransform cards will spawn in
    private Vector3 rectTransformCenter;    // Position of the rectTransform, in screen space
    private float _cardWidth;

    private List<GameObject> _visualDisplay = new List<GameObject>();

    private Vector2[] spawnPositions;

    private List<CardData> displayedData = new List<CardData>();

    #endregion Variables

    #region Functions
    /// <summary>
    /// Called upon the first frame
    /// Gets a reference to the size and position of the transform in appropriate units
    /// </summary>
    private void Awake()
    {
        _dimensions = GetComponent<RectTransform>().sizeDelta;
        GameObject temp = Instantiate(_cardPrefab);
        _cardWidth = temp.transform.GetComponent<RectTransform>().sizeDelta.x;
        _dimensions.x -= _cardWidth;
        Destroy(temp);
        rectTransformCenter = transform.localPosition;
    }

    public void SetDeck(ref Deck deckRef)
    {
        DeckRef = deckRef;
        //This will eventually be moved elsewhere
        DrawToDefaultHand();
    }

    /// <summary>
    /// Displays all cards in the player's deck
    /// </summary>
    public void DisplayAllCards()
    {
        ClearDisplay();


        // Creates referenced array
        spawnPositions = new Vector2[DeckRef.Cards.Count];

        // Generates spawn positions
        GeneratePositions(ref spawnPositions, 0, DeckRef.Cards.Count-1);

        // Spawns all cards
        SpawnCards(DeckRef.PopAndReplaceNCards(DeckRef.Cards.Count), spawnPositions);

    }

    /// <summary>
    /// Yeahh basically just copied DisplayNCards
    /// Draws the hand back to the default size
    /// </summary>
    public void DrawToDefaultHand()
    {
        ClearDisplay();

        int n = DefaultHandSize;

        // Creates referenced array
        Vector2[] spawnPositions = new Vector2[n];

        // Generates spawn positions
        GeneratePositions(ref spawnPositions, 0, n - 1);

        // Spawns the specified number of cards
        SpawnCards(DeckRef.PopAndReplaceNCards(n), spawnPositions);
    }

    /// <summary>
    /// Yeahh basically just copied DisplayNCards
    /// Draws the hand back to the max size
    /// </summary>
    public void DrawToMaxHand()
    {
        ClearDisplay();

        int n = MaxHandSize;

        // Creates referenced array
        Vector2[] spawnPositions = new Vector2[n];

        // Generates spawn positions
        GeneratePositions(ref spawnPositions, 0, n - 1);

        // Spawns the specified number of cards
        SpawnCards(DeckRef.PopAndReplaceNCards(n), spawnPositions);
    }


    /// <summary>
    /// Displays a specified number of cards from the player's hand
    /// If no number is specified, displays max number of cards visible as stated on GameManager
    /// </summary>
    /// <param name="n">The number of cards to display</param>
    public void DisplayNCards(int n=0)
    {
        ClearDisplay();

        Debug.Log("Displaying " + n + " of " + DeckRef.Cards.Count + " cards.");

        // If no value was passed in, set the display count to the number from GameManager
        if(n==0)
        {
            n = DefaultHandSize;
        }


        // Creates referenced array
        Vector2[] spawnPositions = new Vector2[n];


        // Generates spawn positions
        GeneratePositions(ref spawnPositions, 0, n-1);

        // Spawns the specified number of cards
        SpawnCards(DeckRef.PopAndReplaceNCards(n), spawnPositions);
    }

    /// <summary>
    /// Clears all currently displayed cards
    /// </summary>
    public void ClearDisplay()
    {
        for(int i=0; i<VisualDisplay.Count; i++)
        {
            Destroy(VisualDisplay[i]);
        }
        VisualDisplay.Clear();
    }


    public void DrawOneCard()
    {
        if (displayedData.Count < MaxHandSize)
        {
            displayedData.Add(DeckRef.GetNextCardCopy());

            ClearDisplay();

            // Creates referenced array
            Vector2[] spawnPositions = new Vector2[displayedData.Count];

            // Generates spawn positions
            GeneratePositions(ref spawnPositions, 0, displayedData.Count - 1);

            // Spawns the specified number of cards
            SpawnCards(StaticUtilities.ListToArray(displayedData), spawnPositions);
        }
        else
        {
            throw new System.Exception("Maximum hand size reached");
        }

    }

    public void DiscardCard(CardData card)
    {
        displayedData.Remove(card);

        ClearDisplay();

        // Creates referenced array
        Vector2[] spawnPositions = new Vector2[displayedData.Count];

        // Generates spawn positions
        GeneratePositions(ref spawnPositions, 0, displayedData.Count - 1);

        // Spawns the specified number of cards
        SpawnCards(StaticUtilities.ListToArray(displayedData), spawnPositions);
    }

    /// <summary>
    /// Generates the positions cards will spawn at
    /// </summary>
    /// <param name="positions">Vector2 array of spawn positions, passed by reference</param>
    /// <param name="start">The starting index</param>
    /// <param name="end">The ending index</param>
    private void GeneratePositions(ref Vector2[] positions, int start, int end)
    {
        print("Ran");
        // Assign the first position to the left side of the display area
        positions[start] =  new Vector2(_bufferFromEdgeOfRegion - .5f *_dimensions.x + rectTransformCenter.x,150);

        // Calculate the space needed
        float additiveValue = (_dimensions.x - _bufferFromEdgeOfRegion) / (end-start);

        // Position generation
        for(int i=start+1; i<end; i++)
        {
            positions[i] = positions[i - 1];
            positions[i].x += additiveValue;
        }

        // Assigns the last position to the right side of the display area, as a percaution
        // also yeah the numbers are weird. I will fix it later. i'm a lil tired tbh
        positions[end] =    new Vector2(rectTransformCenter.x + .5f *_dimensions.x + .3f * _cardWidth -_bufferFromEdgeOfRegion, 150);
    }

    /// <summary>
    /// Contains the actual logic for spawning the cards
    /// Adds them to a list for storage
    /// </summary>
    /// <param name="cards">An array of CardData for the cards to create</param>
    /// <param name="position">Where the spawned cards should be located</param>
    private void SpawnCards(CardData[] cards, Vector2[] position)
    {
        displayedData.Clear();
        for(int i=0; i<cards.Length; i++)
        {
            //Debug.Log("spawning card at " + position[i]);
            /* There is probably a better way to do this
             * However, I needed to spawn the card, set its anchor, then adjust the position after setting the anchor
             * so it works for now*/
            
            VisualDisplay.Add(Instantiate(_cardPrefab, Vector2.zero, Quaternion.identity, transform));
            //_visualDisplay[i].GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            var cardDisplay = VisualDisplay[i].GetComponent<CardDisplay>();
            cardDisplay.SetCard(cards[i]);
            VisualDisplay[i].transform.localPosition = position[i];
            displayedData.Add(cards[i]);

            cardDisplay.OnMouseDown.AddListener(OnCardClicked);
        }
    }

    #region Obselete
    // ok so I was being a little bit of a dumbass trying to get this to work
    // will probably revisit it later for efficiency 
    // actually nevermind the entire thing is more complex
    /// <summary>
    /// Recursive function to generate card positions
    /// </summary>
    /// <param name="positions">Vector2 array containing all spawn positions</param>
    /// <param name="start">starting index</param>
    /// <param name="end">ending index</param>
    /// <returns>Returns an array of partial positions</returns>
    private Vector2[] RecursivelyGeneratePositions(ref Vector2[] positions, int start, int end)
    {
        int midpoint = start + (end - start) / 2;
        if (start >= end || midpoint == 0 || midpoint == positions.Length - 1)
        {
            return positions;
        }
        positions[midpoint] = new Vector2((positions[start].x + positions[end].x) / 2, (.5f * _dimensions.y));

        Vector2[] leftHalf = RecursivelyGeneratePositions(ref positions, start, midpoint);
        Vector2[] rightHalf = RecursivelyGeneratePositions(ref positions, midpoint + 1, end);

        return StaticUtilities.AddArrays(leftHalf, rightHalf);
    }
    #endregion

    #endregion

    #endregion

    #region Selection
    // could be moved to partial class?

    #region Variables

    [Header("Card Selection")]

    [Tooltip("Add this number to the cards position when a card is selected")]
    [SerializeField] private Vector2 selectedCardOffset = new Vector2(0, 50);

    [SerializeField, Min(1)]
    private int MaxSelectedCards = 1;

    [Tooltip("If true, swaps the selected card when a different card is selected")]
    [SerializeField, ShowIf(nameof(showSwapSelected))]private bool SwapCardsOnSelection = true;


    [HideInInspector] // use this in other scripts to detect when the user selects cards
    public UnityEvent OnCardsSelectedChanged { get; set; }
    public CardData FirstSelectedCard => selectedCards.Count > 0 ? selectedCards.First().cardData : null;

    // tobys first HashSet in Unity! 6/21/2025
    [HideInInspector]
    public HashSet<CardDisplay> selectedCards = new HashSet<CardDisplay>();

    #region Computational Variables

    private int finishedDeselects;

    private bool showSwapSelected => MaxSelectedCards == 1;

    #endregion

    #endregion

    #region Functions

    // See SpawnCards
    private void OnCardClicked(CardDisplay cardDisplay)
    {
        if(selectedCards.Contains(cardDisplay))
            StartCoroutine(DeselectCard(cardDisplay));
        else
            StartCoroutine(SelectCard(cardDisplay));
    }

    private IEnumerator SelectCard(CardDisplay cardDisplay)
    {
        //if (DialogueUIController.Instance != null && !DialogueManager.ReadUserInput)
        //    yield break;

        if (selectedCards.Contains(cardDisplay))
            yield break;

        Debug.Log("selecting card");

        // swap cards if player can only have one 
        if(MaxSelectedCards == 1 && SwapCardsOnSelection)
        {
            yield return DeselectAllCards();
        }

        Debug.Log(selectedCards.Count);

        if(selectedCards.Count >= MaxHandSize)
        {
            Debug.Log("too many cards selected!");
            yield break;
        }

        selectedCards.Add(cardDisplay);

        cardDisplay.transform.position += (Vector3) selectedCardOffset;
        cardDisplay.transform.SetAsFirstSibling(); // bring to front so player can see it
    }
    private IEnumerator DeselectCard(CardDisplay cardDisplay)
    {
        if (!selectedCards.Contains(cardDisplay))
            yield break;

        yield return null;

        Debug.Log("deselect");

        selectedCards.Remove(cardDisplay);

        // TODO: animate this
        cardDisplay.transform.position -= (Vector3)selectedCardOffset;
    }

    public IEnumerator DeselectAllCards()
    {
        Debug.Log("deselect all cards");

        finishedDeselects = 0;
        foreach (CardDisplay cardDisplay in selectedCards)
            // Starts all deselect coroutines at the same time, so we cant do an await here
            StartCoroutine(DeselectSingleCardBulk(cardDisplay));

        // Tobys first WaitUntil! 6/22/2025
        yield return new WaitUntil(() => finishedDeselects == selectedCards.Count);
    }

    private IEnumerator DeselectSingleCardBulk(CardDisplay cardDisplay)
    {
        yield return DeselectCard(cardDisplay);
        finishedDeselects++;
    }

    #endregion

    #endregion
}
