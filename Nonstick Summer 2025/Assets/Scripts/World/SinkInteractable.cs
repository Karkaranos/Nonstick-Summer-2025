/*****************************************************************************
* File Name :         SinkInteractable.cs
* Author :            Cade
* Creation Date :     8/7/2025
*
* Brief Description : Need a drink?
* 
*****************************************************************************/
using UnityEngine;

public class SinkInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject waterSystem;
    private GameObject waterObj = null;
    private bool waterOn = false;

    public void Interact(GameObject player)
    {
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
}
