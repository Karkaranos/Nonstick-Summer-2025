using UnityEngine;

public class TVInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject staticScreen;
    public void Interact(GameObject player)
    {
        staticScreen.SetActive(!staticScreen.activeInHierarchy);
    }
}
