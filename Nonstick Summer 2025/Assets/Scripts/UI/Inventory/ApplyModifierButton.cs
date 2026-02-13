/*****************************************************************************
* File Name :         ApplyModifierButton.cs
* Author :            Toby
* Creation Date :     June 25, 2025
*
* Brief Description : This script has a fat chance of being deleted later ngl.
* 
* Ties together the mofifier deck and the dialogue deck to apply moddies.
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Linq;

public class ApplyModifierButton : MonoBehaviour
{
    [SerializeField] private DeckDisplayer deckDisplay;
    [SerializeField] private ModifierInventory inventory;
    [SerializeField, Required] private RectTransform inventoryScreen;
    [SerializeField] private CanvasGroup group;

    private ModifierDeckDisplay[] modifierDisplays => inventory.modifierDisplays;
    private Button button;
    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);

        deckDisplay.OnCardsSelectedChanged.AddListener(OnAnyCardSelectedChanged);
        ModifierDeckDisplay.OnSelectedChanged.AddListener(OnAnyCardSelectedChanged);

        OnAnyCardSelectedChanged();
    }

    /// <summary>
    /// When a modifier OR a dialogue card is selected
    /// </summary>
    private void OnAnyCardSelectedChanged()
    {
        var canPlay = CanPlayModifier();
        //button.enabled = canPlay;
        //button.SetColors(normalColor: canPlay? Color.green: Color.gray);
        button.interactable = canPlay;

        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        if (ModifierDeckDisplay.selectedCard != null && ModifierDeckDisplay.selectedCard.MarkedToBeDestroyed == false)
        {
            rectTransform.parent = ModifierDeckDisplay.selectedCard.applyButtonAnchor.parent;
            rectTransform.position = ModifierDeckDisplay.selectedCard.applyButtonAnchor.position;
            rectTransform.sizeDelta = ModifierDeckDisplay.selectedCard.applyButtonAnchor.sizeDelta;
            StaticUtilities.EnableCanvasGroup(group);
        }
        else
        {
            rectTransform.SetParent(inventoryScreen);
            StaticUtilities.DisableCanvasGroup(group);
        }
    }

    private void OnButtonPressed()
    {
        if (!CanPlayModifier())
            return;

        // dip out before the button is destroyed and its too late
        rectTransform.SetParent(inventoryScreen);
        StaticUtilities.DisableCanvasGroup(group);

        // All of my work in the last week in one grand ass line of code...
        ModifierDeckDisplay.selectedCard.modifierData.TryApplyModifier(DeckDisplayer.selectedCards.Select(display => display.cardData).ToArray());
        ModifierDeckDisplay.selectedCard.MarkedToBeDestroyed = true;

        ModifierManager.RemoveCard(ModifierDeckDisplay.selectedCard.modifierData);
        deckDisplay.DisplayAllCards();
        deckDisplay.DeselectAllCards();
        foreach (var modifierDisplay in modifierDisplays)
        {
            modifierDisplay.DisplayAllCards();
        }
        deckDisplay.DisplayAllCards();
    }

    private bool CanPlayModifier()
    {
        DeckDisplayer.selectedCards = DeckDisplayer.selectedCards.Where(d => d != null || !d.MarkedToBeDestroyed).ToHashSet();

        // if the player is biting nothing 
        if (ModifierDeckDisplay.selectedCard == null || !deckDisplay.HasCardsSelected)
            return false;

        // if player is biting off more than they can chew
        if (DeckDisplayer.selectedCards.Count > ModifierDeckDisplay.selectedCard.modifierData.MaxCardsApplied)
            return false;

        // if player is biting off less than they can chew
        if (DeckDisplayer.selectedCards.Count < ModifierDeckDisplay.selectedCard.modifierData.MinCardsApplied)
            return false;

        // if player alreaty bit off
        if (ModifierDeckDisplay.selectedCard.MarkedToBeDestroyed)
            return false;

        // if the player is biting off enough that they should can chew
        var carddatas = DeckDisplayer.selectedCards.Select(display => display.cardData).ToArray();
        return ModifierDeckDisplay.selectedCard.modifierData.CanApplyModifier(carddatas);
    }
}
