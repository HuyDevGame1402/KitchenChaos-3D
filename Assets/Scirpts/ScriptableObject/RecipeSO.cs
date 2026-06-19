using UnityEngine;
using System.Collections.Generic;

// tạo 1 scriptable object công thức nấu ăn
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    // chứa list các đồ bên trong món ăn
    public List<KitChenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
