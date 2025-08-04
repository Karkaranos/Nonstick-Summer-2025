/*
 * reading and applying mouse input in update/fixedupdate respectively
 * acutally really makes the camera feel smoother.
 * Source (bro trust me)
 */

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float Sensitivity = 10f;
    public float Slippiness = 10;
    [SerializeField][Required]
    public Camera playerCamera;

    [HideInInspector] public Transform camTransform => playerCamera.transform;
    float xLook, yLook;

    private void Start()
    {
        if(playerCamera == null) 
            playerCamera = Camera.main; // taking a shot in the dark with this one

        //StaticUtilities.DisableCursor();

        Vector3 startRotation = camTransform.localRotation.eulerAngles;
        xLook = startRotation.x;
        yLook = startRotation.y;

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("1"))
        {
            GameManager.Sensitivity = Sensitivity;
        }
        else
        {
            Sensitivity = GameManager.Sensitivity;
        }
    }

    private void Update()
    {
        // Read mouse input
        var mouse = InputEvents.MouseDelta;//.normalized;
        //float mouseX = mouse.x * Time.fixedDeltaTime * Sensitivity;
        //float mouseY = mouse.y * Time.fixedDeltaTime * Sensitivity;
        yLook += mouse.x * Sensitivity;
        xLook -= mouse.y * Sensitivity;
        xLook = Mathf.Clamp(xLook, -85f, 85f);
    }

    private void FixedUpdate()
    {
        // Apply mouse input
        var target = Quaternion.Euler(xLook, yLook, 0);
        var smoothed = Quaternion.Slerp(camTransform.localRotation, target, Time.fixedDeltaTime * Slippiness);
        camTransform.localRotation = smoothed;
    }

    public void UpdateSensitivity(float val)
    {
        Sensitivity = val;
        GameManager.Sensitivity = Sensitivity;
    }
}
