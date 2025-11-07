using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonalCamera : MonoBehaviour
{
    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    private float yaw;  // left-right rotation angle
    private float pitch;    // up-down rotation angle
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float lookSensitivityX;
    [SerializeField] private float lookSensitivityY;
    [SerializeField] private float smoothRotationSpeed;
    [SerializeField] private bool canLook = true;

    private Vector2 mouseDelta;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CameraLook()
    {
        if (cameraContainer == null) return;

        yaw += mouseDelta.x * lookSensitivityX * Time.deltaTime;
        pitch -= mouseDelta.y * lookSensitivityY * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        //cameraContainer.rotation = rot;
        cameraContainer.rotation = Quaternion.Slerp(cameraContainer.rotation, rot, smoothRotationSpeed * Time.deltaTime);
    }
}
