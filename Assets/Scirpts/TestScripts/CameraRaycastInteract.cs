using UnityEngine;

public class CameraRaycastInteract : MonoBehaviour
{
    private Camera _mainCamera;

    void Start()
    {
        // Lấy component Camera gắn trên chính Object này
        _mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // Kiểm tra nếu người chơi click chuột trái (0 là chuột trái, 1 chuột phải, 2 chuột giữa)
        // Vì ở bài trước mình khóa chuột (Locked), bạn có thể tạm thời bấm Esc để hiện chuột và click test
        if (Input.GetMouseButtonDown(0))
        {
            TriggerRaycastFromMouse();
        }
    }

    void TriggerRaycastFromMouse()
    {
        // 1. Tạo một tia Ray xuất phát từ vị trí Camera, hướng thẳng qua vị trí con trỏ chuột trên màn hình
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        // 2. Tạo một biến "thùng chứa" để hứng thông tin vật thể bị tia Ray đâm trúng
        RaycastHit hitInfo;

        // 3. Thực hiện bắn tia Raycast (bắn xa tối đa 100 mét)
        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            // Nếu đâm trúng một vật thể, in tên vật thể đó ra Console để debug
            Debug.Log("Đã click trúng: " + hitInfo.collider.name);

            // 4. Lấy Component Renderer của vật thể đó để đổi màu
            MeshRenderer meshRenderer = hitInfo.collider.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                // Tạo một màu ngẫu nhiên
                Color randomColor = new Color(Random.value, Random.value, Random.value);

                // Đổi màu vật liệu (Material) của Object
                meshRenderer.material.color = randomColor;
            }
        }
    }
}