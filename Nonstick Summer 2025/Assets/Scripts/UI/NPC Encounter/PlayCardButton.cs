/*****************************************************************************
* File Name :         PlayCardButton.cs
* Author :            Toby
* Creation Date :     8/5/2025 (day before code freeze)
*
* Brief Description : Plays the selected card
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private CanvasGroup group;
    [SerializeField, Required] private CanvasGroup parentGroup;
    private DeckDisplayer hand => DialogueUIController.Instance.deckDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        button.onClick.AddListener(OnButtonPressed);
        UpdateButtonEnabled();

        DialogueManager.OnCardPlayedStarted.AddListener(UpdateButtonEnabled);
        DialogueManager.OnPlayerFinishReadingDialogue.AddListener(UpdateButtonEnabled);
        DialogueManager.OnCardPlayedFinished.AddListener(UpdateButtonEnabled);
        hand.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool isHoldingACard = hand.FirstSelectedCard != null;
        var card = hand.FirstSelectedCard;
        bool canAffordCard = isHoldingACard && (Mathf.Abs(card.EnergyCost) <= DialogueManager.CurrentEnergy);
        Debug.Log($"isHoldingACard: {isHoldingACard}\ncanAffordCard:{canAffordCard}");

        //if(isHoldingACard)
        //    Debug.Log($"{Mathf.Abs(card.EnergyCost)} > {DialogueManager.CurrentEnergy} = {(Mathf.Abs(card.EnergyCost) > DialogueManager.CurrentEnergy)}");

        button.interactable = (DialogueUIController.Instance.inSceneFive || (isHoldingACard && canAffordCard));

        StaticUtilities.ToggleCanvasGroup(group, 
            enabled: isHoldingACard,
            interactable: canAffordCard, 
            alpha: isHoldingACard ? 1: 0, 
            ignoreParentGroups:true);
    }

    private void Update()
    {
        //UpdateButtonEnabled();
    }

    public void OnButtonPressed()
    {
        Debug.Log("Play card button pressed");
        if(DialogueUIController.Instance.selectedCardData != null)
        {

            if(Mathf.Abs(DialogueUIController.Instance.selectedCardData.EnergyCost) > DialogueManager.CurrentEnergy && DialogueUIController.Instance.inSceneFive == false)
            {
                return;
            }

        }
        StartCoroutine(DialogueManager.ProcessPlayCard(DialogueUIController.Instance.selectedCardData));
        UpdateButtonEnabled();
    }
}
