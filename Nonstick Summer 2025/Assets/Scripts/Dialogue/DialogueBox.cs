/*****************************************************************************
* File Name :         DialogueBox.cs
* Author :            Jay
* Creation Date :     June 9, 2025
*
* Brief Description :  Displays the NPC's dialogue
* 
*****************************************************************************/

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    private TMP_Text npcText;


    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    public void Initialize(DialogueBranch branch)
    {

        npcText = GetComponent<TMP_Text>();


        npcText.text = branch.dialogue[0].Dialogue;

    }


    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    /// <param name="numberInList">the current line of dialogue that the player is on</param>
    public void ProgressDialogue(DialogueBranch branch, int numberInList)
    {

        npcText = GetComponent<TMP_Text>();

        npcText.text = branch.dialogue[numberInList].Dialogue;

        if (branch.dialogue[numberInList].End)
        {

            DialogueUIController.Instance.ClosingOutCombat();
            DialogueUIController.Instance.HideDeck();

        }

    }

}
