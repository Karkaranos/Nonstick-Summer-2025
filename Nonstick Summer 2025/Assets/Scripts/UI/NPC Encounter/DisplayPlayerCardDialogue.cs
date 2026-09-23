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
using UnityEngine.UI;

// didnt know what to name this script so i just put 3 keywords together
public class DisplayPlayerCardDialogue : MonoBehaviour
{
    [SerializeField, Required] private TMP_Text text;
    [SerializeField, Required] private CanvasGroup group;
    [SerializeField] private float fadeSpeed = 6;
    [ReadOnly] public bool IsShowing;

    private Coroutine fadingCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide(fadeHide:false);
    }

    public void Hide(bool forceHide = false, bool fadeHide = true)
    {
        // dont hide if a card is selected
        if (DialogueUIController.Instance != null && DialogueUIController.Instance.selectedCardData != null && !forceHide)
            return;

        IsShowing = false;

        //;
        if (fadingCoroutine != null) StopCoroutine(fadingCoroutine);
        if (fadeHide)
            fadingCoroutine = StaticUtilities.FadeOpacityBySpeed(group, start_a: group.alpha, 0, fadeSpeed, delay:0.1f); 
        else
            StaticUtilities.DisableCanvasGroup(group);
    }

    public void Show(bool fadeShow = true)
    {
        IsShowing = true;

        if (fadingCoroutine != null) StopCoroutine(fadingCoroutine);
        if (fadeShow)
            fadingCoroutine = StaticUtilities.FadeOpacityBySpeed(group, start_a: group.alpha, 1, fadeSpeed);
        else
            StaticUtilities.EnableCanvasGroup(group);
    }

    public void WriteText(CardData card, bool fadeShow = true)
    {
        group.transform.SetAsLastSibling(); // bring to front
        if (card == null)
        {
            Hide(fadeHide:true);
            return;
        }

        if(DialogueManager.CurrentDialogueBranch == null)
        {
            Debug.LogError("Current dialogue branch is null");
            return;
        }

        //StaticUtilities.EnableCanvasGroup(group, interactable:false);

        Show(fadeShow);

        var cardtext = DialogueManager.CurrentDialogueBranch.GetDialogueOption(card).PlayerDialogue;
        text.text = cardtext;

        RefreshLayout();
    }

    private void RefreshLayout()
    {
        var rectTransform = group.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("How is this null bruh");
            return;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        if (rectTransform.parent != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform.parent as RectTransform);

            // juuuuuust in case
            if (rectTransform.parent.parent != null)
            {
                if (rectTransform.parent.parent is RectTransform)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform.parent.parent as RectTransform);
            }
        }
    }

    public IEnumerator WriteTextTypewriter(CardData card)
    {
        if (card == null)
        {
            text.text = "silent text (make this better later)";
            yield break;
        }

        StaticUtilities.EnableCanvasGroup(group, interactable: false);
        text.text = DialogueManager.CurrentDialogueBranch.GetDialogueOption(card).PlayerDialogue;
        Debug.LogWarning("Implement typewriter later");
        yield return null;
    }
}
