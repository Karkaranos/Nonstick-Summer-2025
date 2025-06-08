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

using System.Collections;
using TMPro;
using UnityEngine;

// didnt know what to name this script so i just put 3 keywords together
public class DisplayPlayerCardDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup group;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
    }

    public void Hide()
    {
        StaticUtilities.DisableCanvasGroup(group);
    }

    public void WriteText(CardData card)
    {
        if (card == null)
        {
            Hide();
            return;
        }

        StaticUtilities.EnableCanvasGroup(group, interactable:false);
        text.text = DialogueManager.CurrentDialogueBranch.ReturnDialogueOption(card).PlayerDialogue;
    }
}
