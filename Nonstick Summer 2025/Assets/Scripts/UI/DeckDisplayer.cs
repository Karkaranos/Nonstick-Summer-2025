/*************************************************
Author Names :          Cade, Naylor, Toby
Date Created :          June 6, 2025
Date Modified :         June 26, 2025
Brief Description :     Handles visual display for the deck

Inside of you there are two decks: your hand and 
your remaining cards.
You can still display cards without ever setting 
remaining cards, you just gotta be careful.

I might've gone really overboard with the animations, sorry cader :,(

TODO :                  Create functions for easier updating
                        _visualDisplays and displayedData are basically storing the same thing. figure out how to clean that up
***************************************************/
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using NaughtyAttributes;
using static Unity.Cinemachine.CinemachineFreeLookModifier;
using FMOD;

// This script needed to be a Monobehavior to get some of the references needed
[RequireComponent(typeof(CanvasGroup))]
public class DeckDisplayer : MonoBehaviour
{
    #region DISPLAY

    #region Variables

    [SerializeField, Required]
    private RectTransform cardArea;

    [HideInInspector]
    public CanvasGroup canvasGroup;
    private static int DefaultHandSize => GameManager.DefaultCardsInHand;
    private static int MaxHandSize => GameManager.MaxCardsVisibleInDeck;

    public List<CardDisplay> VisualDisplays { get => _visualDisplays; private set => _visualDisplays = value; }

    [SerializeField, Tooltip("A reference to the visual Card Prefab")] private GameObject _cardPrefab;
    [SerializeField, Tooltip("Point to spawn cards from")]
    private Vector2 spawnCardsPosition = new Vector2(2400, 1350); // screen dimensions * 1.25
    [SerializeField, Tooltip("Space between cards, this gap is ignored if there are too many cards")]
    private float spacing = 3.5f;
    [SerializeField]
    private bool animateCardsDestroying = true;

    private Vector2 _dimensions;    // Dimensions of the rectTransform cards will spawn in
    private Vector3 rectTransformCenter;    // Position of the rectTransform, in screen space

    private List<CardDisplay> _visualDisplays = new List<CardDisplay>();
    private Deck displayedData = new Deck();

    private bool interactable = true;

    private float realCardWidth;
    private float desiredWidth;

    #endregion Variables

    #region Functions
    /// <summary>
    /// Called upon the first frame
    /// Gets a reference to the size and position of the transform in appropriate units
    /// </summary>
    private void Awake()
    {
        _dimensions = GetComponent<RectTransform>().sizeDelta;
        realCardWidth = _cardPrefab.GetComponent<RectTransform>().rect.width;
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransformCenter = transform.localPosition;
    }

    public void SetDisplayDeck(ref Deck deckRef, bool displayAll=true)
    {
        // Reset current deck
        if(displayedData != null)
        {
            displayedData.OnDeckChanged.RemoveAllListeners();
        }

        displayedData = deckRef;

        deckRef.OnDeckChanged.AddListener(DisplayAllCards); // this function Can cause changes to the deck, but if it keeps running, it will run out of things to change

        if (displayAll)
            DisplayAllCards();
    }

    /// <summary>
    /// Displays all cards in the player's deck
    /// </summary>
    public void DisplayAllCards()
    {
        StartCoroutine(DisplayAllCardsCoroutine());
    }

    public IEnumerator DisplayAllCardsCoroutine()
    {
        if (_visualDisplays == null)
            _visualDisplays = new List<CardDisplay>();

        yield return ClearRemovedCards();

        if (displayedData.Count == 0)
            yield break;

        SpawnNewCards();

        // Generates spawn positions
        GenerateAndSetPositions();
    }

    private IEnumerator ClearRemovedCards()
    {
        // clear modifiers that arent in hand anymore
        for (int i = _visualDisplays.Count() - 1; i >= 0; i--)
        {
            var display = _visualDisplays[i];
            if (display == null || display.gameObject == null || display.cardData == null)
            {
                _visualDisplays.RemoveAt(i);
                continue;
            }

            if (!displayedData.Contains(display.cardData))
            {
                if(animateCardsDestroying)
                    yield return display.UseCardAnimation(destroyAfter: true);
                else
                    Destroy(display.gameObject);

                _visualDisplays.RemoveAt(i);
                continue;
            }
        }
    }

    private void SpawnNewCards()
    {
        // Cards that havent been instantiated yet
        var newCards = displayedData.Cards.
            Where(card =>
                _visualDisplays.Select(display => display.cardData) // what the lambda
                .Contains(card) == false);
        // programming equivalent of doing an awesome skateboard trick ^

        foreach (var newCard in newCards)
        {
            var newCardGameObj = Instantiate(_cardPrefab, this.transform);
            var display = newCardGameObj.GetComponent<CardDisplay>();
            display.SetCard(newCard);

            display.SetPositionAndOffsetNoAnimation(position: spawnCardsPosition, offset: Vector2.zero);
            _visualDisplays.Add(display);

            display.OnMouseDown.AddListener(OnCardClicked);
        }
    }

    /// <summary>
    /// Yeahh basically just copied DisplayNCards
    /// Draws the hand back to the default size
    /// </summary>
    public void DrawToDefaultHand()
    {
        if(DeckManager.RemainingDeck == null)
        {
            throw new ArgumentNullException("No deck to draw from");
        }

        if(DeckManager.RemainingDeck.Count == 0)
        {
            throw new ArgumentOutOfRangeException("No cards to draw");
        }

        while(displayedData.Count < DefaultHandSize)
        {
            displayedData.Add(DeckManager.RemainingDeck.Pop(), false);
        }
            

        DisplayAllCards();
    }

    /// <summary>
    /// Yeahh basically just copied DisplayNCards
    /// Draws the hand back to the max size
    /// </summary>
    public void DrawToMaxHand()
    {
        if (DeckManager.RemainingDeck == null)
        {
            throw new ArgumentNullException("No deck to draw from");
        }

        while (displayedData.Count < MaxHandSize)
        {
            var card = DeckManager.RemainingDeck.Pop();
            AddCardToHand(card);
        }

        DisplayAllCards();
    }

    /// <summary>
    /// Clears all currently displayed cards
    /// </summary>
    public void ClearDisplay()
    {
        for(int i=0; i<_visualDisplays.Count; i++)
        {
            if (animateCardsDestroying)
                StartCoroutine(_visualDisplays[i].UseCardAnimation(destroyAfter: true));
            else
                Destroy(_visualDisplays[i].gameObject);
        }
        _visualDisplays.Clear();
    }

    /// <summary>
    /// NOTE: does not subtract energy
    /// </summary>
    public void DrawOneCard()
    {
        if (DeckManager.RemainingDeck == null)
        {
            throw new ArgumentNullException("No deck to draw from");
        }

        if (DeckManager.PlayerHand.Count < MaxHandSize && DeckManager.RemainingDeck.Count > 0)
        {
            var card = DeckManager.RemainingDeck.Pop();
            AddCardToHand(card);
        }
        else
        {
            throw new System.Exception("Maximum hand size reached");
        }

    }

    public void AddCardToHand(CardData card)
    {
        displayedData.Add(card, false);
        DisplayAllCards();
    }

    public void DiscardCard(CardData card)
    {
        DeselectAllCards();

        displayedData.Remove(card);

        DisplayAllCards();
    }

    /// <summary>
    /// Generates the positions cards will spawn at
    /// </summary>
    /// <param name="positions">Vector2 array of spawn positions, passed by reference</param>
    /// <param name="start">The starting index</param>
    /// <param name="end">The ending index</param>
    private void GenerateAndSetPositions(int? startIndex = null, int? endIndex = null)
    {
        startIndex = startIndex ?? 0;
        endIndex = endIndex ?? _visualDisplays.Count-1;

        GetDesiredWidth();

        float left = cardArea.rect.center.x - (desiredWidth / 2);
        float right = cardArea.rect.center.x + (desiredWidth / 2);

        //Debug.Log($"left {left}, right {right}");

        //TODO sort cards somehow

        _visualDisplays = _visualDisplays.OrderBy(v=> (int) v.cardData.Emotion).ToList();

        for (int i = startIndex.Value; i <= endIndex.Value; i++)
        {
            var card = _visualDisplays[i];

            float t;
            if (_visualDisplays.Count <= 1)
                t = 0.5f; // halfway through to avoid dividing by 0
            else
                t = (float)i / (_visualDisplays.Count - 1);

            float x = Mathf.Lerp(left, right, t);
            card.SetPositionAndOffset(position: new Vector2(x, 0), offset: interactable ? Vector2.zero : disabledCardOffset, speed: 5000);

            card.transform.SetSiblingIndex(i);
        }
    }

    private float GetDesiredWidth()
    {
        if (_visualDisplays.Count == 1)
            desiredWidth = realCardWidth;
        else
            desiredWidth = ((realCardWidth + spacing) * _visualDisplays.Count) - spacing;

        desiredWidth = Mathf.Min(desiredWidth, cardArea.rect.width);

        return desiredWidth;
    }

    #region Obselete

    /// <summary>
    /// Contains the actual logic for spawning the cards
    /// Adds them to a list for storage
    /// </summary>
    /// <param name="cards">An array of CardData for the cards to create</param>
    /// <param name="position">Where the spawned cards should be located</param>
    [System.Obsolete]
    private void SpawnCards(CardData[] cards, Vector2[] position)
    {
        displayedData.Clear();
        for (int i = 0; i < cards.Length; i++)
        {
            //Debug.Log("spawning card at " + position[i]);
            /* There is probably a better way to do this
             * However, I needed to spawn the card, set its anchor, then adjust the position after setting the anchor
             * so it works for now*/

            var displayGameobject = Instantiate(_cardPrefab, Vector2.zero, Quaternion.identity, transform);
            var cardDisplay = displayGameobject.GetComponent<CardDisplay>();
            _visualDisplays.Add(cardDisplay);

            displayedData.Add(cards[i]);

            cardDisplay.SetPositionAndOffset(position: position[i], offset: Vector2.zero, speed: 5000);

            cardDisplay.SetCard(cards[i]);
            cardDisplay.OnMouseDown.AddListener(OnCardClicked);
        }
    }

    /// <summary>
    /// Displays a specified number of cards from the player's hand
    /// If no number is specified, displays max number of cards visible as stated on GameManager
    /// </summary>
    /// <param name="n">The number of cards to display</param>
    [Obsolete]
    public void DisplayNCards(int n = 0)
    {
        ClearDisplay();

        UnityEngine.Debug.Log("Displaying " + n + " of " + DeckManager.RemainingDeck.Cards.Count + " cards.");

        // If no value was passed in, set the display count to the number from GameManager
        if (n == 0)
        {
            n = DefaultHandSize;
        }

        // Creates referenced array
        Vector2[] spawnPositions = new Vector2[n];

        // Generates spawn positions
        GenerateAndSetPositions(0, n - 1);

        // Spawns the specified number of cards
        SpawnCards(DeckManager.RemainingDeck.PopAndReplaceNCards(n), spawnPositions);
    }

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

    [Tooltip("Add this number to the cards position when cards cant be played")]
    [SerializeField] private Vector2 disabledCardOffset = new Vector2(0, -150);

    [SerializeField, Min(1)]
    private int MaxSelectedCards = 1;

    [Tooltip("If true, swaps the selected card when a different card is selected")]
    [SerializeField, ShowIf(nameof(showSwapSelected))]private bool SwapCardsOnSelection = true;


    [HideInInspector] // use this in other scripts to detect when the user selects cards
    public UnityEvent OnCardsSelectedChanged = new UnityEvent();
    public CardData FirstSelectedCard => selectedCards.Count > 0 ? selectedCards.First().cardData : null;
    public bool HasCardsSelected => selectedCards.Count > 0;

    // tobys first HashSet in Unity! 6/21/2025
    [HideInInspector]
    public HashSet<CardDisplay> selectedCards = new HashSet<CardDisplay>();

    #region Computational Variables

    private bool showSwapSelected => MaxSelectedCards == 1;

    #endregion

    #endregion

    #region Functions

    // See SpawnCards
    private void OnCardClicked(CardDisplay cardDisplay)
    {
        if(selectedCards.Contains(cardDisplay))
            DeselectCard(cardDisplay);
        else
            SelectCard(cardDisplay);
    }

    public void SelectCard(CardDisplay cardDisplay)
    {
        if (DialogueUIController.Instance != null && !DialogueManager.ReadUserInput)
            return;

        if (selectedCards.Contains(cardDisplay))
            return;

        UnityEngine.Debug.Log("selecting card");

        // swap cards if player can only have one 
        if(MaxSelectedCards == 1 && SwapCardsOnSelection)
        {
            DeselectAllCards();
        }

        if(selectedCards.Count >= MaxHandSize)
        {
            UnityEngine.Debug.Log("too many cards selected!");
            return;
        }

        selectedCards.Add(cardDisplay);

        cardDisplay.SetPositionAndOffset( offset: (Vector3)selectedCardOffset );
        cardDisplay.transform.SetAsLastSibling(); // bring to front so player can see it

        OnCardsSelectedChanged.Invoke();
    }
    public void DeselectCard(CardDisplay cardDisplay, bool invokeOnCardsSelectChanged=true)
    {
        if (!selectedCards.Contains(cardDisplay))
            return;

        UnityEngine.Debug.Log("deselect");

        selectedCards.Remove(cardDisplay);

        cardDisplay.ResetOffset();

        if(invokeOnCardsSelectChanged)
           OnCardsSelectedChanged.Invoke();
    }

    public void DeselectAllCards()
    {
        var cards = selectedCards.ToArray();
        foreach (CardDisplay cardDisplay in cards)
            DeselectCard(cardDisplay, false);

        OnCardsSelectedChanged.Invoke();
    }
    
    public void UpdateGroupEnabled(bool enabled)
    {
        interactable = enabled;

        if (canvasGroup == null)
        {
            UnityEngine.Debug.LogWarning("No canvas group???");
            return;
        }

        if(enabled)
        {
            StaticUtilities.EnableCanvasGroup(canvasGroup, alpha: 1);

            foreach (CardDisplay display in _visualDisplays)
            {
                display.SetPositionAndOffset(offset: Vector2.zero);
            }
        }
        else
        {
            StaticUtilities.DisableCanvasGroup(canvasGroup, alpha: 0.5f);

            foreach(CardDisplay display in _visualDisplays)
            {
                display.SetPositionAndOffset(offset: disabledCardOffset);
            }
        }
    }

    #endregion

    #endregion
}
