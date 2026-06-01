using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private float moveSpeed = 7f;
    private float rotateSpeed = 10f;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private float interactionDistance = 2f;
    private Vector3 lastInteracDir;
    private Vector3 moverDirX = new Vector3(0, 0, 0);
    private Vector3 moverDirZ = new Vector3(0, 0, 0);
    private bool canMove = true;
    private Vector3 moveDir;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    private ClearCounter selectedCounter;

    private bool isWalking;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null) selectedCounter.Interact();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleMovement()
    {
        moveDir = new Vector3(gameInput.GetMovementVectorNormalized().x, 0
            , gameInput.GetMovementVectorNormalized().y);

        canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight, playerRadius,
            moveDir, moveSpeed * Time.deltaTime);

        if (!canMove)
        {
            moverDirX.x = moveDir.x;
            canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight, playerRadius,
            moverDirX, moveSpeed * Time.deltaTime);
            if (canMove)
            {
                moveDir = moverDirX;
            }
            else
            {
                moverDirZ.z = moveDir.z;
                canMove = !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight, playerRadius,
                    moverDirZ, moveSpeed * Time.deltaTime);
                if (canMove)
                {
                    moveDir = moverDirZ;
                }
                else
                {

                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = gameInput.GetMovementVectorNormalized().magnitude > 0;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);
    }

    private void HandleInteractions()
    {
        moveDir = new Vector3(gameInput.GetMovementVectorNormalized().x, 0
            , gameInput.GetMovementVectorNormalized().y);
        
        if(moveDir != Vector3.zero)
        {
            lastInteracDir = moveDir;
        }

        if(Physics.Raycast(transform.position, lastInteracDir, out RaycastHit raycast
            , interactionDistance, counterLayerMask))
        {
            if(raycast.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //clearCounter.Interact();
                if(clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,
                        new OnSelectedCounterChangedEventArgs
                        {
                            selectedCounter = selectedCounter
                        });
    }
}
