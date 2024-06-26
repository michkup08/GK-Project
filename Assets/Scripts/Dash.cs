using UnityEngine;

/// <summary>
/// This class handles the dash ability for the player, including cooldown management and movement limits.
/// </summary>
public class Dash : MonoBehaviour
{
    [Header("References")]
    /// <value> Reference to the PlayerMovement script. </value>
    private PlayerMovement movement;
    /// <value> Reference to the camera transform. </value>
    public Transform cam;
    /// <value> Reference to the Rigidbody component. </value>
    private Rigidbody rb;
    /// <value> Reference to the player object. </value>
    public GameObject playerObject;

    [Header("Variables")]
    /// <value> Full cooldown duration for the dash ability. </value>
    public float fullCooldown = 1.5f;
    /// <value> Current active cooldown time. </value>
    public float activeCooldown = 0f;
    /// <value> Force applied during the dash. </value>
    public float dashForce = 800f;
    /// <value> Speed limit during the dash. </value>
    public float dashLimit = 80f;
    /// <value> Standard speed limit. </value>
    public float standardLimit = 25f;

    /// <summary>
    /// Initializes the Rigidbody and PlayerMovement components.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Updates the dash cooldown and checks for dash input each frame.
    /// </summary>
    void Update()
    {
        if (activeCooldown > 0)
        {
            activeCooldown -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                DashAbility();
            }
        }
    }

    /// <summary>
    /// Activates the dash ability, applying force and setting movement limits.
    /// </summary>
    private void DashAbility()
    {
        movement.moveSpeedLimit = dashLimit;
        Vector3 cameraEulerAngles = cam.eulerAngles;
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, cameraEulerAngles.y, transform.eulerAngles.z);
        playerObject.transform.eulerAngles = newRotation;

        Vector3 dashVector = cam.forward * dashForce;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(dashVector.x, 0, dashVector.z), ForceMode.Impulse);
        activeCooldown = fullCooldown;
        Invoke(nameof(resetLimit), fullCooldown);
    }

    /// <summary>
    /// Resets the player's movement speed limit to the standard limit.
    /// </summary>
    private void resetLimit()
    {
        movement.moveSpeedLimit = standardLimit;
    }
}
