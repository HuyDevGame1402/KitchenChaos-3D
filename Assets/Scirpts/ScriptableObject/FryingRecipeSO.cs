using UnityEngine;

// khởi tạo 1 scriptable object chứa các trường dữ liệu của đồ chiên
[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitChenObjectSO input; // đầu vào của đồ
    public KitChenObjectSO output; // đầu ra của đô khi chiên xong
    public float fryingTimerMax; // thời gian chiên của đồ
}
