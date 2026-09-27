/*****************************************************************************
* File Name :         TutorialDialogueUIController.cs
* Author :            Toby
* Creation Date :     Feb 25, 2026
*
* Brief Description : Override of DialogueUIController for the tutorial
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueUIController : DialogueUIController
{
    [Header("Tutorial Exclusive")]
    [SerializeField, Required] private GameObject fadeToBlackPrefab;
    [SerializeField, Scene] private int nextScene;

    public override IEnumerator Initialize(DialogueBranch startBranch, Character character, bool isBoss = true, GameObject objRef = null)
    {
        //DeckManager.SetDeck(GameManager.Instance.tutorialCards);

        // set the players hand without setting their whole entire deck yk
        DeckManager.PlayerHand.SetCards(GameManager.Instance.tutorialCards);
        DeckManager.RemainingDeck.Clear();


        yield return base.Initialize(startBranch, character, isBoss, objRef);

        StaticUtilities.DisableCanvasGroup(silentButton.GetComponent<CanvasGroup>());
    }

    public override IEnumerator ToggleUIForDialogueProgression(bool interactable)
    {
        yield return base.ToggleUIForDialogueProgression(interactable);

        if (interactable && deckDisplay.GetCardsCount() == 0)
        {
            //silentButton.gameObject.SetActive(true);
            //yield return StaticUtilities.FadeToVisible(silentButton.GetComponent<CanvasGroup>(), 0.65f, unscaledTime:true);
            StaticUtilities.EnableCanvasGroup(silentButton.GetComponent<CanvasGroup>());
        }
    }

    public override void NextTextPressed()
    {
        if (IfCloseCombat)
        {
            OnCombatEnded();
            return;
        }
        base.NextTextPressed();
    }

    public void OnCombatEnded()
    {
        //DeckManager.SetDeck(GameManager.Instance.startingCards);
        //Debug.Log(DeckManager.PlayerFullDeck.Count);

        SteamAchievementManager.Instance.UnlockAchievement(SteamAchievement.CompleteTutorial);

        Debug.Log("Tutorial ended");

        DoFadeOut();
    }

    public void DoFadeOut()
    {
        var canvas = Instantiate(fadeToBlackPrefab);
        canvas.SetActive(true);
        var fade = canvas.GetComponent<FadeTransition>();
        var image = canvas.GetComponentInChildren<Image>();

        if (fade != null)
        {
            fade.StartFadeOut(image, nextScene);
        }
    }
}

