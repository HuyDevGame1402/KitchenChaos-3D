using UnityEngine;
using System.Collections.Generic;

public class PlateKitchenObject : KitChenObject
{
    [SerializeField] private List<KitChenObjectSO> validKitchenObjectSOList = new List<KitChenObjectSO>();
    private List<KitChenObjectSO> kitchenObjectSOList = new List<KitChenObjectSO>();

    public bool TryAddIngredient(KitChenObjectSO kitChenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitChenObjectSO)) return false;
        if (kitchenObjectSOList.Contains(kitChenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitChenObjectSO);
            return true;
        }
    }
}
