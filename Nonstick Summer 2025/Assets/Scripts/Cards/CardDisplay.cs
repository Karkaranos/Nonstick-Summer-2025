using System;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;

//[RequireComponent(typeof(MouseInteractionEvents))]
public partial class CardDisplay : MonoBehaviour
{
    [Header("Display")]

    [Foldout("UI Components"), SerializeField, Required] TMP_Text EmotionText;
    [Foldout("UI Components"), SerializeField, Required] TMP_Text IntentionText;
    [Foldout("UI Components"), SerializeField, Required] Image CardBackgroundImage;
    [Foldout("UI Components"), SerializeField, Required] CanvasGroup CardFrontGroup;
    [Foldout("UI Components"), SerializeField, Required] CanvasGroup CardBackGroup;
    [Foldout("UI Components"), SerializeField, Required] RectTransform cardBackground;
    [Foldout("UI Components"), SerializeField, Required] Image IntentionImage;
    [Foldout("UI Components"), SerializeField, Required] TMP_Text EnergyText;
    [Foldout("UI Components"), SerializeField] Image[] energyCostIcons;
    [Foldout("UI Components"), SerializeField] StampIconDisplay[] StampImages;

    public CardData cardData { get{ return card; } }

    [SerializeField] [Tooltip("Set this for debug only")]
    private CardData card;

    private MouseInteractionEvents mouseInteraction;
    private RectTransform rectTransform;
    private RenderMode renderMode;

    public UnityEvent<CardDisplay> OnMouseDown = new UnityEvent<CardDisplay> ();

    bool canPlayHover = true;

    private void Start()
    {
        if (card != null) SetCard(card); // mostly for debugging

        rectTransform = GetComponent<RectTransform>();

        if(TryGetComponent<MouseInteractionEvents>(out mouseInteraction))
        {
            mouseInteraction.OnMouseHoverStart.AddListener(OnMouseHoverStart);
            mouseInteraction.OnMouseHoverEnd.AddListener(OnMouseHoverEnd);
            mouseInteraction.OnMouseHoverStay.AddListener(OnMouseHoverStart);
            mouseInteraction.OnMouseDown.AddListener(OnMouseDownStart);
        }

        renderMode = transform.root.GetComponent<Canvas>().renderMode;

        // EVERYTHING breaks if you uncomment this. DO NOT touch it.
        //basePosition = cardBackground.anchoredPosition;
    }

    /// <summary>
    /// using update because cards can move
    /// </summary>
    private void Update()
    {
        if(GameManager.PlayerCameraRef == null || rectTransform == null)
        { 
            rectTransform = GetComponent<RectTransform>();
            return;
        }

        bool facingFront;
        if(renderMode == RenderMode.WorldSpace)
        {
            // Math 
            Vector3 toCamera = GameManager.PlayerCameraRef.transform.position - rectTransform.WorldPosition();
            float dot = Vector3.Dot(transform.forward, toCamera.normalized);
            facingFront = dot <= 0;
        }
        else
        {
            float y = rectTransform.localEulerAngles.y;
            facingFront = ((-90 <= y && y <= 90) || (270 <= y && y <= 450));
        }

        StaticUtilities.ToggleCanvasGroup(CardFrontGroup, facingFront);
        StaticUtilities.ToggleCanvasGroup(CardBackGroup, !facingFront);
    }

    private void OnMouseHoverStart() // TODO this should be moved to another script
    {
        if (DialogueUIController.Instance != null && DialogueUIController.Instance.DeckDisplay.FirstSelectedCard == null 
            && DialogueManager.PlayerInCombat && DialogueManager.ReadUserInput)
           DialogueUIController.Instance.UpdateHoveringCard(card);
        if (canPlayHover)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CardHoverSFX);
            canPlayHover = false;
        }
    }

    private void OnMouseHoverEnd() // TODO this should be moved to another script
    {
        if (DialogueUIController.Instance != null && DialogueUIController.Instance.DeckDisplay.FirstSelectedCard == null
            && DialogueManager.PlayerInCombat && DialogueManager.ReadUserInput)
            DialogueUIController.Instance.UpdateHoveringCard(null);
        canPlayHover = true;
    }

    private void OnMouseDownStart()
    {
        OnMouseDown.Invoke(this);
        /*if (DialogueUIController.Instance != null && DialogueManager.ReadUserInput)
        {
            Debug.Log("selected card");
            StartCoroutine(DialogueUIController.Instance.OnSelectionUpdated(this));
        }*/
    }

    public void SetCard(CardData newCard)
    {
        if(card != null)
            card.OnCardValueChanged -= (() => RefreshDisplay(true));

        card = newCard;
        card.OnCardValueChanged += (() => RefreshDisplay(true));

        RefreshDisplay(false);
    }

    [Button]
    public void RefreshDisplay(bool animate = true)
    {
        if(card == null)
        {
            Debug.LogWarning("No card is set.");
            return;
        }

        if (GameManager.CardStyleManagerReference == null)
            return;

        if(animate)
        {
            StartCoroutine(RefreshDisplayAnimation(animate));
            return;
        }
        RefreshDisplayPrivate();
    }

    /// <summary>
    /// Flippy dippy
    /// </summary>
    private IEnumerator RefreshDisplayAnimation(bool animate)
    {
        Vector3 startRotation = rectTransform.localEulerAngles;
        float halfAnimationLength = RefreshCardTime / 2;
        float time, t, y, timeStarted;

        // 0 degrees to 180
        timeStarted = Time.time;
        do
        {
            time = Time.time - timeStarted;
            t = time / (halfAnimationLength);
            y = Mathf.Lerp(0, 180, t);
            rectTransform.localEulerAngles = startRotation + new Vector3(0, y, 0);
            yield return null;
        }
        while (time < halfAnimationLength);

        // this is where the magic happens
        RefreshDisplayPrivate();

        // 180 to 360
        timeStarted = Time.time;
        do
        {
            time = Time.time - timeStarted;
            t = time / (halfAnimationLength);
            y = Mathf.Lerp(180, 360, t);
            rectTransform.localEulerAngles = startRotation + new Vector3(0, y, 0);
            yield return null;
        }
        while (time < halfAnimationLength);

        rectTransform.localEulerAngles = startRotation;
    }

    private void RefreshDisplayPrivate()
    {
        EmotionText.text = CardStyleManager.GetEmotionStyle(card).DisplayName;
        IntentionText.text = CardStyleManager.GetIntentionStyle(card).DisplayName;
        EnergyText.text = (card.EnergyCost < 0) ? "" : $"+{card.EnergyCost.ToString()}"; // the text is still there just in case the cost somehow ends up giving the player energy
        //EnergyText.color = (card.EnergyCost > 0) ? Color.red : Color.green;
        IntentionImage.sprite = CardStyleManager.GetIntentionSprite(card);
        CardBackgroundImage.sprite = CardStyleManager.GetCardBack(card);
        UpdateEnergyDisplay();
        UpdateStampIcons();
    }

    private void UpdateEnergyDisplay()
    {
        int i;
        for(i = 0; i< Mathf.Abs(card.EnergyCost); i++)
        {
            if(energyCostIcons.Length <= i)
            {
                Debug.LogWarning("not enough energy icons to meaningfully represent cost!");
                return;
            }
            energyCostIcons.ElementAt(i).enabled = true;
        }
        for (; i < energyCostIcons.Length; i++)
        {
            energyCostIcons.ElementAt(i).enabled = false;
        }

        //TODO: change color of icons based on cost.
    }

    private void UpdateStampIcons()
    {
        string[] names =
        {
            "Overthinking", "Repetition", "Mumble", "Confidence", "Energy Bonus"
        };

        for(int i=0; i<names.Length && i < StampImages.Length; i++)
        {
            int index = HasCard(names[i]);
            if (index > -1 && index < StampImages.Length)
            {
                StampImages[i].SetStamp(card.Stamps.ElementAt(index));
            }

        }
        
    }

    private int HasCard(string test)
    {
        for(int i=0; i<card.Stamps.Count; i++)
        {
            if (card.Stamps.ElementAt(i).StampName == test)
                return i;
        }
        return -1;
    }

}
