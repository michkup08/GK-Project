using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// The LineMovement class handles the player's movement along movable lines in the game.
/// </summary>
public class LineMovement : MonoBehaviour
{
    [Header("Moving Timer")]
    /// <value><c>moveTime</c> is the timer for the player's movement along the line after releasing the movement keys.</value>
    private float moveTime = 0.0f;
    /// <value><c>moveTimeMax</c> is the maximum time for the player's movement along the line after releasing the movement keys.</value>
    private float moveTimeMax = 0.1f;

    [Header("Speed")]
    [SerializeField]
    /// <value><c>speed</c> is the speed at which the player moves along the line.</value>
    private float speed = 3.0f;

    /// <value><c>isMovingOnLine</c> is a flag indicating whether the player is currently moving on the line.</value>
    public bool isMovingOnLine = false;

    [Header("References")]
    [SerializeField]
    /// <value><c>playerOrientation</c> is the player's orientation in the game world.</value>
    private Transform playerOrientation;

    [SerializeField]
    /// <value><c>camera3P</c> is reference to the Camera3P component, which is used to block the player's rotation when moving on the line.</value>
    public Camera3P camera3P;

    /// <value><c>playerMovement</c> is the player's movement component, which is used to disable and enable the player's air movement.</value>
    private PlayerMovement playerMovement;

    /// <value><c>lineDirection</c> is the direction of the line.</value>
    private Vector3 lineDirection;

    /// <value><c>buttonPromptsController</c> is the controller for the button prompts displayed to the player.</value>
    ScreenHints buttonPromptsController;

    /// <value><c>movingAboveLineButtonPrompts</c> is the button prompt displayed to the player when he is moving above the line.</value>
    private readonly string[] movingAboveLineButtonPrompts =
    {
        "press <sprite name=\"W\"> and <sprite name=\"S\"> to move forward and backward"
    };

    /// <value><c>movingUnderLineButtonPrompts</c> is the button prompt displayed to the player when he is moving under the line.</value>
    private readonly string[] movingUnderLineButtonPrompts =
    {
        "press <sprite name=\"E\"> to let go",
        "press <sprite name=\"W\"> and <sprite name=\"S\"> to move forward and backward"
    };

    /// <value><c>playerRigidbody</c> is the player's Rigidbody component.</value>
    private Rigidbody playerRigidbody;

    /// <value><c>direction</c> is the direction in which the player is moving along the line. It is used to move the player in current looking direction.</value>
    public int direction = 1;

    /// <value><c>isAboveLine</c> is a flag indicating whether the player is currently above the line.</value>
    public bool isAboveLine = false;

    /// <summary>
    /// Calculates the direction of the line based on the collision.
    /// </summary>
    private void calculateLineDirection(Collision collision)
    {
        CapsuleCollider lineCollider = collision.gameObject.GetComponent<CapsuleCollider>();
        if (lineCollider == null) return; // Early exit if the collider is not found

        // Calculate the line's direction based on its orientation
        Vector3 lineVector = Vector3.zero;
        switch (lineCollider.direction)
        {
            case 0: // X-axis
                lineVector = collision.transform.right;
                break;
            case 1: // Y-axis
                lineVector = collision.transform.up;
                break;
            case 2: // Z-axis
                lineVector = collision.transform.forward;
                break;
        }

        // Calculate the start and end points of the line
        Vector3 startPoint = collision.transform.position - (lineVector * lineCollider.height / 2);
        Vector3 endPoint = collision.transform.position + (lineVector * lineCollider.height / 2);

        // Adjust for the collider's center offset
        startPoint += lineCollider.center;
        endPoint += lineCollider.center;

        // Calculate the direction of the line
        lineDirection = (endPoint - startPoint).normalized;
    }

    /// <summary>
    /// It handles the player's interaction with the movable line.
    /// Sets the player's movement along the line and displays the button prompts to the player.
    /// </summary>
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
                isAboveLine = dotUp > 0.5f;
                bool isUnderLine = dotDown > 0.5f;
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
                    // disable player rotation towards the camera and set player rotation 90 degrees in y axis to the line direction
                    camera3P.disableRotation = true;
                    var playerObject = transform.Find("Ch24_nonPBR");
                    playerObject.transform.rotation = Quaternion.LookRotation(lineDirection) * Quaternion.Euler(0, 90, 0);
                }
            }
        }
    }

    /// <summary>
    /// It is called when player is no longer in contact with the movable line.
    /// It handles the player's disengagement from the movable line.
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableLine"))
        {
            playerRigidbody.useGravity = true;
            isMovingOnLine = false;
            playerMovement.enableAirMovement();
            camera3P.disableRotation = false;
        }
    }

    /// <summary>
    /// It initializes the player's Rigidbody, ScreenHints, and PlayerMovement components at the start of the game.
    /// </summary>
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
        playerMovement = playerOrientation.parent.GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// It handles the player's movement along the movable line and disengagement from the movable line.
    /// </summary>
    void Update()
    {
        if (isMovingOnLine)
        {
            var angle = Vector3.Angle(playerOrientation.forward, lineDirection);
            direction = angle > 90 ? -1 : 1;
            if (Input.GetKey(KeyCode.W))
            {
                moveTime = 0.0f;
                // playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, Quaternion.LookRotation(lineDirection), Time.deltaTime);
                playerRigidbody.velocity = direction * speed * lineDirection;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveTime = 0.0f;
                //playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, Quaternion.LookRotation(lineDirection), Time.deltaTime);
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

    /// <summary>
    /// IsMovingOnLine checks if the player is currently moving on the line.
    /// </summary>
    public bool IsMovingOnLine()
    {
        return isMovingOnLine;
    }
}