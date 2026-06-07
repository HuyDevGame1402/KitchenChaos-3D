using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public enum MovementMode { TransformBased, RigidbodyBased }

    [Header("Movement Settings")]
    public MovementMode moveMode = MovementMode.TransformBased;
    public float moveSpeed = 7f;
    public float rotationSpeed = 10f;

    [Header("Camera Orbit Settings (Mouse)")]
    [SerializeField] private Transform cameraTransform;
    public float mouseSensitivity = 3f;   // Tốc độ nhạy của chuột
    public float cameraDistance = 7f;     // Khoảng cách từ camera tới nhân vật (thay cho cameraOffset cũ)
    public float minVerticalAngle = -20f; // Giới hạn góc ngước lên (không cho camera cắm xuống đất)
    public float maxVerticalAngle = 60f;  // Giới hạn góc nhìn từ trên xuống

    [Header("Component References")]
    [SerializeField] private Rigidbody rb;

    private Vector3 _moveDirection;
    private float _cinemachineTargetX; // Lưu góc xoay ngang (Yaw) theo chuột
    private float _cinemachineTargetY; // Lưu góc xoay dọc (Pitch) theo chuột

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        // Khóa con trỏ chuột vào giữa màn hình game và ẩn nó đi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Khởi tạo góc xoay ban đầu của camera để tránh bị giật góc khi bấm Play
        if (cameraTransform != null)
        {
            Vector3 angles = cameraTransform.eulerAngles;
            _cinemachineTargetX = angles.y;
            _cinemachineTargetY = angles.x;
        }
    }

    void Update()
    {
        // 1. NHẬN INPUT DI CHUYỂN
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 2. NHẬN INPUT CHUỘT ĐỂ TÍNH TOÁN GÓC QUAY CAMERA
        _cinemachineTargetX += Input.GetAxis("Mouse X") * mouseSensitivity;
        _cinemachineTargetY -= Input.GetAxis("Mouse Y") * mouseSensitivity; // Dấu trừ để hướng chuột không bị ngược cảm giác

        // Giới hạn góc xoay dọc để camera không bị lật nhào 360 độ qua đầu/dưới chân
        _cinemachineTargetY = Mathf.Clamp(_cinemachineTargetY, minVerticalAngle, maxVerticalAngle);

        // 3. TÍNH TOÁN HƯỚNG DI CHUYỂN THEO CAMERA
        // Nhân vật sẽ chạy dựa theo hướng mà Camera đang nhìn
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f; // Triệt tiêu trục Y để tránh nhân vật bị cắm đầu xuống đất khi di chuyển
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        _moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // 4. XOAY NHÂN VẬT MƯỢT MÀ THEO HƯỚNG DI CHUYỂN
        if (_moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 5. DI CHUYỂN KHÔNG VẬT LÝ
        if (moveMode == MovementMode.TransformBased)
        {
            rb.isKinematic = true;
            transform.Translate(_moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void FixedUpdate()
    {
        // 6. DI CHUYỂN CÓ VẬT LÝ
        if (moveMode == MovementMode.RigidbodyBased)
        {
            rb.isKinematic = false;
            Vector3 targetPosition = transform.position + _moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }

    // LATEUPDATE: Tính toán vị trí Camera xoay quanh Player sau khi Player đã di chuyển xong xuôi
    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 1. Tạo góc xoay của Camera (Quaternion) từ dữ liệu di chuột X và Y
        Quaternion targetCameraRotation = Quaternion.Euler(_cinemachineTargetY, _cinemachineTargetX, 0f);

        // 2. Xác định điểm mục tiêu để Camera nhìn vào (Nhìn vào ngực/đầu nhân vật thay vì dưới chân)
        Vector3 targetFocusPoint = transform.position + Vector3.up * 1.2f;

        // 3. Tính toán vị trí lý tưởng của Camera dựa trên ma trận xoay và khoảng cách định sẵn
        // Công thức: Vị trí tiêu điểm - (Hướng nhìn của Camera * Khoảng cách)
        Vector3 targetCameraPosition = targetFocusPoint - (targetCameraRotation * Vector3.forward * cameraDistance);

        // 4. Cập nhật trực tiếp vị trí và góc quay mới cho Camera
        cameraTransform.position = targetCameraPosition;
        cameraTransform.rotation = targetCameraRotation;
    }
}