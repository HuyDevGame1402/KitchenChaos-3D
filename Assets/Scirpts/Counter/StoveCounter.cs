using UnityEngine;
using System;

// Counter nấu nướng thịt 
// kế thừa từ basecounter để sử dụng các hàm vs thông số chung của counterbase
// implement interface cho bt đây là counter có thể thay đổi trạng thái khi hoạt động trong game

public class StoveCounter : BaseCounter, IHasProgress
{
    // định nghĩa lại event 
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    // khởi tạo 1 event thay đổi trạng thái state ví dụ từ idle -> bđ nướng -> nướng quá cháy -> nướng cháy -> ....
    // có tham số đầu vào của event là trạng thái thay đổi
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    
    // khởi tạo class để chứa hay định nghĩa tham số đầu vào
    public class OnStateChangedEventArgs : EventArgs
    {
        // chứa state của counter
        public State state;
    }
    // các enum của state counter khi hoạt động
    public enum State
    {
        Idle, // bình thg k có thịt để nướng
        Frying, // nướng thịt
        Fried, // nướng nhg trong quá trình đang cháy qua quá trình nướng đầu r
        Burned, // thịt đã cháy
    }

    // khởi tạo 1 mảng array chứa các dữ liệu đầu vào và đầu ra của đồ chiên
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    // khởi tạo 1 mảng array chứa các trường dữ liệu đầu vào và đầu ra của đồ chiên đã bị cháy
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state; // state của counter biến lưu lại state của counter trong game
    private float fryingTimer; // bộ đếm thời gian chiên
    private float burningTimer; // bộ đếm thời gian khi bị cháy
    private FryingRecipeSO fryingRecipeSO; // biến lưu lại đồ đang chiên trên counter
    private BurningRecipeSO burningRecipeSO; // biến lưu lại đồ đang cháy trên counter

    // khởi tạo trạng thái idle trong state
    private void Start()
    {
        state = State.Idle;
    }
    // hàm loop update trong game hoạt động
    private void Update()
    {
        // counter stove chỉ hoạt động khi nó có đồ kitchenobject bên trên nó
        if (HasKitchenObject())
        {
            // switch case các state của counter hoạt động
            switch (state)
            {
                // idle thì break k cần thực thi các lệnh
                case State.Idle:
                    break;
                // chuyển sang trạng thái nướng khi thịt đã chín                
                case State.Fried:
                    // cộng thời gian cháy thịt lên
                    burningTimer += Time.deltaTime;
                    // tương tự gọi hàm để bt thời gian cháy thịt đang là bn
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
                    });
                    // nếu tg cháy thịt > thời gian max cháy thịt
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // xóa kitchenobject thịt chín tới đi
                        GetKitchenObject().DestroySelf();
                        // spawn ra thịt cháy
                        KitChenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned; // chuyển state cháy thịt
                        // Invoke gọi thay đổi state
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        // Invoke thời gian nấu về 0 vì nấu xong r
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                // nếu vào trạng thái frying đang chiên thì chạy các lệnh thực thi bên trong
                case State.Frying:
                    // cộng dần thời gian bộ đếm lên (timer chiên đồ vật)
                    fryingTimer += Time.deltaTime;
                    // gọi event thay đổi thời gian ( cho ui bar để chạy xem thời gian chiên đến đâu r giống heath bar của máu)
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        // khởi tạo tham số truyền vào là % của tg hiện tại vs tg max xem đc chiên đc bn % rồi
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    // nếu thời gian > tg chiên max thực thi bên trong if
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // xóa cái kitchenobject hiện tại đi (thịt chín tới)
                        GetKitchenObject().DestroySelf();
                        // spawn ra cái thịt bị chín tới
                        KitChenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        // đổi sang trái thái chiên cháy
                        state = State.Fried;
                        burningTimer = 0f; // đặt lại tg chiên cháy là 0f
                        // lấy ra burning cái thịt cháy tương ứng từ thịt chín tới vừa ms tạo ở trên
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitChenObjectSO());
                        // gọi event invoke thay đổi trạng thái state của counter
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                    }
                    break;
                // khi thịt cháy r thì break k làm việc gì cả
                case State.Burned:
                    break;
            }
        }
    }
    // hàm chạy khi tương tác phím e
    public override void Interact(Player player)
    {
        // nếu k có kitchenobject nó sở hữu
        if (!HasKitchenObject())
        {
            // và player p có kitchenobject
            if (player.HasKitchenObject())
            {
                // kiểm tra tiếp xem kitchenobject của player có p đồ có thể nấu k vì có đồ k thể nấu
                if (HasRecipeWithInput(player.GetKitchenObject()
                    .GetKitChenObjectSO()))
                {
                    // thực thi các lệnh nếu kitchenobject của player là thịt có thể nấu
                    // set lại thịt của player sang counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    // lấy ra thịt chín tới từ đầu vào
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject()
                        .GetKitChenObjectSO());
                    // chuyển sang trạng thái nướng thịt
                    state = State.Frying;
                    fryingTimer = 0f; // khởi tạo tg nướng là 0
                    // gọi event thay đổi trạng thái vs tg nướng
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        }
        // ngược lại nếu counter đang có thịt bên trên
        else
        {
            // nếu player có kitchenobject
            if (player.HasKitchenObject())
            {
                // nhg chỉ đc cầm đĩa vì thịt để trên đĩa chứ cầm quả cà chua thì k thể cầm thịt
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // khi mà kt và lấy đc ra đĩa của player thì thêm thịt vào đĩa
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitChenObjectSO()))
                    {
                        // nếu thêm tc thì xóa thịt đi 
                        GetKitchenObject().DestroySelf();
                        // chuyển lại sang idle
                        state = State.Idle;
                        // gọi Invoke thay đổi state vs thay đổi tg nướng về 0
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            // ngược lại nếu player k có cầm đồ
            else
            {
                // xét lại parent để player cầm thịt
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle; // chuyển sang idle và gọi Invoke thay đổi trạng thái vs tg nướng
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    // kiểm tra xem đầu vào có thể chiên được không
    private bool HasRecipeWithInput(KitChenObjectSO inputKitchenObjectSO)
    {
        // sẽ lấy ra dữ liệu của đồ chiên vs đầu vào là thịt sống (tương tự vs các dữ liệu khác)
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null; // trả về true nếu có còn false nếu không 
    }
    // Lấy ra output kitchenobject từ input đầu vào của dữ liệu thịt nướng
    private KitChenObjectSO GetOutputForInput(KitChenObjectSO inputKitchenObjectSO)
    {
        // tương tự lấy ra thịt chiên và trả về
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    // hàm thực thi logic lấy ra thịt chiên
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitChenObjectSO inputKitchenObjectSO)
    {
        // duyệt qua mảng array thịt chiên data
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            // kiểm tra đầu vào của thịt chiên đó ==  vs đầu vào mà chúng ta truyền thì trả về dữ liệu
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    // tương tự vs thịt cháy
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitChenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }   
    // để xem trạng thái bếp đã nấu chín chưa ?
    public bool IsFried()
    {
        return state == State.Fried;    
    }
}
