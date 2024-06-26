using UnityEngine;

/// <summary>
/// This class handles advanced climbing mechanics, including line movement and ziplining.
/// </summary>
public class advancedClimbing : MonoBehaviour
{
    [Header("references")]
    /// <value> Reference to the Rigidbody component. </value>
    Rigidbody rigidbody;
    /// <value> Reference to the Camera component. </value>
    public Camera cam;

    [Header("variables")]
    /// <value> Force applied after jumping off a handle. </value>
    public float afterHandleJumpForce;
    /// <value> Radius of the sphere cast used for detection. </value>
    public float sphereCastR;
    /// <value> Length of the hit detection. </value>
    public float hitingLenght;
    /// <value> Delay after jumping off a handle. </value>
    public float handleAfterJumpDelay;
    /// <value> LayerMask for detecting handlers. </value>
    public LayerMask Handler;
    /// <value> RaycastHit for detecting collisions. </value>
    RaycastHit hit;

    /// <value> Reference to the LineMovement component. </value>
    private LineMovement lineMovement;
    /// <value> Reference to the Ziplining component. </value>
    private Ziplining ziplining;

    /// <value> Indicates if the player can handle. </value>
    public bool canHandle;
    /// <value> Indicates if the player should stop holding. </value>
    public bool stopHolding;

    /// <summary>
    /// Initializes the necessary components and variables.
    /// </summary>
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lineMovement = GetComponent<LineMovement>();
        ziplining = GetComponent<Ziplining>();

        canHandle = false;
        stopHolding = false;
    }

    /// <summary>
    /// Updates the climbing logic each frame.
    /// </summary>
    void Update()
    {
        if (canHandle)
        {
            if (rigidbody.useGravity == true)
            {
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                canHandle = false;
                rigidbody.useGravity = true;
                rigidbody.AddForce(cam.gameObject.transform.forward * afterHandleJumpForce, ForceMode.Impulse);
            }
        }
        else if (!lineMovement.IsMovingOnLine() && !ziplining.IsZiplining())
        {
            if (rigidbody.useGravity == false)
            {
                canHandle = false;
                rigidbody.useGravity = true;
            }
        }
    }

    /// <summary>
    /// Called when the collider enters a collision.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            // Get the position of the object that was collided with
            Vector3 collisionPosition = other.transform.position;

            // Get the position of the object to which this script is attached
            Vector3 objectPosition = transform.position;

            // Calculate the direction to the object that was collided with
            Vector3 dir = collisionPosition - objectPosition;

            // Set the y value to 0 to focus only on rotation around the y-axis
            dir.y = 0;

            // Calculate the new rotation
            Quaternion rot = Quaternion.LookRotation(dir);

            // Set the new rotation
            transform.rotation = rot;

            canHandle = true;
        }
    }

    /// <summary>
    /// Called when the collider exits a collision.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            canHandle = false;
        }
    }
}
