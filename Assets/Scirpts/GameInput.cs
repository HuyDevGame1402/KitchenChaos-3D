using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private Vector2 inputVector;
    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performend;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performend;
        playerInputAction.Player.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_performend;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performend;
        playerInputAction.Player.Pause.performed -= Pause_performed;
        playerInputAction.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
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
