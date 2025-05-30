/*
 * Handles regular player movement (WASD)
 * 
 * - Clare Grady, Toby S, Tyler B, Sky B, Alec P
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration=10;

    private Rigidbody rb;
    private Transform playerOrientationTracker;

    private PlayerCamera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = FindFirstObjectByType<PlayerCamera>();
        playerOrientationTracker = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        Debug.Log(InputEvents.Instance.InputDirection);
        var direction = StaticUtilities.TransformInputDirection(InputEvents.Instance.InputDirection, playerCamera.camTransform);

        //rb.AddForce(direction * speed, ForceMode.Force); we arent that kinda game, kiddo.
        direction.y = 0;
        var newvel = direction.normalized * speed;
        newvel = Vector3.Lerp(rb.linearVelocity, newvel, Time.deltaTime * acceleration);
        newvel.y = rb.linearVelocity.y; // maintain gravity



        rb.linearVelocity = newvel;
    }
}
