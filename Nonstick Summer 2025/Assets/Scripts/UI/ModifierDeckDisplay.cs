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

// This script needed to be a Monobehavior to get some of the references needed
public class ModifierDeckDisplay : MonoBehaviour
{
    #region Variables

        #region Display
    private IReadOnlyCollection<ModifierData> playerModifiers => ModifierManager.ModifierCollection; // changed to be generalized, because deck will not always be the players.
    
    public List<GameObject> VisualDisplay { get => _visualDisplay; private set => _visualDisplay = value; }

    [SerializeField, Tooltip("Adjusts horizontal space between cards and edge of display")]
    private float _bufferFromEdgeOfRegion = 10;
    [SerializeField, Tooltip("A reference to the visual Card Prefab")] private GameObject modifierCardPrefab;

    private Vector2 _dimensions;    // Dimensions of the rectTransform cards will spawn in
    private Vector3 rectTransformCenter;    // Position of the rectTransform, in screen space
    private float _cardWidth;

    private List<GameObject> _visualDisplay = new List<GameObject>();
    private Vector2[] spawnPositions;
    private List<ModifierData> displayedData = new List<ModifierData>();

    #endregion

    #region Input


    #endregion


    #endregion Variables



    #region Display

    /// <summary>
    /// Called upon the first frame
    /// Gets a reference to the size and position of the transform in appropriate units
    /// </summary>
    private void Awake()
    {
        _dimensions = GetComponent<RectTransform>().sizeDelta;
        _cardWidth = modifierCardPrefab.transform.GetComponent<RectTransform>().sizeDelta.x;
        _dimensions.x -= _cardWidth;

        /*GameObject temp = Instantiate(modifierCardPrefab);
        _cardWidth = temp.transform.GetComponent<RectTransform>().sizeDelta.x;
        _dimensions.x -= _cardWidth;
        Destroy(temp);*/

        rectTransformCenter = transform.localPosition;
    }

    /// <summary>
    /// Displays all cards in the player's deck
    /// </summary>
    public void DisplayAllCards()
    {
        ClearDisplay();


        // Creates referenced array
        spawnPositions = new Vector2[playerModifiers.Count];

        // Generates spawn positions
        GeneratePositions(ref spawnPositions, 0, playerModifiers.Count - 1);

        // Spawns all cards
        SpawnCards(spawnPositions);

    }

    /// <summary>
    /// Clears all currently displayed cards
    /// </summary>
    public void ClearDisplay()
    {
        for (int i = 0; i < VisualDisplay.Count; i++)
        {
            Destroy(VisualDisplay[i]);
        }
        VisualDisplay.Clear();
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
        positions[start] = new Vector2(_bufferFromEdgeOfRegion - .5f * _dimensions.x + rectTransformCenter.x, 150);

        // Calculate the space needed
        float additiveValue = (_dimensions.x - _bufferFromEdgeOfRegion) / (end - start);

        // Position generation
        for (int i = start + 1; i < end; i++)
        {
            positions[i] = positions[i - 1];
            positions[i].x += additiveValue;
        }

        // Assigns the last position to the right side of the display area, as a percaution
        // also yeah the numbers are weird. I will fix it later. i'm a lil tired tbh
        positions[end] = new Vector2(rectTransformCenter.x + .5f * _dimensions.x + .3f * _cardWidth - _bufferFromEdgeOfRegion, 150);
    }

    /// <summary>
    /// Contains the actual logic for spawning the cards
    /// Adds them to a list for storage
    /// </summary>
    /// <param name="cards">An array of CardData for the cards to create</param>
    /// <param name="position">Where the spawned cards should be located</param>
    private void SpawnCards(Vector2[] positions)
    {
        displayedData.Clear();
        for (int i = 0; i < playerModifiers.Count; i++)
        {
            //Debug.Log("spawning card at " + position[i]);
            /* There is probably a better way to do this
             * However, I needed to spawn the card, set its anchor, then adjust the position after setting the anchor
             * so it works for now*/

            VisualDisplay.Add(Instantiate(modifierCardPrefab, Vector2.zero, Quaternion.identity, transform));
            VisualDisplay[i].GetComponent<ModifierCardDisplay>().SetModifier(playerModifiers.ElementAt(i));
            VisualDisplay[i].transform.localPosition = positions[i];
            displayedData.Add(playerModifiers.ElementAt(i));
        }
    }

    #endregion

    #region Player Input

    // card click handling See SpawnCards for event references

    private void OnModifierClicked(ModifierData modifier)
    {

    }

    private void SelectModifier(ModifierData modifier)
    {

    }

    private void DeselectModifier(ModifierData modifier)
    {

    }

    #endregion
}
