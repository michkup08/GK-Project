using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Objects Ref")]
    public Transform orientation;
    public LayerMask isGround;
    public CapsuleCollider capsuleCollider;

    [Header("Variables")]
    public float walkSpeedMultiplier;
    public float sprintSpeedMultiplier;
    public float crouchSpeedMultiplier;
    public float moveSpeedLimit;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMovementMultiplier = 0.1f;

    public float velocity; //for ui purpose

    private bool airMovementActive = true;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("private")]
    [SerializeField]
    float horizontalI;
    [SerializeField]
    float verticalI;
    [SerializeField]
    float playerHeight;
    [SerializeField]
    public bool touchGround;
    [SerializeField]
    bool readyToJump;
    Vector3 moveDir;
    Rigidbody playerRigidbody;

    private float moveSpeedMultiplier;
    public bool sprinting = false;
    public bool crouching = false;

    // Start is called before the first frame update
    void Start()
    {
        readyToJump = true;
        playerRigidbody = GetComponent<Rigidbody>();
        moveSpeedMultiplier = walkSpeedMultiplier;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        touchGround = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, 1f, isGround);

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
    }

    private void FixedUpdate()
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

    private void inputControl()
    {
        horizontalI = Input.GetAxisRaw("Horizontal");
        verticalI = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && touchGround && readyToJump && !onSteepSlope())
        {
            readyToJump = false;
            jump();
            Invoke(nameof(afterJump), jumpCooldown);
            touchGround = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && touchGround)
        {
            sprinting = true;
            moveSpeedMultiplier = sprintSpeedMultiplier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
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

    private void speedLimit()
    {
        Vector2 rbSpeed = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);
        if (rbSpeed.magnitude > moveSpeedLimit)
        {
            rbSpeed = rbSpeed.normalized * moveSpeedMultiplier;
            playerRigidbody.velocity = new Vector3(rbSpeed.x, playerRigidbody.velocity.y, rbSpeed.y);
        }
    }

    private void groundMovement()
    {
        moveDir = orientation.forward * verticalI + orientation.right * horizontalI;
        if (moveDir != Vector3.zero && transform.parent != null)
        {
            transform.parent = null;
        }

        if(onSlope())
        {
            playerRigidbody.AddForce(getSlopeMoveDirection() * moveSpeedMultiplier * 0.75f, ForceMode.Force);
        }
        else if(onSteepSlope())
        {
            playerRigidbody.AddForce(getSteepSlopeSlideDirection() * moveSpeedMultiplier * 0.75f, ForceMode.Force);
            playerRigidbody.AddForce(moveDir.normalized * moveSpeedMultiplier * 0.5f, ForceMode.Force);
        }
        else
        {
            playerRigidbody.AddForce(moveDir.normalized * moveSpeedMultiplier, ForceMode.Force);
        }
    }

    private void airMovement()
    {
        moveDir = orientation.forward * verticalI + orientation.right * horizontalI;
        playerRigidbody.AddForce(moveDir.normalized * moveSpeedMultiplier * airMovementMultiplier, ForceMode.Force);
    }

    private void jump()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void afterJump()
    {
        readyToJump = true;
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = false;
            moveSpeedMultiplier = walkSpeedMultiplier;
        }
    }

    public void enableAirMovement()
    {
        airMovementActive = true;
    }

    public void disableAirMovement()
    {
        airMovementActive = false;
    }

    private bool onSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, capsuleCollider.height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private bool onSteepSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, capsuleCollider.height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 getSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    private Vector3 getSteepSlopeSlideDirection()
    {
        return Vector3.Cross(Vector3.Cross(slopeHit.normal, Vector3.down), slopeHit.normal).normalized;
    }
}
