/*************************************************
* Author Names :          Toby, (Cade Naylor Ghostwriter)
* Date Created :          June 21, 2025
* Date Modified :         June 21, 2025
* Brief Description :     Displays the modifier cards in the players inventory.
*   If this script needs to be refactored to be able to show sets of modifiers
*   aside from the player's, that is fine, but it doesn't do that rn.
*   
*   low key just stole cades homework with this one, see DeckDisplayer.cs for original.
*   did simplify it a lil tho, bc modifiers are far less complex than dialogue cards.
*   
*   TODO: 
*       vertical mode?
*       Combine with DeckDisplay thru inheritence? maaaaybeee????
*   
***************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

// This script needed to be a Monobehavior to get some of the references needed
public class ModifierDeckDisplay : MonoBehaviour
{
    #region Display

    #region Variables

    [SerializeField, Required]
    private RectTransform cardArea;
    [SerializeField, Tooltip("Adjusts horizontal space between cards and edge of display")]
    private float _bufferFromEdgeOfRegion = 10;
    [SerializeField, Tooltip("A reference to the visual Card Prefab")] 
    private GameObject modifierCardPrefab;
    [SerializeField, Tooltip("Point to spawn cards from")]
    private Vector2 spawnCardsPosition = new Vector2(2400, 1350); // screen dimensions * 1.25

    [HideInInspector]
    public UnityEvent OnSelectedChanged=new UnityEvent();

    private IReadOnlyCollection<ModifierData> playerModifiers => ModifierManager.ModifierCollection; // changed to be generalized, because deck will not always be the players.

    private List<ModifierCardDisplay> _visualDisplays = new List<ModifierCardDisplay>();

    #endregion

    #region Functions

    private void Start()
    {
        DisplayAllCards();
    }

    /// <summary>
    /// Displays all cards in the player's deck
    /// </summary>
    public void DisplayAllCards(bool fullReset = false)
    {
        if(fullReset)
            ClearDisplay();

        if (_visualDisplays == null)
            _visualDisplays = new List<ModifierCardDisplay>();

        // clear modifiers that arent in hand anymore
        var cardsRemovedFromHand = _visualDisplays
            .Where(mod => !playerModifiers.Contains(mod.modifierData));
        for (int i = cardsRemovedFromHand.Count() - 1; i >= 0; i--)
        {
            if (cardsRemovedFromHand.ElementAt(i) == null)
                continue;

            if (cardsRemovedFromHand.ElementAt(i).gameObject != null)
                Destroy(cardsRemovedFromHand.ElementAt(i).gameObject);
        }

        //_visualDisplays = _visualDisplays.Where(disp => disp != null).ToList();

        // TODO: do this better. im running out of options
        for (int i = _visualDisplays.Count() - 1; i >= 0; i--)
        {
            if (_visualDisplays[i] == null || _visualDisplays[i].gameObject == null)
                _visualDisplays.RemoveAt(i);
        }

        if (playerModifiers.Count == 0)
            return;

        SpawnNewCards();

        // Generates spawn positions
        GenerateAndSetPositions();
    }

    /// <summary>
    /// Clears all currently displayed cards
    /// </summary>
    public void ClearDisplay()
    {
        for (int i = 0; i < _visualDisplays.Count; i++)
        {
            Destroy(_visualDisplays[i].gameObject);
        }
        _visualDisplays.Clear();
    }

    private void SpawnNewCards()
    {
        // Cards that havent been instantiated yet
        var newCards = playerModifiers.
            Where(mod => 
                _visualDisplays.Select(display=>display.modifierData) // what the lambda
                .Contains(mod) == false); 
        // programming equivalent of doing an awesome skateboard trick ^

        foreach(var newCard in newCards)
        {
            var newCardGameObj = Instantiate(modifierCardPrefab, this.transform);
            var display = newCardGameObj.GetComponent<ModifierCardDisplay>();
            //display.OnMouseDown.AddListener(OnCardClicked); // TODO why does this not work D:
            //display.mouseInteraction.OnMouseDown.AddListener(() => OnCardClicked(display));
            display.SetCard(newCard);
            display.SetPositionAndOffsetNoAnimation(position:spawnCardsPosition, offset:Vector2.zero);
            _visualDisplays.Add(display);

        }
    }

    /// <summary>
    /// Generates the positions cards will spawn at
    /// </summary>
    /// <param name="positions">Vector2 array of spawn positions, passed by reference</param>
    /// <param name="start">The starting index</param>
    /// <param name="end">The ending index</param>
    private void GenerateAndSetPositions()
    {
        float left = cardArea.rect.xMin + _bufferFromEdgeOfRegion;
        float right = cardArea.rect.xMax - _bufferFromEdgeOfRegion;

        Debug.Log($"left {left}, right {right}");

        //TODO sort cards somehow

        for(int i=0; i<_visualDisplays.Count; i++)
        {
            var modifier = _visualDisplays[i];

            float t;
            if (_visualDisplays.Count <= 1)
                t = 0.5f; // halfway through to avoid dividing by 0
            else
                t = (float)i / (_visualDisplays.Count - 1);

            float x = Mathf.Lerp(left, right, t);
            modifier.SetPositionAndOffset(position:new Vector2(x,0), offset:Vector2.zero, speed:5000);

            //modifier.OnMouseDown.AddListener(OnCardClicked);
        }

        // Assigns the last position to the right side of the display area, as a percaution
        // also yeah the numbers are weird. I will fix it later. i'm a lil tired tbh
        //positions[end] = new Vector2(rectTransformCenter.x + .5f * _dimensions.x + .3f * _cardWidth - _bufferFromEdgeOfRegion, 150);
    }

    #endregion

    #endregion

    #region Selection Input

    #region Variables

    [Header("Card Selection")]

    [Tooltip("Add this number to the cards position when a card is selected")]
    [SerializeField] private Vector2 selectedCardOffset = new Vector2(0, 50);

    // tobys first HashSet in Unity! 6/21/2025
    [HideInInspector]
    public ModifierCardDisplay selectedCard { get; private set; }

    #endregion

    #region Functions

    // See SpawnCards
    public void OnCardClicked(ModifierCardDisplay cardDisplay)
    {
        Debug.Log("selected changed");
        if (selectedCard == cardDisplay)
            DeselectCard();
        else
            SelectCard(cardDisplay);

        OnSelectedChanged.Invoke();
    }

    public void SelectCard(ModifierCardDisplay cardDisplay)
    {
        if (DialogueUIController.Instance != null && !DialogueManager.ReadUserInput)
            return;

        if (selectedCard == cardDisplay)
            return;

        Debug.Log("selecting card");

        // swap cards 
        DeselectCard();

        selectedCard = cardDisplay;

        cardDisplay.SetPositionAndOffset(offset: (Vector3) selectedCardOffset);
        cardDisplay.transform.SetAsLastSibling(); // bring to front so player can see it
    }

    public void DeselectCard()
    {
        if (selectedCard == null)
            return;

        selectedCard.ResetOffset();
    }

    #endregion

    #endregion
}
