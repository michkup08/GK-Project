using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the third-person camera behavior, including aiming and default camera modes.
/// </summary>
public class Camera3P : MonoBehaviour
{
    [Header("ObjectsRef")]
    /// <value> Reference to the orientation transform. </value>
    public Transform orientation;
    /// <value> Reference to the player transform. </value>
    public Transform player;
    /// <value> Reference to the player object transform. </value>
    public Transform playerObject;
    /// <value> Reference to the Camera component. </value>
    public Camera cam;

    [Header("Variables")]
    /// <value> Speed of camera rotation. </value>
    public float rotationSpeed;
    /// <value> Direction the camera is looking at. </value>
    public Transform lookDir;

    /// <value> Horizontal input value. </value>
    [SerializeField]
    float horizontalI;
    /// <value> Vertical input value. </value>
    [SerializeField]
    float verticalI;
    /// <value> Indicates if aiming mode is active. </value>
    public bool aim;
    /// <value> UI Image to display when aiming. </value>
    public Image aimImage;

    /// <value> Indicates if rotation is disabled. </value>
    public bool disableRotation;

    /// <value> Default Cinemachine camera for third-person view. </value>
    public CinemachineFreeLook defaultCinemachine;
    /// <value> Cinemachine camera for aiming view. </value>
    public CinemachineFreeLook aimCinemachine;

    /// <summary>
    /// Initializes the camera settings and variables.
    /// </summary>
    void Start()
    {
        aim = false;
        aimImage.enabled = aim;
        defaultCinemachine.Priority = 1;
        aimCinemachine.Priority = 0;
    }

    /// <summary>
    /// Updates the camera behavior each frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            aim = !aim;

            if (aim)
            {
                defaultCinemachine.Priority = 0;
                aimCinemachine.Priority = 1;
            }
            else
            {
                defaultCinemachine.Priority = 1;
                aimCinemachine.Priority = 0;
            }
            aimImage.enabled = aim;
        }

        if (!aim)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            horizontalI = Input.GetAxis("Horizontal");
            verticalI = Input.GetAxis("Vertical");

            Vector3 inputDir = (orientation.forward * verticalI) + (orientation.right * horizontalI);
            if (inputDir.magnitude != 0 && !disableRotation)
            {
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            Vector3 lookAtDir = (lookDir.position - new Vector3(transform.position.x, lookDir.position.y, transform.position.z)).normalized;
            orientation.forward = lookAtDir;
            playerObject.forward = lookAtDir;
        }
    }
}
