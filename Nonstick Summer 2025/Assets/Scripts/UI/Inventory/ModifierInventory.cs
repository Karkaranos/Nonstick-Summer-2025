/*****************************************************************************
* File Name :         ModifierInventory.cs
* Author :            Toby
* Creation Date :     June 25, 2025
*
* Brief Description : it is what it is
* 
*****************************************************************************/

using UnityEngine;

public class ModifierInventory : MonoBehaviour
{
    [SerializeField] private DeckDisplayer deckDisplay;
    [SerializeField] public ModifierDeckDisplay[] modifierDisplays;
    [SerializeField] private ApplyModifierButton applyModifierButton;

    private void Start()
    {
        deckDisplay.SetDisplayDeck(ref DeckManager.PlayerFullDeck);
        applyModifierButton.Initialize();
        // modifier deck is already set B)
    }
}
