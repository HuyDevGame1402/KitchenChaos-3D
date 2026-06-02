using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    private Vector2 inputVector;
    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performend;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performend;
    }
    private void Interact_performend(UnityEngine.InputSystem.InputAction.CallbackContext
        obj)
    {  
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void InteractAlternate_performend(UnityEngine.InputSystem.InputAction.CallbackContext
        obj)
    {  
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized()
    {
        inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
