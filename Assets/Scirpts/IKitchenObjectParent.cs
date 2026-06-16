using UnityEngine;

public interface IKitchenObjectParent
{
    // trả về vị trí mà kichenobject phải bám theo
    public Transform GetKitchenObjectFollowTransform();
    // dùng để gán kitchenobject hiện tại (get set ý)
    public void SetKitchenObject(KitChenObject kitchenObject);

    // get trả về kitchenobject
    public KitChenObject GetKitchenObject();
    // xóa tham chiếu kitchenobject
    public void ClearKitchenObject();
    // trả về true nếu có kitchenobject còn false nếu k
    public bool HasKitchenObject();
}
