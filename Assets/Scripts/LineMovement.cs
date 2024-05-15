using System.IO.Pipes;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LineMovement : MonoBehaviour
{
    [Header("Moving Timer")]
    private float moveTime = 0.0f;
    private float moveTimeMax = 0.1f;

    [Header("Speed")]
    [SerializeField]
    private float speed = 3.0f;
    public bool isMovingOnLine = false;

    [Header("References")]
    [SerializeField]
    private Transform playerOrientation;

    private PlayerMovement playerMovement;

    private Vector3 lineDirection;

    ScreenHints buttonPromptsController;

    private readonly string[] movingAboveLineButtonPrompts =
    {
        "press <sprite name=\"W\"> and <sprite name=\"S\"> to move forward and backward"
    };

    private readonly string[] movingUnderLineButtonPrompts =
    {
        "press <sprite name=\"E\"> to let go",
        "press <sprite name=\"W\"> and <sprite name=\"S\"> to move forward and backward"
    };

    private Rigidbody playerRigidbody;
    private int direction = 1;
    private bool isAboveLine = false;

    private void calculateLineDirection(Collision collision)
    {
        CapsuleCollider lineCollider = collision.gameObject.GetComponent<CapsuleCollider>();

        // Get the line's length (subtract the diameter of the end cap spheres)
        float lineLength = lineCollider.height - lineCollider.radius * 2;

        // Calculate the start and end points
        Vector3 startPoint = Vector3.zero, endPoint = Vector3.zero;
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

        // Calculate the direction of the line
        lineDirection = (endPoint - startPoint).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableLine"))
        {
            calculateLineDirection(collision);
            if (collision.contacts.Length > 0)
            {
                playerMovement.disableAirMovement();
                ContactPoint contact = collision.GetContact(0);
                float dotUp = Vector3.Dot(contact.normal, Vector3.up);
                float dotDown = Vector3.Dot(contact.normal, Vector3.down);
                isAboveLine = (dotUp > 0.5f);
                bool isUnderLine = (dotDown > 0.5f);
                isMovingOnLine = isUnderLine || isAboveLine;

                if (isMovingOnLine)
                {
                    playerRigidbody.useGravity = false;
                    playerRigidbody.drag = 0;
                }

                if (isAboveLine)
                {
                    buttonPromptsController.LoadMessage(movingAboveLineButtonPrompts, "aboveLineMovement");
                }
                else if (isUnderLine)
                {
                    buttonPromptsController.LoadMessage(movingUnderLineButtonPrompts, "underLineMovement");
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableLine"))
        {
            playerRigidbody.useGravity = true;
            isMovingOnLine = false;
            playerMovement.enableAirMovement();
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
        playerMovement = playerOrientation.parent.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isMovingOnLine)
        {
            var angle = Vector3.Angle(playerOrientation.forward, lineDirection);
            direction = angle > 90 ? -1 : 1;
            if (Input.GetKey(KeyCode.W))
            {
                moveTime = 0.0f;
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, Quaternion.LookRotation(lineDirection), Time.deltaTime);
                playerRigidbody.velocity = direction * speed * lineDirection;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveTime = 0.0f;
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, Quaternion.LookRotation(lineDirection), Time.deltaTime);
                playerRigidbody.velocity = direction * speed * -lineDirection;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!isAboveLine)
                {
                    playerRigidbody.useGravity = true;
                    isMovingOnLine = false;
                }
            }

            if (moveTime >= moveTimeMax)
            {
                playerRigidbody.velocity = Vector3.zero;
            }

            moveTime += Time.deltaTime;
        }
    }
}
