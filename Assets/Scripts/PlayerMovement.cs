using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Objects Ref")]
    public Transform orientation;
    public LayerMask isGround;

    [Header("Variables")]
    public float moveSpeedMultipler;
    public float moveSpeedLimit;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    public float velocity; //for ui purpose

    [Header("private")]
    [SerializeField]
    float horizontalI;
    [SerializeField]
    float verticalI;
    [SerializeField]
    float playerHeight;
    [SerializeField]
    public bool touchGround;
    [SerializeField]
    bool readyToJump;
    Vector3 moveDir;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        readyToJump = true;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        touchGround = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, 1f, isGround);

        inputControl();
        speedLimit();

        if (touchGround)
        {
            rigidbody.drag = groundDrag;
        }
        else
        {
            rigidbody.drag = 0;
        }

        velocity = rigidbody.velocity.magnitude;
    }

    private void FixedUpdate()
    {
        if (touchGround)
            groundMovement();
    }

    private void LateUpdate()
    {

    }

    private void inputControl()
    {
        horizontalI = Input.GetAxisRaw("Horizontal");
        verticalI = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && touchGround && readyToJump)
        {
            readyToJump = false;
            jump();
            Invoke(nameof(afterJump), jumpCooldown);
            touchGround = false;
        }
    }

    private void speedLimit()
    {
        Vector2 rbSpeed = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);
        if (rbSpeed.magnitude > moveSpeedLimit)
        {
            rbSpeed = rbSpeed.normalized * moveSpeedMultipler;
            rigidbody.velocity = new Vector3(rbSpeed.x, rigidbody.velocity.y, rbSpeed.y);
        }
    }

    private void groundMovement()
    {
        moveDir = orientation.forward * verticalI + orientation.right * horizontalI;
        if (moveDir != Vector3.zero && transform.parent != null)
        {
            transform.parent = null;
        }
        rigidbody.AddForce(moveDir.normalized * moveSpeedMultipler, ForceMode.Force);
    }

    private void jump()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void afterJump()
    {
        readyToJump = true;
    }
}
