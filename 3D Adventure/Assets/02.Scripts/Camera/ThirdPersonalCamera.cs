using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonalCamera : MonoBehaviour
{
    [Header("Look")]
    [SerializeField] private Transform target;
    private float yaw;  // left-right rotation angle
    private float pitch;    // up-down rotation angle

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float lookSensitivityX;
    [SerializeField] private float lookSensitivityY;
    [SerializeField] private float smoothRotationSpeed;
    [SerializeField] public bool canLook = true;

    [Header("Follow")]
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float smoothFollowSpeed = 10f;

    private Vector2 mouseDelta;

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();

        FollowTarget();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CameraLook()
    {
        if (target == null) return;

        yaw += mouseDelta.x * lookSensitivityX * Time.deltaTime;
        pitch -= mouseDelta.y * lookSensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // rotate camera
        Quaternion cameraRot = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraRot, smoothRotationSpeed * Time.deltaTime);
    }

    private void FollowTarget()
    {
        if (target == null) return;

        //Vector3 desiredPos = target.position + Quaternion.Euler(0, yaw, 0) * followOffset;
        Vector3 desiredPos = target.position + transform.TransformDirection(followOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothFollowSpeed * Time.deltaTime);
    }
}
