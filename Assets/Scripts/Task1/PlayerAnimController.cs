using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [Header("References")]
    private Animator playerAnim;

    [Header("Variables")]
    private bool hasAnimator;

    //private void Awake()
    //{
    //    //playerAnim = GetComponent<Animator>();
    //}

    void Awake()
    {
        hasAnimator = TryGetComponent(out playerAnim);
    }


    public void SetMovementAnim(float speed)
    {
        if (hasAnimator)
        {
            playerAnim.SetFloat(AnimTags.speed, speed);
        }
    }

    public void SetGroundedBool(bool isGrounded)
    {
        if (hasAnimator)
        {
            playerAnim.SetBool(AnimTags.grounded, isGrounded);
        }
    }

    public void SetJumpBool(bool isJump)
    {
        playerAnim.SetBool(AnimTags.jump, isJump);
    }

    public void SetFireTrigger()
    {
        playerAnim.SetTrigger(AnimTags.fire);
    }

    public void SetCrouchAnim(bool isCrouch)
    {
        playerAnim.SetBool(AnimTags.crouch,isCrouch);
    }
}
