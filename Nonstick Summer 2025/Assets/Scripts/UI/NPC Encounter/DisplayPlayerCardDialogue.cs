/*****************************************************************************
* File Name :         DisplayCardDialogue.cs
* Author :            Toby
* Creation Date :     June 8, 2025
*
* Brief Description : if player is playing a card: display the text from the card.
* if dialogue is animating progression: animate tooltip typrewriter style
*
* TODO:
 * implement typewriter
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;

// didnt know what to name this script so i just put 3 keywords together
public class DisplayPlayerCardDialogue : MonoBehaviour
{
    [SerializeField, Required] private TMP_Text text;
    [SerializeField, Required] private CanvasGroup group;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
    }

    public void Hide()
    {
        // dont hide if a card is selected
        if (DialogueUIController.Instance != null && DialogueUIController.Instance.selectedCardData != null)
            return;

        StaticUtilities.DisableCanvasGroup(group);
    }

    public void WriteText(CardData card)
    {
        if (card == null)
        {
            Hide();
            return;
        }

        if(DialogueManager.CurrentDialogueBranch == null)
        {
            Debug.LogError("Current dialogue branch is null");
            return;
        }

        StaticUtilities.EnableCanvasGroup(group, interactable:false);
        var cardtext = DialogueManager.CurrentDialogueBranch.ReturnDialogueOption(card).PlayerDialogue;
        text.text = cardtext;
        Debug.Log(cardtext);
    }

    public IEnumerator WriteTextTypewriter(CardData card)
    {
        if (card == null)
        {
            text.text = "silent text (make this better later)";
            yield break;
        }

        StaticUtilities.EnableCanvasGroup(group, interactable: false);
        text.text = DialogueManager.CurrentDialogueBranch.ReturnDialogueOption(card).PlayerDialogue;
        Debug.LogWarning("Implement typewriter later");
        yield return null;
    }
}
