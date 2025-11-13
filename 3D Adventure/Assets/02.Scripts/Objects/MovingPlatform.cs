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

    private Vector3 startPos;
    private float time;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        time += Time.deltaTime * moveSpeed;

        float offset = Mathf.PingPong(time, moveDistance) - moveDistance * 0.5f;

        Vector3 position = startPos;

        if (isMovingX)
            position.x += offset;

        if (isMovingY)
            position.y += offset;

        if (isMovingZ)
            position.z += offset;

        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.transform.SetParent(transform);
            return;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.transform.SetParent(null);
            return;
        }
    }
}
