using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    public PlayerMovement playerMovement;
    Transform orientation;
    int isCrouchingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isCrouchingHash = Animator.StringToHash("isCrouching");
    }

    // Update is called once per frame
    void Update()
    {
        bool isCrouching = animator.GetBool(isCrouchingHash);
        if(playerMovement.crouching && playerMovement.touchGround)
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
    }
}
