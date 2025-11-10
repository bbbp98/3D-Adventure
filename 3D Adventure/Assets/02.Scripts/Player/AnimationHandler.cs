using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private int _isMove = Animator.StringToHash("IsMove");
    private int _jump = Animator.StringToHash("Jump");
    private int _land = Animator.StringToHash("Land");
    private int _isRun = Animator.StringToHash("IsRun");

    [SerializeField] private Animator animator;

    public void OnMove(bool isMove)
    {
        animator.SetBool(_isMove, isMove);
    }

    public void OnJump()
    {
        animator.ResetTrigger(_land);
        animator.SetTrigger(_jump);
    }

    public void OnLand()
    {
        animator.SetTrigger(_land);
    }

    public void OnRun(bool isRun)
    {
        animator.SetBool(_isRun, isRun);
    }
}
