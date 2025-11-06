using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform model;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector2 curMovementInput;

    [Header("Look")]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float minYLook;
    [SerializeField] private float maxYLook;
    private float camCurXRot;
    private float camCurYRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;
    private Camera _camera;

    [Header("Jump")]
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance;
    public bool isGrounded;
    public bool wasGrounded = false;

    [Header("Components")]
    private Rigidbody _rigidbody;
    private AnimationHandler _animationHandler;
    private CapsuleCollider _collider;

    #region Unity Life Cycle
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animationHandler = GetComponent<AnimationHandler>();
        _collider = GetComponent<CapsuleCollider>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }
    #endregion

    #region InputSystem
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            _animationHandler.Move(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            _animationHandler.Move(false);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded)
        {
            _animationHandler.Jump();
        }
    }
    #endregion

    private void Move()
    {
        Vector3 direction = curMovementInput.x * Vector3.right + curMovementInput.y * Vector3.forward;
        direction *= moveSpeed;
        direction.y = _rigidbody.velocity.y;

        _rigidbody.velocity = direction;

        Vector3 normalDirection = new Vector3(direction.x, 0, direction.z).normalized;

        if (normalDirection != Vector3.zero)
        {
            Quaternion targetLotation = Quaternion.LookRotation(normalDirection.normalized);
            model.rotation = Quaternion.Slerp(model.rotation, targetLotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CameraLook()
    {
        //camCurXRot += mouseDelta.y * lookSensitivity;
    }

    public void Jump()
    {
        _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;
    }

    private void Land()
    {
        _animationHandler.Land();
    }

    private void GroundCheck()
    {
        // 캡슐 콜라이더 기준 중심점 계산
        Vector3 start = transform.position + Vector3.up * (_collider.radius + 0.05f);
        float checkDistance = (_collider.height / 2f) - _collider.radius + rayDistance;

        // SphereCast로 감지 (언덕에서도 안정적)
        bool hitGround = Physics.SphereCast(
            start,
            _collider.radius * 0.9f,
            Vector3.down,
            out RaycastHit hit,
            checkDistance,
            groundLayer
        );

        // 착지 순간만 감지
        if (!wasGrounded && hitGround && _rigidbody.velocity.y <= 0)
            Land();

        wasGrounded = hitGround;
        isGrounded = hitGround;
    }
}
