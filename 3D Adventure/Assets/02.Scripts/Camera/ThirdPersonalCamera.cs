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
    [SerializeField] private float smoothFollowTime = 0.03f;

    private Vector2 mouseDelta;
    private Vector3 followVel;

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

        Vector3 backDir = transform.forward * followOffset.z;
        Vector3 height = Vector3.up * followOffset.y;
        Vector3 desiredPos = target.position + backDir + height;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref followVel, smoothFollowTime);
    }
}
