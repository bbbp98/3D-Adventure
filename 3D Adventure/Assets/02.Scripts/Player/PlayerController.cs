using System;
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
    private ThirdPersonalCamera mCamera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector2 curMovementInput;
    public bool canMove = true;
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

    private MovingPlatform currentPlatform;
    private bool isOnPlatform;

    #region Event
    public Action onOpenInventory;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animationHandler = GetComponent<AnimationHandler>();
        _collider = GetComponent<CapsuleCollider>();
        _camera = Camera.main;
        mCamera = _camera.GetComponent<ThirdPersonalCamera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();

        if (isOnPlatform && currentPlatform != null)
        {
            Vector3 platformVel = currentPlatform.PlatformVelocity;
            platformVel.y = 0;
            _rigidbody.velocity += platformVel;
        }

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

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            onOpenInventory?.Invoke();
            ToggleCursor();
        }
    }
    #endregion

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        mCamera.canLook = !toggle;
    }

    #region Movement
    private void Move()
    {
        if (!canMove)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            return;
        }

        Vector3 camForward = _camera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = _camera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // calculate move direction base input value
        Vector3 moveDir = (camForward * curMovementInput.y + camRight * curMovementInput.x).normalized;

        float speed = CharacterManager.Instance.Player.isRun ? runSpeed : moveSpeed;
        
        Vector3 velocity = moveDir * speed;
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
        _rigidbody.AddForce(Vector3.up * (jumpPower + addJumpForce), ForceMode.Impulse);
        isGrounded = false;
        canMove = true;
    }

    private void GroundCheck()
    {
        // 캡슐 콜라이더 기준 중심점 계산
        Vector3 start = transform.position + Vector3.up * 0.1f;
        float checkDistance = rayDistance;

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
            _animationHandler.OnLand();

        wasGrounded = hitGround;
        isGrounded = hitGround;
    }
    #endregion

    public void ApplyBuff(BuffItemData buffData)
    {
        moveSpeed += buffData.buffValue;
        runSpeed += buffData.buffValue;
    }

    public void RemoveBuff(BuffItemData buffData)
    {
        moveSpeed -= buffData.buffValue;    
        runSpeed -= buffData.buffValue;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.TryGetComponent(out MovingPlatform mp))
        {
            if (transform.position.y > mp.transform.position.y + 0.05f)
            {
                isOnPlatform = true;
                currentPlatform = mp;
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent(out MovingPlatform mp))
        {
            isOnPlatform = false;
            currentPlatform = null;
        }
    }
}
