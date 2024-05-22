using System.IO.Pipes;
using System.Net;
using UnityEngine;

/// <summary>
/// The Ziplining class handles the player's interaction with ziplines in the game.
/// </summary>
public class Ziplining : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField]
    /// <value><c>speed</c> is the speed at which the player moves along the zipline.</value>
    private float speed = 3.0f;

    /// <value><c>playerRigidbody</c> is the player's Rigidbody component.</value>
    private Rigidbody playerRigidbody;

    /// <value><c>isZiplining</c> is a flag indicating whether the player is currently ziplining.</value>
    private bool isZiplining = false;

    /// <value><c>startPoint</c> is the start point needed to calculate the direction of the zipline.</value>
    private Vector3 startPoint;

    /// <value><c>endPoint</c> is the end point needed to calculate the direction of the zipline.</value>
    private Vector3 endPoint;

    /// <value><c>endLine</c> is the end (lowest) point of the zipline.</value>
    private Vector3 endLine;

    /// <value><c>playerMovement</c> is the player's movement component.</value>
    private PlayerMovement playerMovement;

    /// <value><c>zipliningButtonsPrompts</c> is the button prompt displayed to the player when they are ziplining.</value>
    private readonly string[] zipliningButtonsPrompts =
    {
        "press <sprite name=\"E\"> to let go"
    };

    /// <value><c>buttonPromptsController</c> is the controller for the button prompts displayed to the player.</value>
    private ScreenHints buttonPromptsController;

    /// <summary>
    /// It initializes the player's Rigidbody, ScreenHints, and PlayerMovement components at the start of the game.
    /// </summary>
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
        playerMovement = playerRigidbody.GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Calculates the direction of the zipline based on the collision.
    /// </summary>
    private void calculateLineDirection(Collision collision)
    {
        CapsuleCollider lineCollider = collision.gameObject.GetComponent<CapsuleCollider>();

        // Get the line's length (subtract the diameter of the end cap spheres)
        float lineLength = lineCollider.height - lineCollider.radius * 2;

        // Calculate the start and end points
        switch (lineCollider.direction)
        {
            case 0: // X-axis
                startPoint = collision.transform.position - collision.transform.right * lineLength / 2;
                endPoint = collision.transform.position + collision.transform.right * lineLength / 2;
                break;
            case 1: // Y-axis
                startPoint = collision.transform.position - collision.transform.up * lineLength / 2;
                endPoint = collision.transform.position + collision.transform.up * lineLength / 2;
                break;
            case 2: // Z-axis
                startPoint = collision.transform.position - collision.transform.forward * lineLength / 2;
                endPoint = collision.transform.position + collision.transform.forward * lineLength / 2;
                break;
        }

        // Check if end point is below start point
        if (endPoint.y > startPoint.y)
        {
            Vector3 temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }
    }

    /// <summary>
    /// Checks if the player is in contact with a zipline.
    /// It sets the ziplining.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("zippableLine"))
        {
            if (collision.contacts.Length > 0)
            {
                playerMovement.disableAirMovement();
                ContactPoint contact = collision.GetContact(0);
                isZiplining = Vector3.Dot(contact.normal, Vector3.up) > 0.7f;
                playerRigidbody.useGravity = false;
                playerRigidbody.drag = 0f;
                isZiplining = true;
                endLine = collision.gameObject.GetComponent<MeshRenderer>().bounds.min;
                calculateLineDirection(collision);
                buttonPromptsController.LoadMessage(zipliningButtonsPrompts, "ziplining");
            }
        }
    }

    /// <summary>
    /// Handles the player's disengagement from the zipline.
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("zippableLine"))
        {
            playerRigidbody.useGravity = true;
            isZiplining = false;
            playerMovement.enableAirMovement();
        }
    }

    /// <summary>
    /// Handles the player's movement along the zipline and disengagement from the zipline.
    /// </summary>
    void Update()
    {
        if (isZiplining)
        {
            Vector3 direction = (endPoint - startPoint).normalized;
            playerRigidbody.velocity = speed * direction;

            if (Vector3.Distance(transform.position, endLine) < 1.6f)
            {
                playerRigidbody.useGravity = true;
                isZiplining = false;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                playerRigidbody.useGravity = true;
                isZiplining = false;
            }
        }
    }

    /// <summary>
    /// Checks if the player is currently ziplining.
    /// </summary>
    public bool IsZiplining()
    {
        return isZiplining;
    }
}