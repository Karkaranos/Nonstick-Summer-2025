using UnityEngine;
using NaughtyAttributes;

public class OpenPause : MonoBehaviour
{
    [SerializeField, Required] public GameObject PauseMenu;

    /// <summary>
    /// Occurs at the first frame. Initializes events
    /// </summary>
    private void Start()
    {
        InputEvents.PauseStarted.AddListener(PausePressed);
    }

    /// <summary>
    /// Handles opening or closing the pause menu 
    /// </summary>
    public void PausePressed()
    {
        Debug.Log("Called");
        if (UITransitionManager.PlayerInMenu &&
            UITransitionManager.CurrentCanvasReference.GetComponent<PauseAndSettings>() != null)
        {
            UITransitionManager.CloseMenu();
        }
        // Prevents the player from pausing while in combay. can be revisited later
        else if (UITransitionManager.CurrentCanvasReference != null &&
            UITransitionManager.CurrentCanvasReference.GetComponent<DialogueUIController>())
        {
            Debug.LogWarning("Pausing is prohibited during combat");
        }
        else
        {
            UITransitionManager.OpenMenu(PauseMenu);
        }
    }
}
