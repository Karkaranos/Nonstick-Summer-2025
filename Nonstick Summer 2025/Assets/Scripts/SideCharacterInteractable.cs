/*************************************************
Author Names :          Cade Naylor
Date Created :          June 23, 2025
Date Modified :         June 24, 2025
Brief Description :     Opens shorter combat for Side Character upon interaction
***************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SideCharacterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField, Required, Tooltip("Asks the player if they want to enter combat")] private GameObject promptCanvas;
    [SerializeField, Tooltip("Question phrasing")] private string combatPrompt;
    [SerializeField, Tooltip("The displayed line of dialogue before entering combat")] private string preCombatLine;
    [SerializeField, Tooltip("The displayed line of dialogue after completing combat")] private string postCombatLine;
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
    private Character character;

    private GameObject openedCanvas;


    public ModifierData[] PossibleModifiers;

    private bool conversationComplete = false;

    [SerializeField, Required] private GameObject ModifierObtainCanvas;



    /// <summary>
    /// Opens prompt canvas and loads desired data
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        GameManager.ObjectiveReference.SetObjectiveVisibility(false);

        openedCanvas = UITransitionManager.OpenMenu(promptCanvas, cameraAnchor, gameObject);
        openedCanvas.GetComponent<CombatPromptCanvas>().Initialize(
            (conversationComplete ? postCombatLine : preCombatLine), conversationComplete, 
            characterSprite, this, (conversationComplete ? null : combatPrompt)
            );
    }

    public void StartSideCombat()
    {
        if (SceneManager.GetActiveScene().name.Equals("Moment_5"))
        {
            MusicManager.instance.StartReflection();
        }
        else
        {
            MusicManager.instance.StartCombat(0);
        }

        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
        var dialogueController = menu.GetComponentInChildren<DialogueUIController>();
        GameManager.ObjectiveReference.SetObjectiveVisibility(false);
        StartCoroutine(dialogueController.Initialize(StartingDialogueBranch, character, false, gameObject));
        StaticUtilities.EnableCursor();
    }

    public void FinishSideCombat()
    {
        conversationComplete = true;
        int i = PossibleModifiers.Length;
        var modifier = PossibleModifiers[(int)Random.Range(int.MinValue, int.MaxValue) % i];
        ModifierManager.AddCard(modifier);
        var iopc = UITransitionManager.OpenMenu(ModifierObtainCanvas, cameraAnchor, null).GetComponent<ItemObtainPopupCanvas>();
        iopc.Initialize(modifier);

    }
}
