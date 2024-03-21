using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MoveUnderLine : MonoBehaviour
{
    [Header("Drop Rope Timer")]
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
    bool isMovingUnderLine = false;

    private Rigidbody playerRigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableUnderLine"))
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.GetContact(0);
                if (Vector3.Dot(contact.normal, Vector3.down) > 0.5)
                {
                    playerRigidbody.useGravity = false;
                    playerRigidbody.drag = 0;
                    isMovingUnderLine = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("movableUnderLine"))
        {
            playerRigidbody.useGravity = true;
            isMovingUnderLine = false;
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isMovingUnderLine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveTime = 0.0f;
                playerRigidbody.velocity = new Vector3(0.0f, 0.0f, speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveTime = 0.0f;
                playerRigidbody.velocity = new Vector3(0.0f, 0.0f, -speed);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (spaceKeyPressed)
                {
                    if (doubleSpaceKeyPressTime <= doubleSpaceKeyPressTimeMax)
                    {
                        playerRigidbody.useGravity = true;
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
