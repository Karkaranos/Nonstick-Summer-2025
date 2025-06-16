using NaughtyAttributes;
using UnityEngine;

public class OpenCanvasInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    private GameObject openedCanvas;
    private bool hasGivenCard = false;

    public void Interact(GameObject player)
    {
        openedCanvas = UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor, gameObject);

        print(openedCanvas.transform.childCount);
        // If this object has given a card, set the card button to false
        if (hasGivenCard)
        {
            openedCanvas.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            openedCanvas.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);

        }
    }

    public void GiveCard()
    {
        hasGivenCard = true;
    }

#if UNITY_EDITOR
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
#endif
}
