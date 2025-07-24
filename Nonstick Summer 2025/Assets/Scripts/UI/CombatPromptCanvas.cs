/*************************************************
Author Names :          Cade Naylor
Date Created :          July 23, 2025
Date Modified :         July 23, 2025
Brief Description :     Initializes the pre-combat canvas given the player's interaction state with them
***************************************************/
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatPromptCanvas : MonoBehaviour
{
    [SerializeField] private GameObject notInteractedUI;
    [SerializeField] private GameObject interactedUI;
    [SerializeField] private TMP_Text dialogueBubbleText;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button startCombatButton;
    public void Initialize(string speechBubbleLine, bool hasInteracted, Sprite image, SideCharacterInteractable sci, string questionLine = null)
    {
        notInteractedUI.SetActive(!hasInteracted);
        interactedUI.SetActive(hasInteracted);

        dialogueBubbleText.text = speechBubbleLine;

        characterImage.sprite = image;

        if (questionLine != null)
            questionText.text = questionLine;

        startCombatButton.onClick.AddListener(()=> sci.StartSideCombat());
    }
}
