using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{
    public event EventHandler OnInteractAction;
    public event EventHandler OnStealthAction;
    public event EventHandler OnPauseAction;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Event_InteractPerformed;
        playerInputActions.Player.Stealth.performed += Event_StealthPerformed;
        playerInputActions.Player.Pause.performed += ctx => OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Event_InteractPerformed(InputAction.CallbackContext obj) => OnInteractAction?.Invoke(this, EventArgs.Empty);
    private void Event_StealthPerformed(InputAction.CallbackContext obj) => OnStealthAction?.Invoke(this, EventArgs.Empty);
    private void Event_PausePerformed(InputAction.CallbackContext obj) => OnPauseAction?.Invoke(this, EventArgs.Empty);

    public Vector2 GetInputVector()
    {
        Vector2 input = playerInputActions.Player.Move.ReadValue<Vector2>();
        return input;
    }

    public Vector2 GetInputVectorNormalized()
    {
        Vector2 input = playerInputActions.Player.Move.ReadValue<Vector2>();
        input = input.normalized;
        return input;
    }
}
