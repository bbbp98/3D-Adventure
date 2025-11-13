using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance;
    [SerializeField] private bool isMovingX;
    [SerializeField] private bool isMovingY;
    [SerializeField] private bool isMovingZ;

    private Rigidbody _rigidbody;
    private Vector3 startPos;
    private float time;

    public Vector3 PlatformVelocity { get; private set; }

    private Vector3 lastPos;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
        CalculateVelocity();
    }

    private void Move()
    {
        time += Time.deltaTime * moveSpeed;

        float offset = Mathf.Sin(time) * (moveDistance * 0.5f);

        Vector3 position = startPos;

        if (isMovingX)
            position.x += offset;

        if (isMovingY)
            position.y += offset;

        if (isMovingZ)
            position.z += offset;

        //transform.position = position;
        _rigidbody.MovePosition(position);
    }

    private void CalculateVelocity()
    {
        PlatformVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        lastPos = transform.position;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.TryGetComponent<Player>(out Player player))
    //    {
    //        player.transform.SetParent(transform);
    //        return;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.TryGetComponent<Player>(out Player player))
    //    {
    //        player.transform.SetParent(null);
    //        return;
    //    }
    //}
}
