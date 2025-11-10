using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform model;
    private Camera _camera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector2 curMovementInput;
    private bool canMove = true;
    [SerializeField] float runSpeed;

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
    #endregion

    #region InputSystem
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            _animationHandler.OnMove(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            _animationHandler.OnMove(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded)
        {
            _animationHandler.OnJump();
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
            canMove = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isGrounded)
        {
            CharacterManager.Instance.Player.isRun = true;
            _animationHandler.OnRun(CharacterManager.Instance.Player.isRun);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            CharacterManager.Instance.Player.isRun = false;
            _animationHandler.OnRun(CharacterManager.Instance.Player.isRun);
        }
    }
    #endregion

    private void Move()
    {
        if (!canMove) return;

        Vector3 camForward = _camera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = _camera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // calculate move direction base input value
        Vector3 moveDir = (camForward * curMovementInput.y + camRight * curMovementInput.x).normalized;

        Vector3 velocity = CharacterManager.Instance.Player.isRun ? moveDir * runSpeed : moveDir * moveSpeed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetLotation = Quaternion.LookRotation(moveDir);
            model.rotation = Quaternion.Slerp(model.rotation, targetLotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Jump(float addJumpForce = 0f)
    {
        transform.position += Vector3.up * 0.05f;
        _rigidbody.AddForce(Vector3.up * (jumpPower + addJumpForce), ForceMode.Impulse);
        isGrounded = false;
        canMove = true;
    }

    private void Land()
    {
        _animationHandler.OnLand();
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
