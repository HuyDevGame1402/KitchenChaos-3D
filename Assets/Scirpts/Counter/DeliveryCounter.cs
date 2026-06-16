using UnityEngine;

// bàn quầy giao món ăn
public class DeliveryCounter : BaseCounter
{
    // sử dụng singleton pattern
    public static DeliveryCounter Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    // hàm tương tác khi ấn phím e
    public override void Interact(Player player)
    {
        // nếu player có kitchenobject thì ms cho giao hàng
        if (player.HasKitchenObject())
        {
            // và đó phải là dạng cái đĩa 
            // tại cái đĩa chứa thức ăn nên ms cho phép giao hàng
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // nếu lấy ra đc cái đĩa thì gọi bên trong này
                // gửi cho delivery manager sử lý đơn thức ăn của chúng ta giao
                // có thể success có thể failed vì thực đơn k có món đó
                // còn success thì nghĩa là đúng thực đơn r 
                // delivery manager sẽ phụ trách xử lý việc đó
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                // khi giao đồ ăn xong thì xóa đi cái kitchenobject đĩa của player đang cầm
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
