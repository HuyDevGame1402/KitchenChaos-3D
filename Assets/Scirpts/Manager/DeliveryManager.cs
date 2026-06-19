using UnityEngine;
using System.Collections.Generic;
using System;


public class DeliveryManager : MonoBehaviour
{
    // sự kiện sinh công thức nấu ăn ms
    public event EventHandler OnRecipeSpawned;
    // sự kiện khi mà 1 công thức hoàn thiện
    // và đc xóa khỏi ds vì đã hoàn thiện món ăn vs công thức đó r
    public event EventHandler OnRecipeCompleted;
    // event khi giao đúng món
    public event EventHandler OnRecipeSuccess;
    // event khi món ăn giao bị sai
    public event EventHandler OnRecipeFailed;
    // singleton design pattern
    public static DeliveryManager Instance { get; private set; }
    // chứa ds công thức sẽ xuất hiện trong game
    [SerializeField] private RecipeListSO recipeListSO;
    // chứa ds các công thức món ăn mà khách yêu cầu
    private List<RecipeSO> waitingRecipeSOList;
    // biến đếm thời gian để spawn ra công thức
    private float spawnRecipeTimer;
    // thời gian tối đa spawn ra công thức
    private float spawnRecipeTimerMax = 4f;
    // giới hạn công thức đặt món số lượng là 4
    private int waitingRecipeMax = 4;
    // số món ăn giao thành công
    private int successfulRecipeAmount;

    // hàm awake để khởi tạo list công thức cần làm đồng thời tạo singleton base
    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();
        Instance = this;
    }

    private void Update()
    {
        // chạy trừ dần thời gian spawn công thức
        spawnRecipeTimer -= Time.deltaTime;
        // nếu thời gian nhỏ <= 0 thì thực thi if
        // cũng đồng nghĩa ban đầu đặt thời gian spawn = tg spawn max luôn vì có đk game phải đang playing
        if(spawnRecipeTimer <= 0f)
        {
            // đặt luôn tg max vào tg spawn
            spawnRecipeTimer = spawnRecipeTimerMax;
            // chỉ thực thi khi mà game đang ở chế độ Playing và số lượng công thức chờ phải ít hơn số lương ct chờ max
            if( GameManager.Instance.IsGamePlaying() && 
                waitingRecipeSOList.Count < waitingRecipeMax)
            {
                // lấy ra công thức random trong ds chứa tất cả công thức
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0,
                    recipeListSO.recipeSOList.Count)];
                // add công thức mon ăn vào ds các công thức đang chờ
                waitingRecipeSOList.Add(waitingRecipeSO);
                // Invoke sự kiện spawn công thức chờ thường để gọi sự kiện tạo UI công thức món ăn
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    // hàm giao đồ ăn thường được gọi khi player giao món ăn
    // tham số truyền vào là đĩa món ăn đó
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        // duyệt qua ds các công thức đang chờ
        for(int i= 0;i < waitingRecipeSOList.Count;i++)
        {
            // lấy ra công thức tại vị trí i trong ds công thức chờ
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            // kiểm tra nếu công thức lấy ra hiện tại có số lượng thành phần = vs thành phần trong món giao thì thực thi tiếp
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // biến đầu tiên mặt định là đúng món
                bool plateContentsMatchesRecipe = true;
                // duyệt qua các thành phần trong công thức đó
                foreach(KitChenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // mặc định là chưa tìm thấy
                    bool ingredientFound = false;
                    // tiếp tục duyệt qua các thành phần trong món giao
                    foreach(KitChenObjectSO plateKitchenObjectSO in 
                        plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // nếu 2 cái bằng nhau
                        if(/*plateKitchenObject*/plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // chuyển đã tìm thấy
                            ingredientFound = true;
                            // break thoát khỏi foreach này
                            break;
                        }
                    }
                    // nếu k tìm thấy cái đầu tiên thì cũng tức là k đúng công thức này chuyển sang công thức khác
                    if (!ingredientFound)
                    {
                        // chuyển là false chưa tìm thấy
                        // nên nhớ ingredientFound = false ở mỗi lần duyệt qua các thành phần trong công thức waiting ở cái foreach trên cùng
                        // nên do đó ở lần 1 ví dụ tìm thấy thì plateContentsMatchesRecipe = true;
                        // nhg khi false thì nó sẽ đổi lại false
                        // nên nhớ sau khi đổi plateContentsMatchesRecipe = false r thì luôn là false cả thể ingredientFound = true lại cũng k thể đổi 
                        // plateContentsMatchesRecipe = true đc nữa đúng logic
                        plateContentsMatchesRecipe = false;
                    }
                }
                // nếu true tìm đc món
                if (plateContentsMatchesRecipe)
                {
                    // tăng số lượng món đã giao lên
                    successfulRecipeAmount++;
                    // xóa công thức đi
                    waitingRecipeSOList.RemoveAt(i);
                    // gọi Invoke sự kiện hoàn thiện công thức món ăn
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    // gọi sự kiện giao món xong
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    // return k thực thi
                    return;
                }
            }
        }
        // ngược lại nếu ở kia false hết và k thực thi lệnh gia xong thì tức là fail món ăn
        // Invoke sự kiện giao món thất bại
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    // hàm trả về ds công thức đang đợi để đc giao đồ trong game
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    // hàm trả về số lượng các món đã hoàn thiện
    public int GetSuccessfulRecipeAmount()
    {
        return successfulRecipeAmount; 
    }
}
