/******************************************************************
*    Author: Marissa Moser
*    Contributors: Sky Beal
*    Date Created: May 24, 2024
*    Description: 
*       Contains a function called when the player presses E. This function will:
            Use a raycast to check if there is something in from of them
            to Interact with and if it is interactable. If so, call the
            Interact(player) function on that game object

*******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] [ReadOnly]
    private GameObject _targetGameObj;
    private IInteractable _interactable;
    private bool _canInteract;

    [SerializeField] private GameObject _pickupAnchor;
    private GameObject _inHandObject;
    private bool _isHolding;

    //raycast variables
    private RaycastHit _colliderHit;
    [SerializeField] private float _maxInteractDistance;
    [SerializeField] LayerMask _layerToIgnore;


    private void Awake()
    {
        //listen for inputs
        InputEvents.InteractStarted.AddListener(InteractPressed);
        InputEvents.InteractCanceled.AddListener(InteractReleased);

        _camera = Camera.main;

        StartDetectingInteractions();
    }

    /// <summary>
    /// Called when Interact input is started. Calls Interact() on the detected
    /// interactable game object
    /// </summary>
    private void InteractPressed()
    {
        if(_interactable != null)
        {
            _interactable.Interact(gameObject);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.InteractSFX);

        }
    }

    /// <summary>
    /// Starts the Detect Interactable coroutine
    /// </summary>
    public void StartDetectingInteractions()
    {
        _canInteract = true;
        StartCoroutine(DetectInteractable());
    }

    /// <summary>
    /// Ends the Detect Interactable coroutine
    /// </summary>
    public void StopDetectingInteractions()
    {
        _canInteract = false;
    }

    /// <summary>
    /// A coroutine that detects if there is an interactable object in front of
    /// the player using a raycast. This coroutine can be stopped with the public 
    /// Start/StopDetectingInteraction function
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetectInteractable()
    {
        while(_canInteract)
        {
            //Casts Raycast in the center of the screen
            Ray r = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(r, out _colliderHit, _maxInteractDistance, ~_layerToIgnore))
            {
                _targetGameObj = _colliderHit.transform.gameObject;

                //sets the _interactable variable for the InteractPressed function
                if (_targetGameObj.TryGetComponent(out IInteractable interactable))
                {
                    _interactable = _targetGameObj.GetComponent<IInteractable>();
                    CanvasInteractionBehavior.ShowInteractUI?.Invoke();

                    //if each object needs their own prompt use this
                    //_interactable.DisplayInteractUI();
                }
                else if (_interactable != null)
                {
                    CanvasInteractionBehavior.HideInteractUI?.Invoke();

                    //if each object needs their own prompt use this
                    //_interactable.HideInteractUI();

                    _interactable = null;
                }
            }
            //resets the variables if the player backs away from interactable
            else if (_interactable != null)
            {
                _targetGameObj = null;

                CanvasInteractionBehavior.HideInteractUI?.Invoke();

                //if each object needs their own prompt use this
                //_interactable.HideInteractUI();

                _interactable = null;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Called when Interact input is canceled. Calls CancelInteract() on the
    /// detected interactable game object.
    /// </summary>
    /// <param name="obj"></param>
    private void InteractReleased()
    {
        if (_interactable != null)
        {
            _interactable.CancelInteract();
        }
    }
    
}
