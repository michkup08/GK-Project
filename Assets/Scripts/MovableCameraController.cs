using UnityEngine;

public class MovableCameraController : MonoBehaviour
{
    /// <value><c>cameraTransform</c> is a reference to the camera's Transform component.</value>
    private Transform cameraTransform;

    [Header("Movement")]
    /// <value><c>movementSpeed</c> is the speed at which the camera moves.</value>
    public float movementSpeed = 10f;

    /// <value><c>fastMovementSpeed</c> is the speed at which the camera moves when the shift key is held down.</value>
    public float fastMovementSpeed = 20f;

    /// <value><c>rotationSpeed</c> is the speed at which the camera rotates.</value>
    public float rotationSpeed = 5f;

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
}