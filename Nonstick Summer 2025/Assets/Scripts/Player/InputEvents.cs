/*
  * VERY generalized script, feel free to add or remove things as you see fit.
  */

using System.ComponentModel;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NaughtyAttributes;


public class InputEvents : Singleton<InputEvents>
{
    // Events
    [HideInInspector] public static UnityEvent MoveStarted = new UnityEvent();
    [HideInInspector] public static UnityEvent MoveHeld = new UnityEvent();
    [HideInInspector] public static UnityEvent MoveCanceled = new UnityEvent();

    /*[HideInInspector] public static UnityEvent JumpStarted = new UnityEvent();
    [HideInInspector] public static UnityEvent JumpHeld = new UnityEvent();
    [HideInInspector] public static UnityEvent JumpCanceled = new UnityEvent(); */

    [HideInInspector] public static UnityEvent PauseStarted = new UnityEvent();

    [HideInInspector] public static UnityEvent InventoryStarted = new UnityEvent();

    [HideInInspector] public static UnityEvent ClickStarted = new UnityEvent();
    [HideInInspector] public static UnityEvent ClickHeld = new UnityEvent();
    [HideInInspector] public static UnityEvent ClickCanceled = new UnityEvent();

    [HideInInspector] public static UnityEvent RightClickStarted = new UnityEvent();
    [HideInInspector] public static UnityEvent RightClickHeld = new UnityEvent();
    [HideInInspector] public static UnityEvent RightClickCanceled = new UnityEvent();

    [HideInInspector] public static UnityEvent InteractStarted = new UnityEvent();
    [HideInInspector] public static UnityEvent InteractHeld = new UnityEvent();
    [HideInInspector] public static UnityEvent InteractCanceled = new UnityEvent();

    // Input values and flags
    public static bool MovePressed, /*JumpPressed,*/ PausePressed, InventoryPressed, ClickPressed, RightClickPressed, DashPressed, InteractPressed;

    public Vector2 InputDirection => UITransitionManager.PlayerInMenu ? Vector2.zero : Move.ReadValue<Vector2>();
    public static Vector2 MousePosition => Mouse.current.position.value;
    public static Vector2 MouseDelta => UITransitionManager.PlayerInMenu ? Vector2.zero : mouseDelta.ReadValue<Vector2>();     // uses canvas space *sigh

    private PlayerInput playerInput;
    private static InputAction Move, /*Jump,*/ Pause, Inventory, LeftClick, RightClick, mouseDelta, Interact;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        InitializeActions();
    }
    
    void InitializeActions()
    {
        var map = playerInput.currentActionMap;
        Move = map.FindAction("Move");
        //Jump = map.FindAction("Jump");
        LeftClick = map.FindAction("LeftClick");
        RightClick = map.FindAction("RightClick");
        Pause = map.FindAction("Pause");
        mouseDelta = map.FindAction("MouseDelta");
        Interact = map.FindAction("Interact");
        Inventory = map.FindAction("Open Inventory");

        Move.started += ctx => ActionStarted(ref MovePressed, MoveStarted);
        //Jump.started += ctx => ActionStarted(ref JumpPressed, JumpStarted);
        LeftClick.started += ctx => ActionStarted(ref ClickPressed, ClickStarted);
        RightClick.started += ctx => ActionStarted(ref RightClickPressed, RightClickStarted);
        Pause.started += ctx => { PausePressed = true; PauseStarted?.Invoke(); };
        Inventory.started += ctx => { InventoryPressed = true; InventoryStarted?.Invoke(); };
        Interact.started += ctx => ActionStarted(ref InteractPressed, InteractStarted);

        Move.canceled += ctx => ActionCanceled(ref MovePressed, MoveCanceled);
        //Jump.canceled += ctx => ActionCanceled(ref JumpPressed, JumpCanceled);
        LeftClick.canceled += ctx => ActionCanceled(ref ClickPressed, ClickCanceled);
        RightClick.canceled += ctx => ActionCanceled(ref RightClickPressed, RightClickCanceled);
        Interact.canceled += ctx => ActionCanceled(ref InteractPressed, InteractCanceled);
    }

    void ActionStarted(ref bool pressedFlag, UnityEvent actionEvent, bool overrideInMenu=false)
    {
        if (UITransitionManager.PlayerInMenu && !overrideInMenu) return;

        pressedFlag = true;
        actionEvent?.Invoke();
    }
    void ActionCanceled(ref bool pressedFlag, UnityEvent actionEvent, bool overrideInMenu = false)
    {
        pressedFlag = false;

        if (UITransitionManager.PlayerInMenu && !overrideInMenu) return;

        actionEvent?.Invoke();
    }
    private void FixedUpdate()
    {
        if (UITransitionManager.PlayerInMenu) return;

        if (MovePressed) MoveHeld?.Invoke();
        //if (JumpPressed) JumpHeld?.Invoke();
        if (ClickPressed) ClickHeld?.Invoke();
        if(RightClickPressed) RightClickHeld?.Invoke();
        if(InteractPressed) InteractHeld?.Invoke();
    }

    private void OnDisable()
    {
        Move.RemoveAllBindingOverrides();
        LeftClick.RemoveAllBindingOverrides();
        RightClick.RemoveAllBindingOverrides();
        Pause.RemoveAllBindingOverrides();
        Interact.RemoveAllBindingOverrides();
        Inventory.RemoveAllBindingOverrides();

        /*Move.started -= ctx => ActionStarted(ref MovePressed, MoveStarted);
        //Jump.started -= ctx => ActionStarted(ref JumpPressed, JumpStarted);
        LeftClick.started -= ctx => ActionStarted(ref ClickPressed, ClickStarted);
        RightClick.started -= ctx => ActionStarted(ref RightClickPressed, RightClickStarted);
        Pause.started -= ctx => { PausePressed = true; PauseStarted?.Invoke(); };
        Interact.started -= ctx => ActionStarted(ref InteractPressed, InteractStarted);

        Move.canceled -= ctx => ActionCanceled(ref MovePressed, MoveCanceled);
        //Jump.canceled -= ctx => ActionCanceled(ref JumpPressed, JumpCanceled);
        LeftClick.canceled -= ctx => ActionStarted(ref ClickPressed, ClickStarted);
        RightClick.canceled -= ctx => ActionCanceled(ref RightClickPressed, RightClickCanceled);
        Interact.canceled -= ctx => ActionCanceled(ref InteractPressed, InteractCanceled);*/
    }
}
