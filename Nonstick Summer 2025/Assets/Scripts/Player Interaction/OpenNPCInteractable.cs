using NaughtyAttributes;
using UnityEngine;
/*****************************************************************************
* File Name :         OpenNPCInteractable.cs
* Author :            Toby
* Creation Date :     June 6, 2025
*
* Brief Description : Listens for player interaction, opens combat on interact
*
* TODO:
* 
* 
*****************************************************************************/
public class OpenNPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
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

    public void Interact(GameObject player)
    {
        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
        var dialogueController = menu.GetComponentInChildren<DialogueUIController>();
        StartCoroutine(dialogueController.Initialize(StartingDialogueBranch, character));

        if(character == Character.Uncle)
            PersistentGameplayData.Instance.PlayerTalkedToUncle = true;
    }

    private void OnDrawGizmos()
    {
        if (cameraAnchor == null)
            return;

        if (!StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            return;

        Gizmos.color = Color.blue; // blue becuase the unity camera icon color is blue
        Gizmos.DrawRay(cameraAnchor.position, cameraAnchor.forward);
        Gizmos.DrawWireSphere(cameraAnchor.position, 0.25f);
    }
}
