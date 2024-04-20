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
    private bool isMovingOnLine = false;

    [Header("References")]
    [SerializeField]
    private Transform playerOrientation;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableLine"))
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.GetContact(0);
                isAboveLine = (Vector3.Dot(contact.normal, Vector3.up) > 0.5);
                bool isUnderLine = (Vector3.Dot(contact.normal, Vector3.down) > 0.5);
                isMovingOnLine = isUnderLine || isAboveLine;
                if (isMovingOnLine)
                {
                    playerRigidbody.useGravity = false;
                    playerRigidbody.drag = 0;
                    buttonPromptsController.LoadMessage(movingAboveLineButtonPrompts, "aboveLineMovement");
                }
                else
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
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
    }

    void Update()
    {
        if (isMovingOnLine)
        {
            var angle = Vector3.Angle(playerOrientation.forward, Vector3.forward);
            direction = angle > 90 ? -1 : 1;
            if (Input.GetKey(KeyCode.W))
            {
                moveTime = 0.0f;
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, Quaternion.Euler(0.0f, 90.0f * direction, 0.0f), Time.deltaTime);
                playerRigidbody.velocity = new Vector3(0.0f, 0.0f, speed * direction);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveTime = 0.0f;
                playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, Quaternion.Euler(0.0f, 90.0f * direction, 0.0f), Time.deltaTime);
                playerRigidbody.velocity = new Vector3(0.0f, 0.0f, -speed * direction);
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
