/*****************************************************************************
* File Name :         NextDialogueButton.cs
* Author :            Toby
* Creation Date :     8/5/2025 (day before code freeze)
*
* Brief Description : 
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class NextDialogueButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private CanvasGroup group;
    [SerializeField, Required] private Sprite EndDialogueSprite;
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
        bool enabled =
            !DialogueManager.ReadUserInput ||
            !DialogueManager.UserCanPlayCard;

        if(DialogueUIController.Instance.IfCloseCombat)
        {
            enabled = true;
            button.targetGraphic.GetComponent<Image>().sprite = EndDialogueSprite;
        }

        button.interactable = enabled;
        StaticUtilities.ToggleCanvasGroup(group, enabled);
    }

    public void OnButtonPressed()
    {
        DialogueUIController.Instance.NextTextPressed();
        UpdateButtonEnabled();
    }
}
