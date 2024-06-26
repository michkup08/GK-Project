using UnityEngine;
using System;

/// <summary>
/// Manages player movement including walking, sprinting, crouching, and jumping.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Objects Ref")]
    /// <value>Reference to the player's orientation.</value>
    public Transform orientation;

    /// <value>Layer mask to identify ground surfaces.</value>
    public LayerMask isGround;

    /// <value>Reference to the player's capsule collider.</value>
    public CapsuleCollider capsuleCollider;

    [Header("Variables")]
    /// <value>Multiplier for walking speed.</value>
    public float walkSpeedMultiplier;

    /// <value>Multiplier for sprinting speed.</value>
    public float sprintSpeedMultiplier;

    /// <value>Multiplier for crouching speed.</value>
    public float crouchSpeedMultiplier;

    /// <value>Limit for the player's movement speed.</value>
    public float moveSpeedLimit;

    /// <value>Drag applied to the player while on the ground.</value>
    public float groundDrag;

    /// <value>Force applied for jumping.</value>
    public float jumpForce;

    /// <value>Cooldown time between jumps.</value>
    public float jumpCooldown;

    /// <value>Multiplier for movement speed while in the air.</value>
    public float airMovementMultiplier = 0.8f;

    /// <value>Current velocity of the player (for UI purposes).</value>
    public float velocity;

    /// <value>Current flat (horizontal) velocity of the player (for UI purposes).</value>
    public float velocityFlat;

    private bool airMovementActive = true;

    [Header("Slope Handling")]
    /// <value>Maximum angle of slope the player can walk on.</value>
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("private")]
    [SerializeField]
    /// <value>Horizontal input value.</value>
    float horizontalI;

    [SerializeField]
    /// <value>Vertical input value.</value>
    float verticalI;

    [SerializeField]
    /// <value>Height of the player.</value>
    float playerHeight;

    [SerializeField]
    /// <value>Flag indicating if the player is touching the ground.</value>
    public bool touchGround;

    [SerializeField]
    /// <value>Flag indicating if the player is ready to jump.</value>
    bool readyToJump;

    /// <value>Direction of player movement.</value>
    Vector3 moveDir;

    /// <value>Reference to the player's Rigidbody component.</value>
    Rigidbody playerRigidbody;

    private float moveSpeedMultiplier;
    /// <value>Flag indicating if the player is sprinting.</value>
    public bool sprinting = false;

    /// <value>Flag indicating if the player is crouching.</value>
    public bool crouching = false;

    [SerializeField]
    /// <value>Flag indicating if the player is dead.</value>
    public bool dead = false;

    private Quaternion initialRotation;

    /// <summary>
    /// Initializes the player movement script.
    /// </summary>
    void Start()
    {
        readyToJump = true;
        playerRigidbody = GetComponent<Rigidbody>();
        moveSpeedMultiplier = walkSpeedMultiplier;
        Cursor.lockState = CursorLockMode.Locked;
        SaveSystem.initialize();
    }

    /// <summary>
    /// Updates the player state and handles input each frame.
    /// </summary>
    void Update()
    {
        touchGround = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, 1f, isGround);

        if (!dead)
        {
            inputControl();
            speedLimit();

            if (touchGround)
            {
                playerRigidbody.drag = groundDrag;
            }
            else
            {
                playerRigidbody.drag = 0;
            }

            velocity = playerRigidbody.velocity.magnitude;
            velocityFlat = Math.Abs(playerRigidbody.velocity.x) + Math.Abs(playerRigidbody.velocity.z);
        }
        else
        {
            horizontalI = 0;
            verticalI = 0;
            transform.rotation = initialRotation;
        }
    }

    /// <summary>
    /// Handles physics updates for player movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (!dead)
        {
            if (touchGround)
            {
                groundMovement();
            }
            else if (airMovementActive)
            {
                airMovement();
            }
        }
    }

    /// <summary>
    /// Processes player input for movement and actions.
    /// </summary>
    private void inputControl()
    {
        horizontalI = Input.GetAxisRaw("Horizontal");
        verticalI = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && touchGround && readyToJump && !onSteepSlope())
        {
            readyToJump = false;
            Invoke(nameof(jump), 0.2f);
            Invoke(nameof(afterJump), jumpCooldown);
            touchGround = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && touchGround && !crouching)
        {
            sprinting = true;
            moveSpeedMultiplier = sprintSpeedMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !crouching)
        {
            sprinting = false;
            moveSpeedMultiplier = walkSpeedMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && touchGround)
        {
            capsuleCollider.height -= 0.6f;
            capsuleCollider.center -= Vector3.up * 0.3f;
            playerRigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            readyToJump = false;
            crouching = true;
            moveSpeedMultiplier = crouchSpeedMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && crouching)
        {
            capsuleCollider.height += 0.6f;
            capsuleCollider.center += Vector3.up * 0.3f;
            readyToJump = true;
            crouching = false;
            moveSpeedMultiplier = walkSpeedMultiplier;
        }
    }

    /// <summary>
    /// Limits the player's speed to the defined maximum.
    /// </summary>
    private void speedLimit()
    {
        Vector2 rbSpeed = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);
        if (rbSpeed.magnitude > moveSpeedLimit)
        {
            rbSpeed = rbSpeed.normalized * moveSpeedMultiplier;
            playerRigidbody.velocity = new Vector3(rbSpeed.x, playerRigidbody.velocity.y, rbSpeed.y);
        }
    }

    /// <summary>
    /// Handles movement while the player is on the ground.
    /// </summary>
    private void groundMovement()
    {
        moveDir = (orientation.forward * verticalI) + (orientation.right * horizontalI);
        if (moveDir != Vector3.zero && transform.parent != null)
        {
            transform.parent = null;
        }

        if (onSlope())
        {
            playerRigidbody.AddForce(getSlopeMoveDirection() * moveSpeedMultiplier * 0.75f, ForceMode.Force);
        }
        else if (onSteepSlope())
        {
            playerRigidbody.AddForce(getSteepSlopeSlideDirection() * moveSpeedMultiplier * 0.75f, ForceMode.Force);
            playerRigidbody.AddForce(moveDir.normalized * moveSpeedMultiplier * 0.5f, ForceMode.Force);
        }
        else
        {
            playerRigidbody.AddForce(moveDir.normalized * moveSpeedMultiplier, ForceMode.Force);
        }
    }

    /// <summary>
    /// Handles movement while the player is in the air.
    /// </summary>
    private void airMovement()
    {
        moveDir = (orientation.forward * verticalI) + (orientation.right * horizontalI);
        playerRigidbody.AddForce(moveDir.normalized * moveSpeedMultiplier * airMovementMultiplier, ForceMode.Force);
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    private void jump()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Resets the player's jump state after jumping.
    /// </summary>
    private void afterJump()
    {
        readyToJump = true;
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = false;
            moveSpeedMultiplier = walkSpeedMultiplier;
        }
    }

    /// <summary>
    /// Enables air movement for the player.
    /// </summary>
    public void enableAirMovement()
    {
        airMovementActive = true;
    }

    /// <summary>
    /// Disables air movement for the player.
    /// </summary>
    public void disableAirMovement()
    {
        airMovementActive = false;
    }

    /// <summary>
    /// Checks if the player is on a walkable slope.
    /// </summary>
    /// <returns>True if the player is on a slope within the maximum slope angle.</returns>
    private bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (capsuleCollider.height * 0.5f) + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    /// <summary>
    /// Checks if the player is on a steep slope.
    /// </summary>
    /// <returns>True if the player is on a slope steeper than the maximum slope angle.</returns>
    private bool onSteepSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (capsuleCollider.height * 0.5f) + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > maxSlopeAngle && angle != 0;
        }
        return false;
    }

    /// <summary>
    /// Gets the direction to move on a slope.
    /// </summary>
    /// <returns>Normalized direction vector for moving on the slope.</returns>
    private Vector3 getSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    /// <summary>
    /// Gets the direction to slide down a steep slope.
    /// </summary>
    /// <returns>Normalized direction vector for sliding down the steep slope.</returns>
    private Vector3 getSteepSlopeSlideDirection()
    {
        return Vector3.Cross(Vector3.Cross(slopeHit.normal, Vector3.down), slopeHit.normal).normalized;
    }
}