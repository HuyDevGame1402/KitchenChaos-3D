using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    private float moveSpeed = 7f;
    private float rotateSpeed = 30f;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private float interactionDistance = 2f;
    private Vector3 lastInteracDir;
    private Vector3 moverDirX = new Vector3(0, 0, 0);
    private Vector3 moveDirZ = new Vector3(0, 0, 0);
    private bool canMove = true;
    private Vector3 moveDir;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    private BaseCounter selectedCounter;

    private bool isWalking;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [Header("Kitchen Object")]
    private KitChenObject kitchenObject;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null) selectedCounter.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null) selectedCounter.Interact(this);
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
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight, playerRadius,
            moverDirX, moveSpeed * Time.deltaTime);
            if (canMove)
            {
                moveDir = moverDirX;
            }
            else
            {
                moveDirZ.z = moveDir.z;
                canMove = moveDirZ.z != 0 && !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight, playerRadius,
                    moveDirZ, moveSpeed * Time.deltaTime);
                if (canMove)
                {
                    moveDir = moveDirZ;
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
            if(raycast.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //clearCounter.Interact();
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
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
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,
                        new OnSelectedCounterChangedEventArgs
                        {
                            selectedCounter = selectedCounter
                        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitChenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
    public KitChenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}
