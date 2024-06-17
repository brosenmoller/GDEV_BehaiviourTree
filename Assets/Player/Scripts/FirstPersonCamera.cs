using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSpeed = 0.2f;
    [SerializeField] private float lookXLimit = 45.0f;

    private float rotationX = 0;
    private float rotationY = 0;
    private InputService inputService;

    private void Start()
    {
        inputService = ServiceLocator.Instance.Get<InputService>();
        rotationY = transform.rotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        float inputX = inputService.playerInputActions.PlayerActionMap.MoveCameraX.ReadValue<float>();
        float inputY = inputService.playerInputActions.PlayerActionMap.MoveCameraY.ReadValue<float>();

        if (Mathf.Abs(inputY) < 0.01f &&
            Mathf.Abs(inputX) < 0.01f)
        {
            return;
        }

        rotationX += -inputY * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);

        rotationY = transform.rotation.eulerAngles.y;
        if (rotationY > 180)
        {
            rotationY -= 360;
        }

        rotationY += inputX * lookSpeed;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
