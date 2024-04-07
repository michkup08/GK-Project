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
    /// Player movement reference to access touchGround variable
    [SerializeField]
    PlayerMovement playerMovement;

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

    /// climbing movement timers
    private float climbTimeX = 0.0f;
    private float climbTimeZ = 0.0f;

    private bool climbingLock = false;

    Rigidbody playerRigidbody;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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

        var currentVelocity = playerRigidbody.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            climbTimeX = 0.0f;
            playerRigidbody.velocity = new Vector3(0.0f, climbSpeed, currentVelocity.z);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            climbTimeX = 0.0f;
            playerRigidbody.velocity = new Vector3(0.0f, -climbSpeed, currentVelocity.z);
        }

        if (Input.GetKey(KeyCode.A))
        {
            climbTimeZ = 0.0f;
            playerRigidbody.velocity = new Vector3(0.0f, 0.0f, climbSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            climbTimeZ = 0.0f;
            playerRigidbody.velocity = new Vector3(0.0f, 0.0f, -climbSpeed);
        }

        // release climbing
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
            playerRigidbody.velocity = new Vector3(0.0f, 0.0f, currentVelocity.z);
            climbTimeX = 0.0f;
        }
        if (climbTimeZ > maxClimbTimeZ)
        {
            playerRigidbody.velocity = new Vector3(0.0f, currentVelocity.y, 0.0f);
            climbTimeZ = 0.0f;
        }
    }

    private void SetClimbingState()
    {
        bool onWall = Physics.SphereCast(transform.position, 0.3f, orientation.forward, out RaycastHit hitWall, 1f, climbableWall);
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
