using UnityEngine;

public class Ziplining : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField]
    private float speed = 3.0f;

    private Rigidbody playerRigidbody;
    private bool isZiplining = false;

    private Vector3 minPoint;

    private readonly string[] zipliningButtonsPrompts =
    {
        "press <sprite name=\"E\"> to let go"
    };

    ScreenHints buttonPromptsController;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        buttonPromptsController = GetComponent<ScreenHints>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("zippableLine"))
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.GetContact(0);
                isZiplining = Vector3.Dot(contact.normal, Vector3.up) > 0.7f;
                playerRigidbody.useGravity = false;
                playerRigidbody.drag = 0f;
                isZiplining = true;
                minPoint = collision.gameObject.GetComponent<MeshRenderer>().bounds.min;
                buttonPromptsController.LoadMessage(zipliningButtonsPrompts, "ziplining");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("zippableLine"))
        {
            playerRigidbody.useGravity = true;
            isZiplining = false;
        }
    }

    void Update()
    {
        if (isZiplining)
        {
            Vector3 direction = (minPoint - transform.position).normalized;
            playerRigidbody.velocity = speed * direction;

            if (Vector3.Distance(transform.position, minPoint) < 1.5f)
            {
                playerRigidbody.useGravity = true;
                isZiplining = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            playerRigidbody.useGravity = true;
            isZiplining = false;
        }

    }
}
