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
    [SerializeField] private ModifierDeckDisplay stampModifierDisplay;
    [SerializeField] private ModifierDeckDisplay stickerModifierDisplay; // bruh these dont even do anything
    [SerializeField] private ApplyModifierButton applyModifierButton;

    private void Start()
    {
        deckDisplay.SetDisplayDeck(ref DeckManager.PlayerFullDeck);
        // modifier deck is already set B)
    }
}
