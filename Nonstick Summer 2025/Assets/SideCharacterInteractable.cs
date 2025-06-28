/*************************************************
Author Names :          Cade Naylor
Date Created :          June 23, 2025
Date Modified :         June 24, 2025
Brief Description :     Opens shorter combat for Side Characters upon interaction
***************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideCharacterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField, Required, Tooltip("Asks the player if they want to enter combat")] private GameObject promptCanvas;
    [SerializeField, Tooltip("Question phrasing")] private string combatPrompt;
    [SerializeField, Tooltip("The displayed line of dialogue before entering combat")] private string preCombatLine;
    [SerializeField] private Sprite characterSprite;

    [SerializeField, Tooltip("Dialogue Canvas")]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [SerializeField]
    [Required]
    private DialogueBranch StartingDialogueBranch;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    [Tooltip("Current character you're interacting with.")]
    [SerializeField]
    private characters character;

    private GameObject openedCanvas;


    public ModifierData[] PossibleModifiers;


    /// <summary>
    /// Opens prompt canvas and loads desired data
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        GameManager.ObjectiveReference.SetObjectiveVisibility(false);

        openedCanvas = UITransitionManager.OpenMenu(promptCanvas, cameraAnchor, gameObject);

        openedCanvas.transform.GetChild(1).GetComponent<TMP_Text>().text = combatPrompt;
        openedCanvas.transform.GetChild(4).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = preCombatLine;
        openedCanvas.transform.GetChild(3).GetChild(0).GetComponent<Image>().sprite = characterSprite;
        openedCanvas.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() => StartSideCombat());
    }

    public void StartSideCombat()
    {
        print("Cah");
        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
        var dialogueController = menu.GetComponentInChildren<DialogueUIController>();
        GameManager.ObjectiveReference.SetObjectiveVisibility(false);
        StartCoroutine(dialogueController.Initialize(StartingDialogueBranch, character, false, gameObject));
    }

    public void GetModifier()
    {
        int i = PossibleModifiers.Length;
        ModifierManager.AddCard(PossibleModifiers[(int)Random.Range(int.MinValue, int.MaxValue) % i]);
    }
}
