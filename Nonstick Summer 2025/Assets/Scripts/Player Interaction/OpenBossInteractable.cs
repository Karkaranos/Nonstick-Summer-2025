using NaughtyAttributes;
using UnityEngine;
/*****************************************************************************
* File Name :         OpenBossInteractable.cs
* Author :            Sky
* Creation Date :     June 18, 2025
*
* Brief Description : Opens combat automatically for bosses
*
* TODO:
* 
* 
*****************************************************************************/
public class OpenBossInteractable : MonoBehaviour
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
    private characters character;

    public void OpenCanvas()
    {
        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
        var dialogueController = menu.GetComponentInChildren<DialogueUIController>();
        StartCoroutine(dialogueController.Initialize(StartingDialogueBranch, character));
    }

    private void OnDrawGizmos()
    {
        if (cameraAnchor == null)
            return;

        if (!StaticUtilities.Editor_SelectingSelfOrChild(this.transform))
            return;

        Gizmos.color = Color.blue; // blue because the unity camera icon color is blue
        Gizmos.DrawRay(cameraAnchor.position, cameraAnchor.forward);
        Gizmos.DrawWireSphere(cameraAnchor.position, 0.25f);
    }
}
