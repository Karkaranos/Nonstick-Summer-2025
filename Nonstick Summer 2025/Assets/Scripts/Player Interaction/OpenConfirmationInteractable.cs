using NaughtyAttributes;
using UnityEngine;

/*****************************************************************************
// File Name :          OpenConfirmationInteractable.cs
// Author :             Sky
// Creation Date :      July 10, 2025
// Modified Date :      July 10, 2025
//
// Brief Description :  Controls confirmation menu popups through interaction.

*****************************************************************************/
public class OpenConfirmationInteractable : MonoBehaviour, IInteractable
{
    [HideInInspector] public bool InteractSuccessful = false;
    [HideInInspector] public bool BossDefeated = false;
    [SerializeField, Scene] public int NextSceneIndex;
    [SerializeField, Tooltip("Which object moves you between scenes.")] BedInteractionPopupCanvas.EndType sceneTransitionType;

    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    public void Interact(GameObject player)
    {
        var menu = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor, gameObject);
        menu.GetComponent<BedInteractionPopupCanvas>().Bed = this;
        menu.GetComponent<BedInteractionPopupCanvas>().SceneTransitionType = sceneTransitionType = BedInteractionPopupCanvas.EndType.BED;
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

    public void ClearBlocker()
    {
        BossDefeated = true;
    }
}
