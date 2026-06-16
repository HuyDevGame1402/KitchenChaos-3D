using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    // đây là event sự kiện tĩnh để thông báo khi có bất kỳ object nào đặt lên bàn
    // ví dụ để 1 quả cà chua gọi để play sound
    public static event EventHandler OnAnyObjectPlacedHere;
    // reset lại event như là out ra ngoài scene menu thì cần reset
    // tại đây là biến tính static tồn tại trong suốt quá trình game chạy
    // nên dù có destroy scene đi thì vẫn p reset lại
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    // điểm mà kitchenobject follow theo ví dụ tay cầm player cầm đồ
    // hay điểm trên counter bàn để đồ, ...
    [SerializeField] private Transform counterTopPoint;
    // referen kitchenobject để lưu lại đồ vật mà đang sở hữu
    protected KitChenObject kitchenObject;
    
    // hđ khi ấn phím e để tương tác vs vs bàn
    public virtual void Interact(Player player)
    {
        
    }
    // dùng cho hành động phụ phím F
    // như cutting counter thì ấn sẽ cắt hoa quả
    public virtual void InteractAlternate(Player player)
    {

    }

    // trả về điểm follow
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
    // lưu kitchenobject
    public void SetKitchenObject(KitChenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            // invoke event đặt kitchenobject lên counter
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    // trả về kitchenobject
    public KitChenObject GetKitchenObject() => kitchenObject;
    // xóa kitchenobject
    public void ClearKitchenObject() => kitchenObject = null;
    // trả về true false nếu có kitchenobject
    public bool HasKitchenObject() => kitchenObject != null;
}
