using UnityEngine;
using TMPro;
using UnityEngine.UI;

// class này phụ trách hiển thị 1 công thức món ăn lên UI
public class DeliveryManagerSingleUI : MonoBehaviour
{
    // name của công thức
    [SerializeField] private TextMeshProUGUI recipeNameText;
    // transform icon chứa các icon thành phần trong công thức
    [SerializeField] private Transform iconContainer;
    // icon các thành phần trong công thức
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        // ẩn đi cái icon thành phần đi
        iconTemplate.gameObject.SetActive(false);
    }
    // hàm set công thức lên UI
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        // set name lên UI
        recipeNameText.text = recipeSO.recipeName;
        // duyệt qua các child con của container
        foreach(Transform child in iconContainer)
        {
            // == vs icon mẫu thì bỏ qua còn lại xóa hết
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);  
        }
        // duyệt qua tất cả các thành phần trong công thức
        foreach(KitChenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            // tạo ra icon thành phần món ăn 
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            // bật active lên
            iconTransform.gameObject.SetActive(true);
            // set image đúng vs sprite của tp món ăn
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
