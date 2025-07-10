using NaughtyAttributes;
using UnityEngine;

public class OpenConfirmationInteractable : MonoBehaviour, IInteractable
{
    [HideInInspector] public bool InteractSuccessful = false;
    //private bool _playerHasLeft = false;
    //[HideInInspector]
    public bool BossDefeated = false;
    [SerializeField, Scene] private int _nextSceneIndex;

    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    private Transform cameraAnchor;

    public void Interact(GameObject player)
    {
        UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor, gameObject);
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

    public void GoToBed()
    {
        if (BossDefeated)
        {
            InteractSuccessful = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneIndex);
        }
        else
        {
            Debug.Log("Boss not defeated.");
        }
    }

    public void SetIsObjective(bool b = false) { }

    public void ClearBlocker()
    {
        BossDefeated = true;
    }

    public void TryBoss() { }
}
