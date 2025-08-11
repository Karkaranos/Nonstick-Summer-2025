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
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

// This script needed to be a Monobehavior to get some of the references needed
public class ModifierDeckDisplay : MonoBehaviour
{
    #region Display

    #region Variables

    [SerializeField, Required]
    private RectTransform cardArea;
    [SerializeField, Tooltip("A reference to the visual Card Prefab")]
    private GameObject modifierCardPrefab;
    [SerializeField, Tooltip("Point to spawn cards from")]
    private Vector2 spawnCardsPosition = new Vector2(2400, 1350); // screen dimensions * 1.25
    [SerializeField, Tooltip("Space between cards, this gap is ignored if there are too many cards")]
    private float spacing = 3.5f;
    [SerializeField]
    private ModifierType[] typeFilter;
    [SerializeField]
    private bool animateCardsDestroying = true;

    [HideInInspector]
    public static UnityEvent OnSelectedChanged = new UnityEvent();

    private IEnumerable<ModifierData> filteredPlayerModifiers =>
        ModifierManager.ModifierCollection.Where(m =>
            typeFilter.Contains(m.ModifierType) ||
            typeFilter.Count() == 0 || 
            m.ModifierType == ModifierType.None)
        .ToArray();

    private List<ModifierCardDisplay> _visualDisplays = new List<ModifierCardDisplay>();

    private float realCardWidth;
    private float desiredWidth;

    #endregion

    #region Functions

    private void Start()
    {
        realCardWidth = modifierCardPrefab.GetComponent<RectTransform>().rect.width;
        DisplayAllCards();
        selectedCard = null; // clear from last time inventory was opened.
    }

    /// <summary>
    /// Displays all cards in the player's deck
    /// </summary>
    public void DisplayAllCards()
    {
        StartCoroutine(DisplayAllCardsCoroutine());
    }

    private IEnumerator DisplayAllCardsCoroutine(bool fullReset = false)
    {
        if(fullReset)
            ClearDisplay();

        if (_visualDisplays == null)
            _visualDisplays = new List<ModifierCardDisplay>();

        yield return ClearRemovedCards();

        if (filteredPlayerModifiers.Count() == 0)
            yield break;

        SpawnNewCards();

        // TODO: sort a little better? like, a different type of sort
        // Sort by name, and then by type. Mid solution, imo (especially because it does two sorts),
        // but it Does group moddies by what they are
        _visualDisplays = _visualDisplays
            .OrderBy(d => d.modifierData.name)
            .OrderBy(d => d.modifierData.GetType().ToSafeString())
            .ToList();

        // Generates spawn positions
        GenerateAndSetPositions();

        Debug.Log($"{_visualDisplays.Count} modifier displays, {filteredPlayerModifiers.Count()} modifiers in player inventory");
    }

    private IEnumerator ClearRemovedCards()
    {
        // clear modifiers that arent in hand anymore
        for (int i = _visualDisplays.Count() - 1; i >= 0; i--)
        {
            var display = _visualDisplays[i];
            if (display == null || display.gameObject == null)
            {
                _visualDisplays.RemoveAt(i);
                continue;
            }

            if (!filteredPlayerModifiers.Contains(display.modifierData) || display.modifierData == null)
            {
                if (animateCardsDestroying)
                    yield return display.UseCardAnimation(destroyAfter: true);
                else
                    Destroy(display.gameObject);

                _visualDisplays.RemoveAt(i);
                continue;
            }
        }
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
        var newCards = filteredPlayerModifiers.
            Where(mod => 
                _visualDisplays.Select(display=>display.modifierData) // what the lambda
                .Contains(mod) == false); 
        // programming equivalent of doing an awesome skateboard trick ^

        foreach(var newCard in newCards)
        {
            if (newCard == null)
                continue;

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
        GetDesiredWidth();

        float left = cardArea.rect.center.x - (desiredWidth/2);
        float right = cardArea.rect.center.x + (desiredWidth/2);

        Debug.Log($"left {left}, right {right}");

        //TODO sort cards somehow

        for(int i=0; i<_visualDisplays.Count; i++)
        {
            var modifier = _visualDisplays[i];

            float t;
            if (_visualDisplays.Count <= 1)
                t = 0.5f; // halfway through to avoid dividing by 0
            else
                t = (float)(i) / (_visualDisplays.Count - 1); 

            float x = Mathf.Lerp(left, right, t);
            modifier.SetPositionAndOffset(position:new Vector2(x,0), offset:Vector2.zero, speed:5000);

            modifier.transform.SetSiblingIndex(i);
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

    #endregion

    #endregion

    #region Selection Input

    #region Variables

    [Header("Card Selection")]

    [Tooltip("Add this number to the cards position when a card is selected")]
    [SerializeField] private Vector2 selectedCardOffset = new Vector2(0, 50);

    // tobys first HashSet in Unity! 6/21/2025
    // ???? where????? dumbass
    [HideInInspector]
    public static ModifierCardDisplay selectedCard { get; private set; }

    #endregion

    #region Functions

    // See SpawnCards
    public void OnCardClicked(ModifierCardDisplay cardDisplay)
    {
        if(! filteredPlayerModifiers.Contains(cardDisplay.modifierData))
        {
            //Debug.LogError($"{cardDisplay.modifierData.name} is not in {gameObject.name}!"); 
            return;
        }

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

        //Debug.Log("selecting card");

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
        selectedCard = null;
    }

    #endregion

    #endregion
}
