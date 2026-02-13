/*****************************************************************************
* File Name :         CardData.cs
* Author :            Cade
* Creation Date :     July 30, 2025
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
    [SerializeField, Tooltip("Displays before a Stamp's name")] private string modifierStatement = "You obtained a";
    [SerializeField, Tooltip("Displays before a Card's description")] private string modifierMessage = "Stamp Description: ";
    [SerializeField, Tooltip("Displays before a Card's name")] private string cardStatement = "You found a";
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
            this.modifier = modifier;
            cardPickup.SetActive(false);
            modifierPickup.SetActive(true);

            modifierDisplay.SetCard(modifier);
            if (modifier.name.Contains("Change"))
            {
                statement.text = TextUtilities.FilterText(modifierStatement + ColorStamp(modifier.name) + "Stamp!");
            }
            else if (modifier.name.ToLower().Contains("applier"))
            {
                statement.text = modifierStatement + " " + GetNameFromApply(modifier.name) + "Stamp!";
                message.text = TextUtilities.FilterText(modifierMessage + GetMessageFromApply(modifier.GetTooltipDescription()));
                return;
            }
            else
            {
                statement.text = modifierStatement + " " +  modifier.name + " Stamp!";
            }
            message.text = TextUtilities.FilterText(modifierMessage + modifier.GetTooltipDescription());
        }
        else
        {
            this.card = card;
            cardPickup.SetActive(true);
            modifierPickup.SetActive(false);
            cardStatement += (card.GetEmotion() == CardEmotion.Assertive ? "n " :  " ");
            statement.text = TextUtilities.FilterText(cardStatement + ColorCard(card.GetEmotion()) + card.GetIntention() + " card!");

            message.text = cardMessage;


            cardDisplay.SetCard(card);
        }
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
    private string GetNameFromApply(string stampName)
    {
        return stampName.Substring(0, stampName.Length - 7);
    }

    private string GetMessageFromApply(string desc)
    {
        int splitMe = desc.LastIndexOf("\n") +1;
        return desc.Substring(splitMe, desc.Length - splitMe);

    }

    #region Collect animation

    private void CollectCardAnimation()
    {
        StartCoroutine(CollectCardAnimationCoroutine(modifierDisplay.GetComponent<RectTransform>()));
        StartCoroutine(CollectCardAnimationCoroutine(cardDisplay.GetComponent<RectTransform>()));
    }

    private IEnumerator CollectCardAnimationCoroutine(RectTransform display)
    {
                                                               // fuck canvas groups bro
        yield return StaticUtilities.FadeOpacity(backgroundGroup, 0.001f, seconds: 0.5f);

        // move to top left

                     StaticUtilities.AnimateUIPosition(display, TabIconButton.Instance.rectTransform.position, seconds: 1f);
        yield return StaticUtilities.AnimateScale     (display, new Vector3(0, 0, 0), seconds: 1f);

        UIUtilityFunctions.CloseCurrentPopup();
    }

    #endregion
}
