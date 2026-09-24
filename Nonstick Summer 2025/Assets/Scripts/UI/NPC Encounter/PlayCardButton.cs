/*****************************************************************************
* File Name :         PlayCardButton.cs
* Author :            Toby
* Creation Date :     8/5/2025 (day before code freeze)
* Last Modified :     9/23/2026 (day before steam release)
*
* Brief Description : Plays the selected card
* 
*****************************************************************************/

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardButton : MonoBehaviour
{
    [SerializeField, Required] private Button button;
    [SerializeField, Required] private Image buttonImage;
    [SerializeField, Required] private CanvasGroup group;
    [SerializeField, Required] private CanvasGroup parentGroup;
    [SerializeField, Required] private CanvasGroup playerTextGroup;
    [SerializeField, Required] private Image disabledButtonOverlay;

    [Header("Specific Sprites")]
    [SerializeField, Required] private Sprite disabledSprite;
    [SerializeField, Required] private Sprite defaultSprite;

    private DeckDisplayer hand => DialogueUIController.Instance.deckDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        button.onClick.AddListener(OnButtonPressed);
        UpdateButtonEnabled();

        DialogueManager.OnCardPlayedStarted.AddListener(UpdateButtonEnabled);
        DialogueManager.OnPlayerFinishReadingDialogue.AddListener(UpdateButtonEnabled);
        DialogueManager.OnCardPlayedFinished.AddListener(UpdateButtonEnabled);
        hand.OnCardsSelectedChanged.AddListener(UpdateButtonEnabled);
    }

    /// <summary>
    /// toggle button interactability based of if player has cards they can draw
    /// </summary>
    public void UpdateButtonEnabled()
    {
        bool isHoldingACard = hand.FirstSelectedCard != null;
        var card = hand.FirstSelectedCard;
        bool canAffordCard = isHoldingACard && (Mathf.Abs(card.EnergyCost) <= DialogueManager.CurrentEnergy);
        Debug.Log($"isHoldingACard: {isHoldingACard}\ncanAffordCard:{canAffordCard}");

        //if(isHoldingACard)
        //    Debug.Log($"{Mathf.Abs(card.EnergyCost)} > {DialogueManager.CurrentEnergy} = {(Mathf.Abs(card.EnergyCost) > DialogueManager.CurrentEnergy)}");

        button.interactable = (DialogueUIController.Instance.inSceneFive || (isHoldingACard && canAffordCard));

        float alpha = isHoldingACard ? 1 : 0;
        StaticUtilities.ToggleCanvasGroup(group, 
            enabled: isHoldingACard,
            interactable: canAffordCard, 
            alpha: group.alpha, //isHoldingACard ? 1: 0, 
            ignoreParentGroups:true);
    }

    private void Update()
    {
        //UpdateButtonEnabled();

        // please dont hate me (game is about to release and this is the easiest solution i have to a problem i pulled out of my ass)
        //parentGroup.alpha = playerTextGroup.alpha;
        group.alpha = parentGroup.alpha;


        if (parentGroup.alpha >= 1 && button.interactable)
        {
            var spriteState = button.spriteState;
            spriteState.disabledSprite = defaultSprite;
            button.spriteState = spriteState;
        }
        if (parentGroup.alpha < 1){
            var spriteState = button.spriteState;
            spriteState.disabledSprite = disabledSprite;
            button.spriteState = spriteState;
        }

        float target_a = button.interactable ? 0 : 1;
        float a = Mathf.MoveTowards(disabledButtonOverlay.color.a, target_a, Time.unscaledDeltaTime * 4);        
        disabledButtonOverlay.color = disabledButtonOverlay.color.WithAlpha(a * group.alpha);
    }

    public void OnButtonPressed()
    {
        Debug.Log("Play card button pressed");
        if(DialogueUIController.Instance.selectedCardData != null)
        {

            if(Mathf.Abs(DialogueUIController.Instance.selectedCardData.EnergyCost) > DialogueManager.CurrentEnergy && DialogueUIController.Instance.inSceneFive == false)
            {
                return;
            }

        }
        StartCoroutine(DialogueManager.ProcessPlayCard(DialogueUIController.Instance.selectedCardData));
        UpdateButtonEnabled();
    }
}
