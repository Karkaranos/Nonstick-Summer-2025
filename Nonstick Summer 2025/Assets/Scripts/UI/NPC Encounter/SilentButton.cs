/*****************************************************************************
* File Name :         SilentButton.cs
* Author :            Toby
* Creation Date :     8/5/2025 (day before code freeze)
*
* Brief Description : 
* 
*****************************************************************************/

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MouseInteractionEvents))]
public class SilentButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private TMP_Text energyCostDisplay;
    private MouseInteractionEvents mouseInteractionEvents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        energyCostDisplay.text = "+" + DialogueManager.EnergyGainedIfSilent;

        mouseInteractionEvents = GetComponent<MouseInteractionEvents>();
        mouseInteractionEvents.OnMouseHoverStart.AddListener(OnButtonHover);

        button.onClick.AddListener(OnButtonPressed);
        UpdateButtonEnabled();

        DialogueManager.OnCardPlayedStarted.AddListener(UpdateButtonEnabled);
        DialogueManager.OnPlayerFinishReadingDialogue.AddListener(UpdateButtonEnabled);
        DialogueManager.OnCardPlayedFinished.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool enabled =
            DialogueManager.ReadUserInput &&
            DialogueManager.UserCanPlayCard;
        button.interactable = enabled;
    }

    public void OnButtonPressed()
    {
        StartCoroutine(DialogueManager.ProcessPlayCard(null));
        UpdateButtonEnabled();
    }

    public void OnButtonHover()
    {
        DialogueUIController.Instance.UpdateHoveringCard(null);
    }
}
