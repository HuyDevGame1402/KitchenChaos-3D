using UnityEngine;
using System.Collections.Generic;
using System;

public class PlateKitchenObject : KitChenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitChenObjectSO kitchenObjectSO;
    }
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
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { 
                kitchenObjectSO = kitChenObjectSO   
            });
            return true;
        }
    }
}
