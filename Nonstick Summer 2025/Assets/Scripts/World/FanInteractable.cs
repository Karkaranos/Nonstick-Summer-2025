/*****************************************************************************
* File Name :         FanInteractable.cs
* Author :            Toby
* Creation Date :     8/7/2025
*
* Brief Description : Cools down the room
* 
*****************************************************************************/

using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class FanInteractable : MonoBehaviour, IInteractable
{
    [SerializeField, Required] private Transform fanBlades;
    [SerializeField] private float rotationSpeed = 180;
    [SerializeField] private float fanAccelerationTime = 2.5f;

    private bool fanActive = false;
    private Coroutine fanAnimation;
    private float fanSpeedScalar = 0;
    private float targetFanSpeedScalar => fanActive ? 1 : 0;
    public void Interact(GameObject player)
    {
        // Turn on fan
        if(!fanActive && fanAnimation == null)
        {
            fanAnimation = StartCoroutine(FanAnimation());
        }

        // this updates targetFanSpeedScalar, which stops the coroutine, eventually
        fanActive = !fanActive;
    }

    private IEnumerator FanAnimation()
    {
        Debug.Log("fan started");
        Vector3 rotation = new Vector3(0, rotationSpeed, 0);
        do
        {
            // accelerate
            fanSpeedScalar = Mathf.MoveTowards(fanSpeedScalar, targetFanSpeedScalar, Time.fixedDeltaTime / fanAccelerationTime);

            fanBlades.Rotate(rotation * fanSpeedScalar * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate(); // fixed update for maximum fan quality
        }
        while (!Mathf.Approximately(fanSpeedScalar, 0) || fanActive);
        Debug.Log("fan ended");
        fanAnimation = null;
    }
}
