using UnityEngine;

public class KitChenObject : MonoBehaviour
{
    [SerializeField] private KitChenObjectSO kitChenObjectSO;

    private ClearCounter clearCounter;

    public KitChenObjectSO GetKitChenObjectSO()
    {
        return kitChenObjectSO;
    }
    public void SetClearCounter(ClearCounter clearCounter)
    {
        if(this.clearCounter != null)
        {
            this.clearCounter.ClearKitchenObject();
        }
        this.clearCounter = clearCounter;
        if (clearCounter.HasKitchenObject())
        {

        }
        clearCounter.SetKitchenObject(this);
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}
