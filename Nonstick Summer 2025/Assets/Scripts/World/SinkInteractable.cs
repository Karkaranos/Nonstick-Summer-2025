/*****************************************************************************
* File Name :         SinkInteractable.cs
* Author :            Cade
* Creation Date :     8/7/2025
*
* Brief Description : Need a drink?
* 
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class SinkInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject waterSystem;
    private GameObject waterObj = null;
    private bool waterOn = false;

    private Coroutine resetInteract;

    public void Interact(GameObject player)
    {
        if (resetInteract != null)
        {
            return;
        }

        resetInteract = StartCoroutine(InteractDelay());

        if (!waterOn && waterObj == null)
        {
            waterOn = true;
            waterObj = Instantiate(waterSystem, spawnPoint);
            waterObj.transform.localPosition = Vector3.zero;
        }
        else
        {
            waterOn = false;
            Destroy(waterObj);

            //not needed but as a safeguard
            waterObj = null;
        }
    }

    private IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(.1f);
        resetInteract = null;
    }
}
