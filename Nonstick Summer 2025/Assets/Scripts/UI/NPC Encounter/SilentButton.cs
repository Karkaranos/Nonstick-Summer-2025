/*****************************************************************************
* File Name :         SilentButton.cs
* Author :            Toby
* Creation Date :     8/5/2025 (day before code freeze)
*
* Brief Description : 
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SilentButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
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
        DialogueManager.ProcessPlayCard(null);
        UpdateButtonEnabled();
    }
}
