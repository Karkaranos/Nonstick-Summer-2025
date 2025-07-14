/*****************************************************************************
* File Name :         DialogueBox.cs
* Author :            Jay, Toby
* Creation Date :     June 9, 2025
*
* Brief Description :  Displays the NPC's dialogue
* 
*****************************************************************************/

using System.Collections;
using TMPro;
using NaughtyAttributes;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TMP_Text npcText;

    [HideInInspector] private int NumberInList = 0; // TODO move this to npc dialogue bubble?

    [ReadOnly] public bool PlayerReadAllDialogue;

    private DialogueNPC[] dialogueStored;


    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    public IEnumerator Initialize(DialogueBranch branch)
    {
        npcText = npcText != null ? npcText : GetComponentInChildren<TMP_Text>();

        NumberInList = 0;

        yield return SetDialogueIndex(NumberInList, branch); 
        //npcText.text = branch.dialogue[0].Dialogue; text initialized 
    }

    #region Dialogue Iteration

    public IEnumerator LoadNewDialogue(DialogueBranch branch = null, DialogueOption option = null)
    {
        branch = branch ?? DialogueManager.CurrentDialogueBranch;

        dialogueStored = option.CombinedDialogue;

        option.SetNextBranchReaction(branch);

        PlayerReadAllDialogue = false;
        NumberInList = 0;

        yield return SetDialogueIndex(0, branch, option.CombinedDialogue);
    }

    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    /// <param name="numberInList">the current line of dialogue that the player is on</param>
    public IEnumerator ProgressNPCDialogue(DialogueBranch branch=null)
    {
        yield return SetDialogueIndex(NumberInList+1, null, dialogueStored); // mods it in this function dw
    }

    public IEnumerator SetDialogueIndex(int numberInList, DialogueBranch branch = null, DialogueNPC[] dialogue = null)
    {
        branch = branch ?? DialogueManager.CurrentDialogueBranch;

        if (branch == null)
        {
            Debug.LogError("No branch has been set");
            yield break;
        }

        if (dialogue == null && numberInList >= branch.dialogue.Length - 1)
        {
            PlayerReadAllDialogue = true;
            Debug.Log("player read all text");
        }
        else if (dialogue != null && numberInList >= dialogue.Length)
        {

            PlayerReadAllDialogue = true;
            Debug.Log("player read all text");

        }

        // go to next 
        if (dialogue != null)
        {

            NumberInList = numberInList % dialogue.Length;

            DialogueUIController.Instance.portraitDisplay?.SetPortraitSprite(dialogue[NumberInList]);

            npcText.text = dialogue[NumberInList].Dialogue;

        }
        else
        {

            NumberInList = numberInList % branch.dialogue.Length;

            DialogueUIController.Instance.portraitDisplay?.SetPortraitSprite(branch.dialogue[NumberInList]);

            npcText.text = branch.dialogue[NumberInList].Dialogue;

        }

        Debug.Log($"({NumberInList + 1}/{branch.dialogue.Length}): {branch.dialogue[NumberInList].Dialogue}");

        //TODO typewriter text goes here

        if (PlayerReadAllDialogue)
        {
            DialogueUIController.Instance.ClosingOutCombat();
        }

        yield return null;
    }

    #endregion
}
