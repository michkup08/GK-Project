using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LineMovement : MonoBehaviour
{
    [Header("Drop Line Timer")]
    private float doubleSpaceKeyPressTime = 0.0f;

    [SerializeField]
    private float doubleSpaceKeyPressTimeMax = 0.5f;
    private bool spaceKeyPressed = false;

    [Header("Moving Timer")]
    private float moveTime = 0.0f;
    private float moveTimeMax = 0.1f;

    [Header("Speed")]
    [SerializeField]
    private float speed = 3.0f;
    bool isMovingOnLine = false;

    [Header("ObjectsRef")]
    [SerializeField]
    private Transform playerOrientation;

    private Rigidbody playerRigidbody;
    private int direction = 1;
    private bool isOnLine = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableUnderLine"))
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.GetContact(0);
                isOnLine = (Vector3.Dot(contact.normal, Vector3.up) > 0.5);
                isMovingOnLine = (Vector3.Dot(contact.normal, Vector3.down) > 0.5) || (Vector3.Dot(contact.normal, Vector3.up) > 0.5);
                if (isMovingOnLine)
                {
                    playerRigidbody.useGravity = false;
                    playerRigidbody.drag = 0;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableUnderLine"))
        {
            playerRigidbody.useGravity = true;
            isMovingOnLine = false;
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (spaceKeyPressed)
                {
                    if (doubleSpaceKeyPressTime <= doubleSpaceKeyPressTimeMax)
                    {
                        playerRigidbody.useGravity = true;
                        isMovingOnLine = false;
                    }
                }
                spaceKeyPressed = true;
            }

            if (moveTime >= moveTimeMax)
            {
                playerRigidbody.velocity = Vector3.zero;
            }

            if (spaceKeyPressed)
            {
                doubleSpaceKeyPressTime += Time.deltaTime;
                if (doubleSpaceKeyPressTime > doubleSpaceKeyPressTimeMax)
                {
                    spaceKeyPressed = false;
                    doubleSpaceKeyPressTime = 0.0f;
                }
            }
            moveTime += Time.deltaTime;
        }
    }
}
