/*****************************************************************************
* File Name :         DrawButton.cs
* Author :            Toby
* Creation Date :     June 29, 2025
*
* Brief Description : The Draw button during NPC combat. Gives the player a card
* when pressed.
* 
*****************************************************************************/

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private TMP_Text energyCostDisplay;
    private DeckDisplayer handDisplay => DialogueUIController.Instance.DeckDisplay;
    [ReadOnly]
    public bool CantDrawAnymore = false;
    private int drawCounter = 0;
    public int MaxDrawTimes = 1; //lets design change draws per turn if needed.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        energyCostDisplay.text = "-" + DialogueManager.DrawButtonEnergyCost;

        button.onClick.AddListener(OnButtonPressed);
        CantDrawAnymore = false;
        UpdateButtonEnabled();

        DialogueManager.OnCardPlayedStarted.AddListener(UpdateButtonEnabled);
        handDisplay.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled); // idk it just feels right
        //DialogueUIController.Instance.playCardButton.onClick.AddListener(UpdateButtonEnabled);
        DialogueManager.OnPlayerFinishReadingDialogue.AddListener(UpdateButtonEnabled);
        DeckManager.PlayerHand.OnDeckChanged.AddListener(UpdateButtonEnabled);

        DialogueManager.OnCardPlayedFinished.AddListener(OnPlayerPlayedCardFinish);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool enabled = 
            DeckManager.RemainingDeck.Count > 0 && 
            DialogueManager.ReadUserInput && 
            DialogueManager.UserCanPlayCard &&
            DialogueManager.CurrentEnergy >= DialogueManager.DrawButtonEnergyCost && 
            !CantDrawAnymore; // maybe add a bool in gamemanager/dialogueManager to toggle this.
        button.interactable = enabled;
    }

    public void OnButtonPressed()
    {
        drawCounter++;
        if(drawCounter >= MaxDrawTimes)
        {
            CantDrawAnymore = true;
        }
        DialogueManager.DrawCards(N: 1, forceDraw: true);
        DialogueManager.CurrentEnergy = DialogueManager.CurrentEnergy - DialogueManager.DrawButtonEnergyCost;
        UpdateButtonEnabled();

        Debug.Log($"{DeckManager.RemainingDeck.Count} Cards left in remaining deck");
    }

    public void OnPlayerPlayedCardFinish()
    {
        Debug.Log("PLayer played card finished");
        CantDrawAnymore = false;
        drawCounter = 0;
        UpdateButtonEnabled();
    }
}
