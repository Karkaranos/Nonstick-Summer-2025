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
using Unity.VisualScripting;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TMP_Text npcText;

    [HideInInspector] private int NumberInList = 0; // TODO move this to npc dialogue bubble?

    [ReadOnly] public bool PlayerReadAllDialogue;


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

    public IEnumerator LoadNewDialogue(DialogueBranch branch = null, DialogueOption option = null, bool branchSwitch = false)
    {
        branch = branch ?? DialogueManager.CurrentDialogueBranch;

        PlayerReadAllDialogue = false;

        if(branchSwitch)
        {

            NumberInList = 0;

            DialogueNPC[] newText = new DialogueNPC[option.NpcReactionText.Length + branch.dialogue.Length];

            for (int i = 0; i < option.NpcReactionText.Length; i++)
            {

                newText[i] = option.NpcReactionText[i];

            }
            for(int i = 0; i < branch.dialogue.Length; i++)
            {

                newText[i + option.NpcReactionText.Length] = branch.dialogue[i];

            }

            branch.dialogue = newText;

            yield return SetDialogueIndex(0, branch);

        }
        else
        {

            DialogueNPC[] combinedTexts = new DialogueNPC[branch.dialogue.Length + option.NpcReactionText.Length];

            for(int i = 0; i < (NumberInList + 1); i++)
            {

                combinedTexts[i] = branch.dialogue[i];

            }
            for(int i = 0; i < option.NpcReactionText.Length; i++)
            {

                combinedTexts[i + (NumberInList + 1)] = option.NpcReactionText[i];

            }
            for (int i = (NumberInList + 1); i < branch.dialogue.Length; i++)
            {

                combinedTexts[i + option.NpcReactionText.Length] = branch.dialogue[i];

            }

            branch.dialogue = combinedTexts;

            yield return SetDialogueIndex(NumberInList + 1, branch);

        }

    }

    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    /// <param name="numberInList">the current line of dialogue that the player is on</param>
    public IEnumerator ProgressNPCDialogue(DialogueBranch branch=null)
    {
        yield return SetDialogueIndex(NumberInList+1); // mods it in this function dw
    }

    public IEnumerator SetDialogueIndex(int numberInList, DialogueBranch branch = null)
    {
        branch = branch ?? DialogueManager.CurrentDialogueBranch;

        if (branch == null)
        {
            Debug.LogError("No branch has been set");
            yield break;
        }

        if (branch.dialogue[NumberInList].Pause == true)
        {
            PlayerReadAllDialogue = true;
            Debug.Log("player read all text");
        }

        // go to next 
        NumberInList = numberInList % branch.dialogue.Length;

        Debug.Log($"({NumberInList + 1}/{branch.dialogue.Length}): {branch.dialogue[NumberInList].Dialogue}");

        DialogueUIController.Instance.portraitDisplay?.SetPortraitSprite(branch.dialogue[NumberInList]);

        //TODO typewriter text goes here
        npcText.text = branch.dialogue[NumberInList].Dialogue;

        if (PlayerReadAllDialogue)
        {
            DialogueUIController.Instance.ClosingOutCombat();
        }

        yield return null;
    }

    #endregion
}
