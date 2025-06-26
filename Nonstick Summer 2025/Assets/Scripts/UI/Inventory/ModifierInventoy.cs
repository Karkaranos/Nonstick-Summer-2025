/*****************************************************************************
* File Name :         ModifierInventoy.cs
* Author :            Toby
* Creation Date :     June 25, 2025
*
* Brief Description : it is what it is
* 
*****************************************************************************/

using UnityEngine;

public class ModifierInventoy : MonoBehaviour
{
    [SerializeField] private DeckDisplayer deckDisplay;
    [SerializeField] private ModifierDeckDisplay modifierDisplay;
    [SerializeField] private ApplyModifierButton applyModifierButton;

    private void Start()
    {
        deckDisplay.SetDeck(ref DeckManager.PlayerDeck);
        // modifier deck is already set B)
    }
}
