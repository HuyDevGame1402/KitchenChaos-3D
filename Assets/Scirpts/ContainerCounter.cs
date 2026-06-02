using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitChenObjectSO kitchenObjectSO;

    public override void Interact(Player player) 
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab,
                GetKitchenObjectFollowTransform().position, Quaternion.identity);
        kitchenObjectTransform.GetComponent<KitChenObject>().
            SetKitchenObjectParent(player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
