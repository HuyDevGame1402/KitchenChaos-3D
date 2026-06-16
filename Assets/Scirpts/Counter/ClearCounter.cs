using UnityEngine;

public class ClearCounter : BaseCounter
{
    // chứa dữ liệu của kitchenobject prefab icon name
    [SerializeField] private KitChenObjectSO kitchenObjectSO;

    // override lại hàm tương tác phím e
    public override void Interact(Player player)
    {
        // nếu k có kitchenobject thì chạy if
        // hay còn gọi là bàn trống
        if (!HasKitchenObject())
        {
            // nếu player có object 
            // tức là cần đặt object đó vào bàn
            if (player.HasKitchenObject())
            {
                // lấy kitchenobject đó vào bàn và xóa kitchenobject đó khỏi player
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        // ngược lại nếu bàn đã có kitchenobject rồi
        else
        {
            // mà player cũng có kitchenobject
            if (player.HasKitchenObject())
            {
                // kiểm tra xem player có đang cầm đĩa k
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // nếu có cầm đĩa
                    // hãy thử thêm nguyên liệu vào đĩa
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitChenObjectSO()))
                    {
                        // nếu thêm thành công thì xóa kitchen object ở counter bàn đi
                        GetKitchenObject().DestroySelf();
                    }
                }
                // nếu player k cầm đĩa
                else
                {
                    // mà counter lại chứa đĩa vì đĩa để lên bàn counter đc
                    // lấy thử đĩa trên bàn nếu đc thì chạy bên trong
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // nếu đã có đĩa trên bàn thì thêm thử kitchenobject của player vào
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitChenObjectSO()))
                        {
                            // nếu thêm đc thì xóa kitchenobject của player đi
                            // tại vẫn có thể player cầm đĩa khác k thêm đc
                            // còn cầm các đồ có thể thêm thì vẫn thêm bình thg và xóa đi oke
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            // nếu player k có kitchenobject thì thực thi else
            else
            {
                // set kichenobect cái đĩa theo player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
