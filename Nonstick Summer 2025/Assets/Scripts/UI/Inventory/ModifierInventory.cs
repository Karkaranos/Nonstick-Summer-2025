/*****************************************************************************
* File Name :         ModifierInventory.cs
* Author :            Toby
* Creation Date :     June 25, 2025
*
* Brief Description : it is what it is
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine.Events;

public class ModifierInventory : MonoBehaviour
{
    [SerializeField] private DeckDisplayer deckDisplay;
    [SerializeField] public ModifierDeckDisplay[] modifierDisplays;
    [SerializeField] private ApplyModifierButton applyModifierButton;

    public static UnityEvent OnInventoryOpened = new();

    private void Start()
    {
        deckDisplay.SetDisplayDeck(ref DeckManager.PlayerFullDeck);
        // modifier deck is already set B)

        OnInventoryOpened.Invoke();
    }
}
