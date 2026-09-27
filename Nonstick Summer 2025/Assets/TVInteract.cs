using UnityEngine;
using System.Collections;

public class TVInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject staticScreen;
    private Coroutine resetInteract;


    public void Interact(GameObject player)
    {
        if (resetInteract != null)
        {
            return;
        }

        resetInteract = StartCoroutine(InteractDelay());

        staticScreen.SetActive(!staticScreen.activeInHierarchy);
    }

    private IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(.1f);
        resetInteract = null;
    }
}
