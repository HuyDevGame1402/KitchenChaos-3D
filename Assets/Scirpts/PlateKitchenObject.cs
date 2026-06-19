using UnityEngine;
using System.Collections.Generic;
using System;

// class đại diện cho cái đĩa đựng đồ ăn
public class PlateKitchenObject : KitChenObject
{
    // event 1 nguyên liệu ms đc thêm vào đĩa
    // có tham số kèm theo event
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        // đó là kitchenobject SO thêm vào đĩa sẽ đc event gửi đi
        public KitChenObjectSO kitchenObjectSO;
    }
    // ds các nguyên liệu đc phép cho vào đĩa (tất cả các nguyên liệu hợp lệ trong game chứ k phải trong đĩa này chỉ có cần đó)
    [SerializeField] private List<KitChenObjectSO> validKitchenObjectSOList = new List<KitChenObjectSO>();
    // ds các nguyên liệu trong chính đĩa này
    private List<KitChenObjectSO> kitchenObjectSOList = new List<KitChenObjectSO>();
    // thêm 1 nguyên liệu vào đĩa
    public bool TryAddIngredient(KitChenObjectSO kitChenObjectSO)
    {
        // nếu k có trong ds các nguyên liệu đc thêm vào thì return false
        if (!validKitchenObjectSOList.Contains(kitChenObjectSO)) return false;
        // nếu đĩa này đã có nguyên liệu đó r thì return false thì mỗi món chỉ đc 1 nguyên liệu thôi
        if (kitchenObjectSOList.Contains(kitChenObjectSO))
        {
            return false;
        }
        // ngc lại thì thêm vào
        else
        {
            // thêm vào ds nguyên liệu trong đĩa
            kitchenObjectSOList.Add(kitChenObjectSO);
            // Invoke gọi thêm 1 nguyên liệu vào đĩa và gửi nguyên liệu đó đi trong event và return true
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { 
                kitchenObjectSO = kitChenObjectSO   
            });
            return true;
        }
    }
    // lấy ra ds nguyên liệu trong đĩa
    public List<KitChenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
