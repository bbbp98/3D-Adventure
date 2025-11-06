using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private int _IsMove = Animator.StringToHash("IsMove");
    private int _Jump = Animator.StringToHash("Jump");
    private int _Land = Animator.StringToHash("Land");

    [SerializeField] private Animator animator;

    public void Move(bool isMove)
    {
        animator.SetBool(_IsMove, isMove);
    }

    public void Jump()
    {
        animator.ResetTrigger(_Land);
        animator.SetTrigger(_Jump);
    }

    public void Land()
    {
        animator.SetTrigger(_Land);
    }
}
