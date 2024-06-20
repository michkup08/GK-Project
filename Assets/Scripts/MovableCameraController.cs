using UnityEngine;

/// <summary>
/// The MovableCameraController class handles the movement and rotation of a movable camera in the scene.
/// Movable camera is additional camera that is used to make different views of the scene for recording videos or taking screenshots.
/// </summary>
public class MovableCameraController : MonoBehaviour
{
    /// <value><c>cameraTransform</c> is a reference to the camera's Transform component.</value>
    private Transform cameraTransform;

    [Header("Movement")]
    /// <value><c>movementSpeed</c> is the speed at which the camera moves.</value>
    public float movementSpeed = 10f;

    /// <value><c>fastMovementSpeed</c> is the speed at which the camera moves when the shift key is held down.</value>
    public float fastMovementSpeed = 50f;

    /// <value><c>rotationSpeed</c> is the speed at which the camera rotates.</value>
    public float rotationSpeed = 5f;

    /// <value><c>objectToFollow</c> is the object that the camera will follow.</value>
    public Transform objectToFollow;

    /// <summary>
    /// The Start method is called before the first frame update. It sets the cameraTransform variable to the camera's Transform component.
    /// </summary>
    void Start()
    {
        cameraTransform = transform.Find("Camera");
    }


    /// <summary>
    /// The Update method is called once per frame. It handles camera movement and rotation based on user input.
    /// </summary>
    void Update()
    {
        if (objectToFollow == null)
        {
            // Check if left shift is held down and adjust movement speed accordingly
            float currentMovementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastMovementSpeed : movementSpeed;

            // Movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movementDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
            transform.position += movementDirection * currentMovementSpeed * Time.deltaTime;

            // Rotation
            if (Input.GetMouseButton(1)) // Right mouse button
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                Vector3 rotation = transform.eulerAngles;
                rotation.y += mouseX * rotationSpeed;
                rotation.x -= mouseY * rotationSpeed;

                transform.eulerAngles = rotation;
            }
        }
        else
        {
            transform.position = objectToFollow.position + new Vector3(0, 4, -5);
            transform.rotation = objectToFollow.rotation;
        }
    }
}