using UnityEngine;

/// <summary>
/// Controls the animation states of the player character based on various game conditions.
/// </summary>
public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    public PlayerMovement playerMovement;   /// <value> Reference to the PlayerMovement script attached to the player object. </value>

    public LineMovement lineMovement;       /// <value> Reference to the LineMovement script for handling line movement. </value>
    public advancedClimbing advancedClimbing;   /// <value> Reference to the advancedClimbing script for advanced climbing behaviors. </value>
    public Ziplining ziplining;         /// <value> Reference to the Ziplining script for handling ziplining mechanics. </value>

    int isCrouchingHash;        /// <value> Hash for the "isCrouching" parameter in the animator. </value>
    int velocityHash;           /// <value> Hash for the "Velocity" parameter in the animator. </value>
    int isJumpingHash;          /// <value> Hash for the "isJumping" parameter in the animator. </value>
    int isWalkingOnLineHash;    /// <value> Hash for the "isWalkingOnLine" parameter in the animator. </value>
    int isWalkingUnderLineHash; /// <value> Hash for the "isWalkingUnderLine" parameter in the animator. </value>
    int isWalkingUnderLineDirectionHash;    /// <value> Hash for the "isWalkingUnderLineDirection" parameter in the animator. </value>
    int isHangingHash;          /// <value> Hash for the "isHanging" parameter in the animator. </value>
    int isZipLiningHash;        /// <value> Hash for the "isZipLining" parameter in the animator. </value>
    int velocityFlatHash;       /// <value> Hash for the "xVelocity" parameter in the animator. </value>

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isCrouchingHash = Animator.StringToHash("isCrouching");
        velocityHash = Animator.StringToHash("Velocity");
        velocityFlatHash = Animator.StringToHash("xVelocity");
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
        animator.SetFloat(velocityFlatHash, playerMovement.velocityFlat / 16);
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
            animator.SetBool("isFliping", true);
        }
        else
        {
            animator.SetBool("isFliping", false);
        }
    }
}
