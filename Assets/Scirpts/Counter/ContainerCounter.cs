using UnityEngine;
using System;

// thùng đựng nguyên liệu player có thể lấy ra sử dụng
public class ContainerCounter : BaseCounter
{
    // event thông báo 1 ng chs vừa ms lấy 1 kitchenobject của nó
    public event EventHandler OnPlayerGrabbedObject;
    // mỗi thùng chỉ có thể đựng 1 loại vật phẩm thôi
    [SerializeField] private KitChenObjectSO kitchenObjectSO;

    // khi ng chs tương tác vs counter để lấy object của nó
    public override void Interact(Player player) 
    {
        // kiểm tra player buộc phải k có kitchenobject nào chả thì ms cho lấy
        if(player.HasKitchenObject() == false)
        {
            // tạo ra kitchenobject mà thùng này chứa
            // xong set parent cho player 
            // 2 việc đó nằm trong hàm Spawn bên dưới
            KitChenObject.SpawnKitchenObject(kitchenObjectSO, player);
            // invoke ra event để gọi sự kiện lấy kitchenobject ra
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
