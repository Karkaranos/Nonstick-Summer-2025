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
    [SerializeField, Required] private CanvasGroup group;

    [HideInInspector] private int NumberInList = 0; // TODO move this to npc dialogue bubble?

    [ReadOnly] public bool PlayerReadAllDialogue;

    private DialogueNPC[] dialogueStored;
    private Character currentCharacter;


    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    public IEnumerator Initialize(DialogueBranch branch, Character character)
    {
        npcText = npcText != null ? npcText : GetComponentInChildren<TMP_Text>();

        NumberInList = 0;

        currentCharacter = character;

        yield return SetDialogueIndex(NumberInList, branch); 
        //npcText.text = branch.dialogue[0].Dialogue; text initialized 
    }

    #region Dialogue Iteration

    public IEnumerator LoadNewDialogue(DialogueBranch branch = null, DialogueOption option = null)
    {
        branch = branch ?? DialogueManager.CurrentDialogueBranch;

        option.SetNextBranchReaction(branch);
        dialogueStored = option.CombinedDialogue;

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

    public void DisplayOneLine(string line)
    {
        npcText.text = line;
    }

    public IEnumerator SetDialogueIndex(int numberInList, DialogueBranch branch = null, DialogueNPC[] dialogue = null)
    {
        group.transform.SetAsLastSibling(); // bring to front

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
        else if (dialogue != null && numberInList >= dialogue.Length - 1)
        {

            PlayerReadAllDialogue = true;
            Debug.Log("player read all text");

        }

        // go to next 
        if (dialogue != null)
        {

            NumberInList = numberInList % dialogue.Length;

            DialogueUIController.Instance.portraitDisplay?.SetPortraitSprite(dialogue[NumberInList], currentCharacter);

            //TODO: fmod here!!
            if(dialogue[NumberInList].AudioResponse == "Happy")
            {

                ReactionManager.instance.PlayReaction(1);

            }
            if (dialogue[NumberInList].AudioResponse == "Sad")
            {

                ReactionManager.instance.PlayReaction(2);

            }
            if (dialogue[NumberInList].AudioResponse == "Angry")
            {

                ReactionManager.instance.PlayReaction(3);

            }
            if (dialogue[NumberInList].AudioResponse == "Neutral")
            {

                ReactionManager.instance.PlayReaction(0);

            }

            npcText.text = dialogue[NumberInList].Dialogue;

            Debug.Log($"({NumberInList + 1}/{dialogue.Length}): {dialogue[NumberInList].Dialogue}");

        }
        else
        {

            NumberInList = numberInList % branch.dialogue.Length;

            DialogueUIController.Instance.portraitDisplay?.SetPortraitSprite(branch.dialogue[NumberInList], currentCharacter);

            //TODO: fmod here!!
            if (branch.dialogue[NumberInList].AudioResponse == "Happy")
            {

                ReactionManager.instance.PlayReaction(1);

            }
            if (branch.dialogue[NumberInList].AudioResponse == "Sad")
            {

                ReactionManager.instance.PlayReaction(2);

            }
            if (branch.dialogue[NumberInList].AudioResponse == "Angry")
            {

                ReactionManager.instance.PlayReaction(3);

            }
            if (branch.dialogue[NumberInList].AudioResponse == "Neutral")
            {

                ReactionManager.instance.PlayReaction(0);

            }

            npcText.text = branch.dialogue[NumberInList].Dialogue;

            Debug.Log($"({NumberInList + 1}/{branch.dialogue.Length}): {branch.dialogue[NumberInList].Dialogue}");

        }

        //TODO typewriter text goes here

        if (PlayerReadAllDialogue)
        {
            DialogueUIController.Instance.ClosingOutCombat();
        }

        yield return null;
    }

    #endregion
}
