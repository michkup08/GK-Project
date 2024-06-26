using UnityEngine;

/// <summary>
/// Enables wall running mechanics for the player character.
/// </summary>
public class Wallrunning : MonoBehaviour
{
    /// <value>
    /// Layer mask to determine what is considered as a wall for wall running.
    /// </value>
    [Header("Wallrunning")]
    public LayerMask whatIsWall;

    /// <value>
    /// Layer mask to determine what is considered as ground for detecting jump height.
    /// </value>
    public LayerMask whatIsGround;

    /// <value>
    /// Force applied when wall running.
    /// </value>
    public float wallRunForce;

    /// <value>
    /// Maximum duration allowed for wall running.
    /// </value>
    public float maxWallRunTime;

    /// <value>
    /// Horizontal input axis value.
    /// </value>
    private float horizontalInput;

    /// <value>
    /// Vertical input axis value.
    /// </value>
    private float verticalInput;

    /// <value>
    /// Distance to check for walls.
    /// </value>
    [Header("Detection")]
    public float wallCheckDistance;

    /// <value>
    /// Minimum height for a jump to be considered valid for wall running.
    /// </value>
    public float minJumpHeight;

    /// <value>
    /// RaycastHit structure for the left wall detection.
    /// </value>
    private RaycastHit leftWallhit;

    /// <value>
    /// RaycastHit structure for the right wall detection.
    /// </value>
    private RaycastHit rightWallhit;

    /// <value>
    /// Boolean indicating if there is a wall detected on the left.
    /// </value>
    private bool wallLeft;

    /// <value>
    /// Boolean indicating if there is a wall detected on the right.
    /// </value>
    private bool wallRight;

    /// <value>
    /// Reference to the orientation transform.
    /// </value>
    [Header("References")]
    public Transform orientation;

    /// <value>
    /// Indicates if the player is currently wall running.
    /// </value>
    public bool wallrunning;

    /// <value>
    /// Reference to the Rigidbody component attached to the player.
    /// </value>
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((wallLeft || wallRight) && verticalInput > 0 && CanWallRun())
        {
            if (!wallrunning) wallrunning = true;
        }
        else
        {
            if (wallrunning) wallrunning = false;
        }
    }

    private void FixedUpdate()
    {
        if (wallrunning) WallRunningMovement();
    }

    /// <summary>
    /// Checks if the player can start wall running based on jump height and ground detection.
    /// </summary>
    /// <returns>True if the player can start wall running, false otherwise.</returns>
    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    /// <summary>
    /// Applies forces to the Rigidbody to simulate wall running movement.
    /// </summary>
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }
}
