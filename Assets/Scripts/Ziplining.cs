using System.IO.Pipes;
using System.Net;
using UnityEngine;

public class Ziplining : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField]
    private float speed = 3.0f;

    private Rigidbody playerRigidbody;
    private bool isZiplining = false;

    private Vector3 startPoint, endPoint;
    private Vector3 endLine;

    private PlayerMovement playerMovement;

    private readonly string[] zipliningButtonsPrompts =
    {
        "press <sprite name=\"E\"> to let go"
    };

    ScreenHints buttonPromptsController;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
        playerMovement = playerRigidbody.GetComponent<PlayerMovement>();
    }

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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("zippableLine"))
        {
            playerRigidbody.useGravity = true;
            isZiplining = false;
            playerMovement.enableAirMovement();
        }
    }

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
}
