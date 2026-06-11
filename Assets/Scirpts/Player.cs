using UnityEngine;
using System;
using Unity.Jobs;

// implement interface cho player có thể cầm đồ vật phòng bếp
public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPickedSomething;
    private float moveSpeed = 7f;
    private float rotateSpeed = 30f;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private float interactionDistance = 2f; // khoảng cách tới counter bắn raycast
    private Vector3 lastInteracDir; // hướng cuối cùng player nhìn tránh việc dừng lại thì hướng là moveDir 0 0 0
    // moveDirX và moveDirZ để xử lý player đi vào các góc tường vẫn đi đc 1 hg khác
    private Vector3 moverDirX = new Vector3(0, 0, 0);
    private Vector3 moveDirZ = new Vector3(0, 0, 0);
    private bool canMove = true;
    private Vector3 moveDir;
    // lấy input của người chs
    [SerializeField] private GameInput gameInput;
    // layer mask để raycast check counter
    [SerializeField] private LayerMask counterLayerMask;
    
    // counter mà player đang đứng trc để tương tác
    private BaseCounter selectedCounter;

    // biến này check xem player có đang đi hay không
    private bool isWalking;

    // event khi player thay đổi counter đứng trc để tương tác
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [Header("Kitchen Object")]
    // biến này là để lưu vật thể mà player đang cầm trên tay
    private KitChenObject kitchenObject;
    [SerializeField] private Transform KitchenObjectHoldPoint; // điểm tay cầm vp

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // đăng ký sự kiện khi player tương tác với counters
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying() == false) return;
        if (selectedCounter != null) selectedCounter.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying() == false) return;
        // nếu player đứng trước Counter thì tg tác vs nó 
        if(selectedCounter != null) selectedCounter.Interact(this);
    }

    private void Update()
    {
        // xử lý di chuyển và tương tác
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleMovement()
    {
        // lấy hướng di chuyển từ input
        moveDir = new Vector3(gameInput.GetMovementVectorNormalized().x, 0
            , gameInput.GetMovementVectorNormalized().y);

        // kiểm tra xem có thể di chuyển đc k
        canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight, playerRadius,
            moveDir, moveSpeed * Time.deltaTime);

        // nếu k thể di chuyển -> thực thi if
        if (!canMove)
        {
            // ý tưởng ở đây là nếu player k đi đc 
            // hãy lấy ý tưởng đi vào tường hướng chéo mà k đi đc
            // sẽ đổi sang hg ngang X hoặc dọc Z
            // kiểu giống trượt dài ý
            // và k có th là ấn ngang thì xét sang dọc vì có việc kt Z != 0 tức là Z p có đầu vào
            // => phải đi chéo X Z đều có đầu vào input cả X vs Z đều làm vậy
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
        // nếu di chuyển đc => di chuyển
        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = gameInput.GetMovementVectorNormalized().magnitude > 0;
        // xoay player theo hướng di chuyển
        // hàm slerp sẽ giúp xoay mượt mà hơn thay vì xoay ngay lập tức
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);
    }

    // hàm tương tác vs counter
    private void HandleInteractions()
    {
        // lấy hg
        moveDir = new Vector3(gameInput.GetMovementVectorNormalized().x, 0
            , gameInput.GetMovementVectorNormalized().y);
        
        if(moveDir != Vector3.zero)
        {
            // lấy hướng cuối cùng tránh moveDir = 0 vì player k di chuyển nữa
            lastInteracDir = moveDir;
        }

        if(Physics.Raycast(transform.position, lastInteracDir, out RaycastHit raycast
            , interactionDistance, counterLayerMask))
        {
            if(raycast.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // bắn ra raycast kt nếu có BaseCounter tức là counter
                if(baseCounter != selectedCounter)
                {
                    // gọi hàm select vs counter đó
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
        // lưu lại counter đang  select
        this.selectedCounter = selectedCounter;
        // Invoke event khi counter thay đổi
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
    // set vật phẩm player đang cầm trên tay
    public void SetKitchenObject(KitChenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    // lấy ra vp đang cầm
    public KitChenObject GetKitchenObject() => kitchenObject;
    // xóa vật phẩm đang cầm
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}
