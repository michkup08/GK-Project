using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    /// Player orientation
    [SerializeField]
    Transform orientation;
    /// Climbable layer
    [SerializeField]
    LayerMask climbableWall;

    [Header("Climbing")]
    /// Climb speed
    [SerializeField]
    float climbSpeed = 1.5f;
    /// max climb angle (to recognize climbing and walking on the wall)
    [SerializeField]
    float maxClimbAngle = 30.0f;

    /// drag before start climbing
    private float originalDrag;
    /// climbing state
    private bool isClimbing = false;

    [Header("Climbing Timer")]
    /// max climbing times
    [SerializeField]
    float maxClimbTimeX = 0.4f;
    [SerializeField]
    float maxClimbTimeZ = 0.4f;

    private RaycastHit hitWall;

    private readonly string[] climbingButtonsPrompts =
    {
        "press <sprite name=\"E\"> to let go",
        "move with <sprite name=\"W\"> <sprite name=\"A\"> <sprite name=\"S\"> <sprite name=\"D\">"
    };

    ScreenHints buttonPromptsController;

    /// climbing movement timers
    private float climbTimeX = 0.0f;
    private float climbTimeZ = 0.0f;

    /// Player movement reference to access touchGround variable
    PlayerMovement playerMovement;

    private bool climbingLock = false;

    Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        SetClimbingState();
        if (isClimbing)
        {
            originalDrag = playerRigidbody.drag;
            Climb();
        }
    }

    private void DisableClimbing()
    {
        playerRigidbody.useGravity = true;

        if (!playerMovement.touchGround)
            playerRigidbody.drag = 0;
        else
            playerRigidbody.drag = originalDrag;
    }

    private void Climb()
    {
        playerRigidbody.useGravity = false;
        playerRigidbody.drag = 0;

        // Get the normal of the wall hit
        Vector3 wallNormal = hitWall.normal;

        // Calculate movement direction perpendicular to the wall
        Vector3 rightDirection = Vector3.Cross(Vector3.up, wallNormal);
        Vector3 forwardDirection = Vector3.Cross(wallNormal, rightDirection);

        // Calculate player's movement based on input
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += forwardDirection;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= forwardDirection;
        if (Input.GetKey(KeyCode.A))
            moveDirection += rightDirection;
        if (Input.GetKey(KeyCode.D))
            moveDirection -= rightDirection;

        // Apply movement along the wall
        playerRigidbody.velocity = moveDirection.normalized * climbSpeed;

        // Release climbing
        if (Input.GetKey(KeyCode.E))
        {
            DisableClimbing();
            isClimbing = false;
            climbingLock = true;
        }

        climbTimeX += Time.deltaTime;
        climbTimeZ += Time.deltaTime;

        if (climbTimeX > maxClimbTimeX)
        {
            playerRigidbody.velocity = new Vector3(0.0f, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            climbTimeX = 0.0f;
        }
        if (climbTimeZ > maxClimbTimeZ)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y, 0.0f);
            climbTimeZ = 0.0f;
        }
    }

    private void SetClimbingState()
    {
        bool onWall = Physics.SphereCast(transform.position, 0.3f, orientation.forward, out hitWall, 1f, climbableWall);
        float angle = Vector3.Angle(orientation.forward, -hitWall.normal);

        SetClimbingLock();

        if (!onWall && isClimbing)
        {
            DisableClimbing();
            isClimbing = false;
        }
        else if (!playerMovement.touchGround && onWall && angle <= maxClimbAngle && !climbingLock)
        {
            isClimbing = true;
            buttonPromptsController.LoadMessage(climbingButtonsPrompts, "climbing");
        }
        else if (isClimbing)
        {
            DisableClimbing();
            isClimbing = false;
        }
        else
        {
            isClimbing = false;
        }
    }

    private void SetClimbingLock()
    {
        if (climbingLock)
        {
            if (playerMovement.touchGround)
            {
                climbingLock = false;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                climbingLock = false;
            }
        }
    }
}
