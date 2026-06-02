using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();
    public void SetKitchenObject(KitChenObject kitchenObject);

    public KitChenObject GetKitchenObject();
    public void ClearKitchenObject();
    public bool HasKitchenObject();
}
