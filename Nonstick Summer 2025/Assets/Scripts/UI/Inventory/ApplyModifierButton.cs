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
    [SerializeField] private ModifierDeckDisplay modifierDisplay;

    private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);

        deckDisplay.OnCardsSelectedChanged.AddListener(OnAnyCardSelectedChanged);
        modifierDisplay.OnSelectedChanged.AddListener(OnAnyCardSelectedChanged);

        OnAnyCardSelectedChanged();
    }

    /// <summary>
    /// When a modifier OR a dialogue card is selected
    /// </summary>
    private void OnAnyCardSelectedChanged()
    {
        var canPlay = CanPlayModifier();
        //button.enabled = canPlay;
        button.SetColors(normalColor: canPlay? Color.green: Color.gray);
    }

    private void OnButtonPressed()
    {
        if (!CanPlayModifier())
            return;

        // All of my work in the last week in one grand ass line of code...
        modifierDisplay.selectedCard.modifierData.TryApplyModifier(deckDisplay.selectedCards.Select(display=>display.cardData).ToArray());

        ModifierManager.RemoveCard(modifierDisplay.selectedCard.modifierData);
        modifierDisplay.DisplayAllCards();
        deckDisplay.DeselectAllCards();
        deckDisplay.DisplayAllCards();
    }

    private bool CanPlayModifier()
    {
        // if the player is biting nothing
        if (modifierDisplay.selectedCard == null || !deckDisplay.HasCardsSelected)
            return false;

        var carddatas = deckDisplay.selectedCards.Select(display => display.cardData).ToArray();


        
        // if player is biting off more than they can chew
        if (deckDisplay.selectedCards.Count > modifierDisplay.selectedCard.modifierData.MaxCardsApplied)
            return false;
        return modifierDisplay.selectedCard.modifierData.CanApplyModifier(carddatas);
        /*        return modifierDisplay.selectedCard.modifierData.CanApplyModifier(carddatas);
        // if player is biting off less than they should chew
        if (deckDisplay.selectedCards.Count < modifierDisplay.selectedCard.modifierData.MinCardsApplied)
            return false;*/

        return true;
    }
}
