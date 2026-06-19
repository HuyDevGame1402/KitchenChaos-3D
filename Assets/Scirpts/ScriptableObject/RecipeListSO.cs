using UnityEngine;
using System.Collections.Generic;

// tạo 1 scriptable object chứa ds công thức
[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    // list ds công thức
    public List<RecipeSO> recipeSOList;
}
