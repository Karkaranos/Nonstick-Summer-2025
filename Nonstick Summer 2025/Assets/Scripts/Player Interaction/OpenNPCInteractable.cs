using NaughtyAttributes;
using UnityEngine;

public class OpenNPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [SerializeField]
    [Required]
    private DialogueBranch StartingDialogeBranch;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    public void Interact(GameObject player)
    {
        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
        var dialogueController = menu.GetComponentInChildren<DialougeUIController>();
        dialogueController.Initialize(StartingDialogeBranch);
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
