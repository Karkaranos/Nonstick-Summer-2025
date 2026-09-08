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

    private Coroutine resetInteract = null;
    private Coroutine slowing = null;

    ParticleSystem[] childSystems = new ParticleSystem[2];


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
            childSystems = waterObj.GetComponentsInChildren<ParticleSystem>();
            waterObj.transform.localPosition = Vector3.zero;
        }
        else
        {
            if(slowing != null)
            {
                StopCoroutine(slowing);
                slowing = null;
                waterObj.GetComponent<ParticleSystem>().loop = true;
                foreach(ParticleSystem p in childSystems)
                {
                    p.loop = true;
                }
                return;

            }

            waterObj.GetComponent<ParticleSystem>().loop = false;
            foreach (ParticleSystem p in childSystems)
            {
                p.loop = false;
            }

            slowing = StartCoroutine(StopWater());
        }
    }

    private IEnumerator StopWater()
    {
        yield return new WaitForSeconds(1.5f);

        waterOn = false;
        Destroy(waterObj);

        //not needed but as a safeguard
        waterObj = null;
        slowing = null;
    }

    private IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(.1f);
        resetInteract = null;
    }
}
