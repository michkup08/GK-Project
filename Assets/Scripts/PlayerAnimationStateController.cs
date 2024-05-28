using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    public PlayerMovement playerMovement;
    int isCrouchingHash;
    int velocityHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isCrouchingHash = Animator.StringToHash("isCrouching");
        velocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        bool isCrouching = animator.GetBool(isCrouchingHash);
        if (playerMovement.touchGround)
        {
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
                animator.SetFloat(velocityHash, playerMovement.velocity);
            }
            else
            {
                if (animator.GetFloat(velocityHash) != 0)
                {
                    animator.SetFloat(velocityHash, 0f);
                }
            }
        }

    }
}
