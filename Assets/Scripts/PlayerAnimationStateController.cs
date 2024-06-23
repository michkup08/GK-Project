using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    public PlayerMovement playerMovement;
    public LineMovement lineMovement;
    public advancedClimbing advancedClimbing;
    public Ziplining ziplining;
    int isCrouchingHash;
    int velocityHash;
    int isJumpingHash;
    int isWalkingOnLineHash;
    int isWalkingUnderLineHash;
    int isWalkingUnderLineDirectionHash;
    int isHangingHash;
    int isZipLiningHash;
   
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        isCrouchingHash = Animator.StringToHash("isCrouching");
        velocityHash = Animator.StringToHash("Velocity");
        isJumpingHash = Animator.StringToHash("isJumping");
        isWalkingOnLineHash = Animator.StringToHash("isWalkingOnLine");
        isWalkingUnderLineHash = Animator.StringToHash("isWalkingUnderLine");
        isWalkingUnderLineDirectionHash = Animator.StringToHash("isWalkingUnderLineDirection");
        isHangingHash = Animator.StringToHash("isHanging");
        isZipLiningHash = Animator.StringToHash("isZipLining");
    }

    // Update is called once per frame
    void Update()
    {
        bool isCrouching = animator.GetBool(isCrouchingHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isWalkingOnLine = animator.GetBool(isWalkingOnLineHash);
        bool isWalkingUnderLine = animator.GetBool(isWalkingUnderLineHash);
        bool isWalkingUnderLineDirection = animator.GetBool(isWalkingUnderLineDirectionHash);
        bool isHanging = animator.GetBool(isHangingHash);
        bool isZipLining = animator.GetBool(isZipLiningHash);
        if (playerMovement.touchGround || lineMovement.isMovingOnLine || advancedClimbing.canHandle || ziplining.isZiplining)
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
            if (lineMovement.isMovingOnLine)
            {
                if (lineMovement.isAboveLine)
                {
                    if (!isWalkingOnLine)
                    {
                        animator.SetBool(isWalkingOnLineHash, true);
                    }
                }
                else
                {
                    if (!isWalkingUnderLine)
                    {
                        animator.SetBool(isWalkingUnderLineHash, true);
                    }
                    if (lineMovement.direction == 1)
                    {
                        animator.SetBool(isWalkingUnderLineDirectionHash, true);
                    }
                    else
                    {
                        animator.SetBool(isWalkingUnderLineDirectionHash, false);
                    }
                }
            }
            else
            {
                if (isWalkingOnLine || isWalkingUnderLine)
                {
                    animator.SetBool(isWalkingOnLineHash, false);
                    animator.SetBool(isWalkingUnderLineHash, false);
                }
            }
            if (advancedClimbing.canHandle)
            {
                animator.SetBool(isHangingHash, true);
            }
            else
            {
                animator.SetBool(isHangingHash, false);
            }
            if (ziplining.isZiplining)
            {
                if (!isZipLining)
                {
                    animator.SetBool(isZipLiningHash, true);
                }
            }
            else
            {
                if (isZipLining)
                {
                    animator.SetBool(isZipLiningHash, false);
                }
            }
        }
        else
        {
            animator.SetBool(isJumpingHash, true);
            animator.SetBool(isHangingHash, false);
            animator.SetBool(isWalkingUnderLineHash, false);
        }

        

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isKicking", true);
        }
        else
        {
            animator.SetBool("isKicking", false);
        }

        if (Input.GetKey(KeyCode.C))
        {
            //Debug.Log("true");
            animator.SetBool("isFliping", true);
        }
        else
        {
            //Debug.Log("false");
            animator.SetBool("isFliping", false);
        }

    }
}
