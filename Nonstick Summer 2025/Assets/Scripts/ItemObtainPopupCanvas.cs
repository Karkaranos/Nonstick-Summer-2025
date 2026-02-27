/*****************************************************************************
* File Name :         CardData.cs
* Author :            Cade, Toby
* Creation Date :     July 30, 2025
* Last Edited:        Feb  15, 2026
*
* Brief Description : Lets the player know what they picked up when they obtain an item
* 
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Collections;

public class ItemObtainPopupCanvas : MonoBehaviour
{
    [HideInInspector] public ModifierData modifier;
    [HideInInspector] public CardData card;

    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_Text statement;
    [SerializeField] private GameObject cardPickup;
    [SerializeField] private GameObject modifierPickup;

    [Header("Text")]
    [SerializeField, Tooltip("Displays before a Stamp's name")] private string foundModifierStatement = "You got a";
    [SerializeField, Tooltip("Displays before a Stamp's name")] private string foundScissorsStatement = "You got some [Scissors]!";
    [SerializeField, Tooltip("Displays before a Card's description")] private string modifierMessage = "Stamp Description: ";
    [SerializeField, Tooltip("Displays before a Card's name")] private string foundCardStatement = "You found a";
    [SerializeField, Tooltip("Displays when a card is picked up")] private string cardMessage = "Would you like to pick it up?";

    [Header("Card Components")]
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private Button takeCard;
    [SerializeField] private Button leaveCard;

    [Header("Modifier Components")]
    [SerializeField] private ModifierCardDisplay modifierDisplay;
    [SerializeField] private Button confirmModifier;

    [Header("Other components")]
    [SerializeField, Required] private CanvasGroup backgroundGroup;
    [SerializeField, Required] private List<CanvasGroup> collectableGroups;

    private CardPickupInteractable cardPickupScript;

    public void Initialize(ModifierData? modifier = null, CardData? card = null, CardPickupInteractable? origin = null)
    {
        // it doesnt matter if its a card or modifier. assign all the buttons
        leaveCard.onClick.AddListener(() => { UIUtilityFunctions.CloseCurrentPopup(); });
        takeCard.onClick.AddListener(CollectCardAnimation);
        confirmModifier.onClick.AddListener(CollectCardAnimation);

        if(modifier == null && card == null)
        {
            return;
        }

        if(origin!=null)
        {
            cardPickupScript = origin;
            takeCard.onClick.AddListener(() => origin.TakeCard());
        }

        if(modifier != null)
        {
            CollectModifer(modifier);
        }
        else
        {
            CollectCard(card);
        }
    }

    void CollectModifer(ModifierData modifier)
    {
        this.modifier = modifier;
        cardPickup.SetActive(false);
        modifierPickup.SetActive(true);

        modifierDisplay.SetCard(modifier);
        // Emotion / Intention changers
        if (modifier is EmotionChangeModifier || modifier is IntentionChangeModifier)
        {
            statement.text = TextUtilities.FilterText($"{foundModifierStatement} [{modifier.GetModifierName()}] Stamp!");
            message.text = TextUtilities.FilterText($"Stamp Description: {modifier.GetTooltipDescription()}");
            return;
        }
        // Scissors only
        else if (modifier is DestroyCardModifier)
        {
            Debug.Log("picked up scissors");
            statement.text = TextUtilities.FilterText($"{foundScissorsStatement}");
            message.text = TextUtilities.FilterText($"Tool Description: {modifier.GetTooltipDescription()}");
            return;
        }
        // all stamps
        else if (modifier.ModifierType == ModifierType.Sticker)
        {
            statement.text = TextUtilities.FilterText($"{foundModifierStatement} [{GetNameFromSticker(modifier)}] Sticker!");
            message.text = TextUtilities.FilterText(modifierMessage + GetMessageFromApply(modifier.GetTooltipDescription()));
            return;
        }
        // idk tbh
        else if (modifier.ModifierType == ModifierType.Tool)
        {
            statement.text = TextUtilities.FilterText($"{foundModifierStatement} [{modifier.name}] tool!");
            message.text = TextUtilities.FilterText($"Tool Description: {modifier.GetTooltipDescription()}");
            return;
        }
        Debug.LogError("unknown card format with " + modifier.name);
        //message.text = TextUtilities.FilterText($"{modifierMessage} {modifier.GetTooltipDescription()}");
    }

    void CollectCard(CardData card)
    {
        this.card = card;
        cardPickup.SetActive(true);
        modifierPickup.SetActive(false);
        foundCardStatement += (card.GetEmotion() == CardEmotion.Assertive ? "n " : " "); // what?
        statement.text = TextUtilities.FilterText($"{foundCardStatement}{ColorCard(card.GetEmotion())}[{card.GetIntention()}] card!");

        message.text = cardMessage;


        cardDisplay.SetCard(card);
    }

    // TODO: Get the color part to work
    private string ColorCard(CardEmotion emotion)
    {
        switch (card.GetEmotion())
        {
            case CardEmotion.Charming:
                return "[Charming] " ;
            case CardEmotion.Assertive:
                return "[Assertive] " ;
            case CardEmotion.Sappy:
                return  "[Sappy] " ;
            default:
                return "ERROR";
        }
    }

    // TODO: Get the color part to work
    private string ColorStamp(string stampName)
    {
        switch (stampName)
        {
            case "ChangeToCharming":
                return " [Charming] ";
            case "ChangeToAssertive":
                return "n [Assertive] ";
            case "ChangeToSappy":
                return  " [Sappy] " ;
            default:
                return "ERROR";
        }
    }

    //there might be a string find function, idc
    private string GetNameFromSticker(ModifierData modifier)
    {
        return modifier.GetModifierName();
        //return stampName.Substring(0, stampName.Length - 7);
    }

    private string GetMessageFromApply(string desc)
    {
        int splitMe = desc.LastIndexOf("\n") +1;
        return desc.Substring(splitMe, desc.Length - splitMe);

    }

    #region Collect animation

    private void CollectCardAnimation()
    {
        // Play both animations, but one of them is invisible (this is totally the best way to do this yeah trust)
        StartCoroutine(CollectCardAnimationCoroutine(modifierDisplay.GetComponent<RectTransform>()));
        StartCoroutine(CollectCardAnimationCoroutine(cardDisplay.GetComponent<RectTransform>()));
    }

    private IEnumerator CollectCardAnimationCoroutine(RectTransform display)
    {
                                                               // fuck canvas groups bro
        yield return StaticUtilities.FadeOpacity(backgroundGroup, 0.001f, seconds: 0.5f);

        StartCoroutine(RotateForever(display));
        yield return new WaitForSeconds(0.25f); // wait a smidge so you can see it spin LOL

        // move to top left
        StaticUtilities.AnimateUIPosition(display, TabIconButton.Instance.rectTransform.position, seconds: 0.5f);
        yield return StaticUtilities.AnimateScale     (display, new Vector3(0, 0, 0), seconds: 0.5f);

        // shake ur little icon buddy!
        yield return TabIconButton.Instance.CollectedCardShakeAnimation();

        UIUtilityFunctions.CloseCurrentPopup();
    }

    private IEnumerator RotateForever(RectTransform display)
    {
        // hard coding this 
        float z = 0;
        while(display != null)
        {
            z += Time.deltaTime * 700;
            display.eulerAngles = new Vector3(0, 0, z);
            yield return null;
        }
    }

    #endregion
}
