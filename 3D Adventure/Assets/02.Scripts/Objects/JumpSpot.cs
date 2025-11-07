using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSpot : MonoBehaviour
{
    [SerializeField] private float force;

    private Vector3 originalScale;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (!player.controller.isGrounded)
            {
                player.controller.Jump(force);
                StartCoroutine(PressAnimationCo());
            }
        }
    }

    private IEnumerator PressAnimationCo()
    {
        originalScale = transform.localScale;
        float timer = 0f;
        float duration = 0.1f;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * 0.8f, originalScale.z);

        // 눌리기
        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
            yield return null;
        }

        // 복원
        timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / 0.3f);
            yield return null;
        }
    }
}
