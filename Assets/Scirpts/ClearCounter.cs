using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitChenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    private KitChenObject kitchenObject;

    public void Interact(Player player)
    {
        Debug.Log("Counter object = " + kitchenObject);

        if (kitchenObject == null)
        {
            Debug.Log("Spawn");
        }
        else
        {
            Debug.Log("Give to player");
        }

        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint.position, Quaternion.identity);
            kitchenObjectTransform.GetComponent<KitChenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            kitchenObject.SetKitchenObjectParent(player);
        }
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitChenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
    public KitChenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;

}
