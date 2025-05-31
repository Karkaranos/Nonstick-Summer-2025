using NaughtyAttributes;
using UnityEngine;

public class OpenCanvasInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    public GameObject CanvasToOpenPrefab;

    [Tooltip("Can be left null if you don't want the camera to move.")]
    [SerializeField]
    [Required("Leaving this null means player camera wont lock onto an object (which is okay)")]
    private Transform cameraAnchor;

    public void Interact(GameObject player)
    {
        UITransitionManager.OpenMenu(CanvasToOpenPrefab, cameraAnchor);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
