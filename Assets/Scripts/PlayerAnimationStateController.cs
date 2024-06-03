using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    public PlayerMovement playerMovement;
    public LineMovement lineMovement;
    public advancedClimbing advancedClimbing;
    int isCrouchingHash;
    int velocityHash;
    int isJumpingHash;
    int isWalkingOnLineHash;
    int isHangingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isCrouchingHash = Animator.StringToHash("isCrouching");
        velocityHash = Animator.StringToHash("Velocity");
        isJumpingHash = Animator.StringToHash("isJumping");
        isWalkingOnLineHash = Animator.StringToHash("isWalkingOnLine");
        isHangingHash = Animator.StringToHash("isHanging");
    }

    // Update is called once per frame
    void Update()
    {
        bool isCrouching = animator.GetBool(isCrouchingHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isHanging = animator.GetBool(isHangingHash);
        if (playerMovement.touchGround || lineMovement.isMovingOnLine || advancedClimbing.canHandle)
        {
            animator.SetBool(isJumpingHash, false);
            if (playerMovement.crouching)
            {
                if (!isCrouching)
                {
                    animator.SetBool(isCrouchingHash, true);
                }
            }
            else
            {
                if (isCrouching)
                {
                    animator.SetBool(isCrouchingHash, false);
                }
            }

            if (playerMovement.velocity > 0.1)
            {
                animator.SetFloat(velocityHash, playerMovement.velocity / 16);
            }
            else
            {
                if (animator.GetFloat(velocityHash) != 0)
                {
                    animator.SetFloat(velocityHash, 0f);
                }
            }

            if (lineMovement.isMovingOnLine && lineMovement.isAboveLine)
            {
                animator.SetBool(isWalkingOnLineHash, true);
            }
            else
            {
                animator.SetBool(isWalkingOnLineHash, false);
            }
            if (advancedClimbing.canHandle)
            {
                animator.SetBool(isHangingHash, true);
            }
            else
            {
                animator.SetBool(isHangingHash, false);
            }
        }
        else
        {
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isHangingHash, false);
        }
    }
}
