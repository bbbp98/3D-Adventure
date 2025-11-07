using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    public void Jump()
    {
        controller?.Jump();
    }
}
