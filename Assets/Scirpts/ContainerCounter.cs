using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitChenObjectSO kitchenObjectSO;

    public override void Interact(Player player) 
    {
        if(player.HasKitchenObject() == false)
        {
            KitChenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
