using UnityEngine;

// class cho vật thể trong game như táo phô mai hay đĩa,...
public class KitChenObject : MonoBehaviour
{
    // chứa dữ liệu của kitchenobject
    [SerializeField] private KitChenObjectSO kitChenObjectSO;
    // biến này lưu lại parent của kichenobject cho bt nó đang nằm ở player hay counter
    private IKitchenObjectParent kitchenObjectParent;
    // lấy ra kitchenobject SO của kitchenobject hiện tại
    public KitChenObjectSO GetKitChenObjectSO()
    {
        return kitChenObjectSO;
    }
    // Set KitchenObject Parent truyền vào tham số parent
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // nếu hiện tại kitchenobject đã có parent sở hữu thì thực hiện if
        if(this.kitchenObjectParent != null)
        {
            // xóa tham chiếu của parent cũ vs kitchenobject này đi
            this.kitchenObjectParent.ClearKitchenObject();
        }
        // set lại parent ms cho kitchenobject này
        this.kitchenObjectParent = kitchenObjectParent;
        // kiểm tra xem player đã có object chưa nếu có log ra lỗi
        // thực ra ở đây chưa chặt chẽ lắm 1 là k cho luôn return ở đầu 2 là phải kt xem có playr plate để đặt đồ lên đĩa k
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Kitchen Object Parent already has a Kitchen Object!");
        }
        // set kitchen object cho parrent vs vị trí child gameobject
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    // trả về parent
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    // xóa kitchenobject đi
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    // hàm kiểm tra kitchenobject hiện tại có phải plate đĩa k
    // và trả ra platekitchenobject
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        // nếu bản thân là platekitchenobject thì trả ra và return true
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        // ngc lại null và false-
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
    // hàm tĩnh static spawn kitchenobject
    // vs tham số truyền vào kitchenobjectSO và parent để set parent cho nó
    public static KitChenObject SpawnKitchenObject(KitChenObjectSO kitChenObjectSO,
        IKitchenObjectParent kitchenObjectParent)
    {
        // tạo ra object
        Transform kitchenObjectTransform = Instantiate(kitChenObjectSO.prefab);
        // lấy ra kitchen object
        KitChenObject kitchenObject = kitchenObjectTransform.GetComponent<KitChenObject>();
        // set parent
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        // trả về kitchenobject đó
        return kitchenObject;
    }
}
