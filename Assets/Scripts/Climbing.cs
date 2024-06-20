using UnityEngine;

/// <summary>
/// Handles the climbing mechanics for the player.
/// </summary>
public class Climbing : MonoBehaviour
{
    [Header("References")]
    /// <value><c>orientation</c> player's orientation object.</value>
    [SerializeField]
    Transform orientation;
    /// <value><c>climbableWall</c> layer of climbable walls.</value>
    [SerializeField]
    LayerMask climbableWall;

    [Header("Climbing")]
    /// <value><c>climbSpeed</c> the speed of climbing.</value>
    [SerializeField]
    float climbSpeed = 1.5f;
    /// <value><c>maxClimbAngle</c> maximum angle to recognize climbing and walking on the wall.</value>
    [SerializeField]
    float maxClimbAngle = 30.0f;

    /// <value><c>originalDrag</c> original drag before start climbing.</value>
    private float originalDrag;
    /// <value><c>isClimbing</c> the state of climbing.</value>
    private bool isClimbing = false;

    [Header("Climbing Timer")]
    /// <value><c>maxClimbTimeX</c> the maximum climbing times in the X direction.</value>
    [SerializeField]
    float maxClimbTimeX = 0.4f;
    /// <value><c>maxClimbTimeZ</c> the maximum climbing times in the Z direction.</value>
    [SerializeField]
    float maxClimbTimeZ = 0.4f;

    /// <value><c>hitWall</c> RaycastHit to store information about the wall on which the player is climbing.</value>
    private RaycastHit hitWall;

    /// <value><c>climbingButtonsPrompts</c> text for climbing button prompts.</value>
    private readonly string[] climbingButtonsPrompts =
    {
        "press <sprite name=\"E\"> to let go",
        "move with <sprite name=\"W\"> <sprite name=\"A\"> <sprite name=\"S\"> <sprite name=\"D\">"
    };

    /// <value><c>buttonPromptsController</c> controller for button prompts on the screen.</value>
    ScreenHints buttonPromptsController;

    /// <value><c>climbTimeX</c> and <c>climbTimeZ</c> current climbing time in X and Z directions.</value>
    private float climbTimeX = 0.0f;
    private float climbTimeZ = 0.0f;

    /// <value><c>playerMovement</c> reference to PlayerMovement to access touchGround variable.</value>
    PlayerMovement playerMovement;

    /// <value><c>climbingLock</c> lock for climbing.</value>
    private bool climbingLock = false;

    /// <value><c>playerRigidbody</c> the Rigidbody of the player.</value>
    Rigidbody playerRigidbody;

    /// <summary>
    /// Set properties values at the start of the game.
    /// </summary>
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Set climbing state and handle climbing mechanics.
    /// </summary>
    void Update()
    {
        SetClimbingState();
        if (isClimbing)
        {
            originalDrag = playerRigidbody.drag;
            Climb();
        }
    }

    /// <summary>
    /// Disables climbing.
    /// </summary>
    private void DisableClimbing()
    {
        playerRigidbody.useGravity = true;

        if (!playerMovement.touchGround)
            playerRigidbody.drag = 0;
        else
            playerRigidbody.drag = originalDrag;
    }

    /// <summary>
    /// Handles climbing mechanics.
    /// </summary>
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

        // Get input and set movement direction
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

    /// <summary>
    /// Sets the climbing state based on various conditions.
    /// </summary>
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

    /// <summary>
    /// Resets the climbing lock based on various conditions.
    /// </summary>
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
