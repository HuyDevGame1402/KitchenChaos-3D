using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitChenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
