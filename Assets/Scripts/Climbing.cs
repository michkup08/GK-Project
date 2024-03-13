using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    /// Player orientation
    public Transform orientation;
    /// Climbable layer
    public LayerMask climbableWall;
    /// Player movement reference to access the rigidbody
    public PlayerMovement playerMovement;

    [Header("Climbing")]
    /// Climb speed
    public float climbSpeed = 1.5f;
    /// max climb angle (to recognize climbing and walking on the wall)
    public float maxClimbAngle = 30.0f;

    /// drag before start climbing
    private float originalDrag;
    /// climbing state
    bool isClimbing = false;

    [Header("Climbing Timer")]
    /// max climbing times
    public float maxClimbTimeX = 0.4f;
    public float maxClimbTimeZ = 0.4f;
    /// climbing movement timers
    private float climbTimeX = 0.0f;
    private float climbTimeZ = 0.0f;


    void Start()
    {

    }

    void Update()
    {
        SetClimbingState();
        if (isClimbing)
        {
            originalDrag = playerMovement.rigidbody.drag;
            Climb();
        }
        else
        {
            DisableClimbing();
        }
    }

    private void DisableClimbing()
    {
        playerMovement.rigidbody.useGravity = true;

        if (!playerMovement.touchGround)
            playerMovement.rigidbody.drag = 0;
        else
            playerMovement.rigidbody.drag = originalDrag;

    }

    private void Climb()
    {
        playerMovement.rigidbody.useGravity = false;
        playerMovement.rigidbody.drag = 0;

        var currentVelocity = playerMovement.rigidbody.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            climbTimeX = 0.0f;
            playerMovement.rigidbody.velocity = new Vector3(0.0f, climbSpeed, currentVelocity.z);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            climbTimeX = 0.0f;
            playerMovement.rigidbody.velocity = new Vector3(0.0f, -climbSpeed, currentVelocity.z);
        }
        //else
        //{
        //    playerMovement.rigidbody.velocity = new Vector3(0.0f, 0.0f, currentVelocity.z);
        //    Debug.Log("velocity"+playerMovement.rigidbody.velocity);
        //}

        if (Input.GetKey(KeyCode.A))
        {
            climbTimeZ = 0.0f;
            playerMovement.rigidbody.velocity = new Vector3(0.0f, 0.0f, climbSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            climbTimeZ = 0.0f;
            playerMovement.rigidbody.velocity = new Vector3(0.0f, 0.0f, -climbSpeed);
        }
        //else
        //{
        //    playerMovement.rigidbody.velocity = new Vector3(0.0f, currentVelocity.y, 0.0f);
        //}

        climbTimeX += Time.deltaTime;
        climbTimeZ += Time.deltaTime;

        if (climbTimeX > maxClimbTimeX)
        {
            playerMovement.rigidbody.velocity = new Vector3(0.0f, 0.0f, currentVelocity.z);
            climbTimeX = 0.0f;
        }
        if (climbTimeZ > maxClimbTimeZ)
        {
            playerMovement.rigidbody.velocity = new Vector3(0.0f, currentVelocity.y, 0.0f);
            climbTimeZ = 0.0f;
        }
    }

    private void SetClimbingState()
    {
        bool onWall = Physics.SphereCast(transform.position, 0.3f, orientation.forward, out RaycastHit hitWall, 1f, climbableWall);
        float angle = Vector3.Angle(orientation.forward, -hitWall.normal);

        if (!onWall)
        {
            isClimbing = false;
        }
        else if (!playerMovement.touchGround && onWall && angle <= maxClimbAngle)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }
    }
}
