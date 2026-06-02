using UnityEngine;

public class KitChenObject : MonoBehaviour
{
    [SerializeField] private KitChenObjectSO kitChenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitChenObjectSO GetKitChenObjectSO()
    {
        return kitChenObjectSO;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Kitchen Object Parent already has a Kitchen Object!");
        }
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
