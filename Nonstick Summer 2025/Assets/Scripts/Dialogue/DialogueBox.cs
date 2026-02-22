/*****************************************************************************
* File Name :         DialogueBox.cs
* Author :            Jay, Toby
* Creation Date :     June 9, 2025
*
* Brief Description :  Displays the NPC's dialogue
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    //[SerializeField, Required] private CanvasGroup group;

    [HideInInspector] private int NumberInList = 0;

    [ReadOnly] public bool PlayerReadAllDialogue;

    [ReadOnly] public bool DialogueScrolling = false;
    [ReadOnly] public bool skipTypewriterRequested = false;

    private DialogueNPC[] dialogueStored;
    private Character currentCharacter;

    float scrollSpeed;


    /// <summary>
    /// displays dialogue according to where the player is in a dialogue branch
    /// </summary>
    /// <param name="branch">the current dialogue branch that the player is on</param>
    public IEnumerator Initialize(DialogueBranch branch, Character character)
    {
        dialogueText = dialogueText != null ? dialogueText : GetComponentInChildren<TMP_Text>();

        NumberInList = 0;

        currentCharacter = character;

        scrollSpeed = GameManager.Instance.TextScrollSpeed;

        yield return SetDialogueIndex(NumberInList, branch);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        //npcText.text = branch.dialogue[0].Dialogue; text initialized 

    }

    public void SkipTypewriter()
    {
        skipTypewriterRequested = true;
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

        if(DialogueScrolling)
        {

            //i'll figure out something more graceful later but the text boxes will fuck up otherwise
            yield return null;

        }
        else
        {
            scrollSpeed = GameManager.Instance.TextScrollSpeed;
            yield return SetDialogueIndex(NumberInList + 1, null, dialogueStored);

        }

    }

    public void MuffleTextPlayed(string line)
    {
        StartCoroutine(DisplayOneLine(line));
    }

    IEnumerator DisplayOneLine(string line)
    {

        DialogueScrolling = true;

        for (int i = 0; i < line.Length; i++)
        {

            dialogueText.text += line[i];

            yield return new WaitForSeconds(scrollSpeed);

        }

        DialogueScrolling = false;

        //RefreshLayout();
    }

    public IEnumerator SetDialogueIndex(int numberInList, DialogueBranch branch = null, DialogueNPC[] dialogue = null)
    {
        //group.transform.SetAsLastSibling(); // bring to front

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
            PlayEmotionReaction(dialogue[NumberInList].AudioResponse);

            // Wait for Typewriter Text
            yield return TypewriteText(dialogue[NumberInList].Dialogue);

            Debug.Log($"({NumberInList + 1}/{dialogue.Length}): {dialogue[NumberInList].Dialogue}");

        }
        else
        {

            NumberInList = numberInList % branch.dialogue.Length;

            DialogueUIController.Instance.portraitDisplay?.SetPortraitSprite(branch.dialogue[NumberInList], currentCharacter);

            //TODO: fmod here!!

            PlayEmotionReaction(branch.dialogue[NumberInList].AudioResponse);

            // Wait for Typewriter Text
            yield return TypewriteText(branch.dialogue[NumberInList].Dialogue);

            Debug.Log($"({NumberInList + 1}/{branch.dialogue.Length}): {branch.dialogue[NumberInList].Dialogue}");

        }

        if (PlayerReadAllDialogue)
        {
            DialogueUIController.Instance.ClosingOutCombat();
        }

        yield return null;
        //RefreshLayout();
    }

    private IEnumerator TypewriteText(string text)
    {
        skipTypewriterRequested = false;
        if (dialogueText.text != text)
        {
            DialogueScrolling = true;
            dialogueText.text = string.Empty;

            for (int i = 0; i < text.Length && !skipTypewriterRequested; i++)
            {
                //dialogueText.text += text[i];
                dialogueText.text = text.Substring(0, i);

                yield return new WaitForSeconds(scrollSpeed);
            }
            // just apply all the dialogue to be safe
            dialogueText.text = text;

            skipTypewriterRequested = false;
            DialogueScrolling = false;
        }
    }

    private void PlayEmotionReaction(string AudioResponse)
    {
        if (AudioResponse == "Happy")
        {

            ReactionManager.instance.PlayReaction(1);

        }
        if (AudioResponse == "Sad")
        {

            ReactionManager.instance.PlayReaction(2);

        }
        if (AudioResponse == "Angry")
        {

            ReactionManager.instance.PlayReaction(3);

        }
        if (AudioResponse == "Neutral")
        {

            ReactionManager.instance.PlayReaction(0);

        }
    }

    #endregion
}
