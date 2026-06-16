using UnityEngine;

// tạo 1 scriptable object chứa dữ liệu cut kitchenobject
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public KitChenObjectSO input; // đầu vào cắt
    public KitChenObjectSO output; // đầu ra khi cắt xong 
    public int cuttingProgressMax; // số lượng lần cắt tối đa của vật thể
}
