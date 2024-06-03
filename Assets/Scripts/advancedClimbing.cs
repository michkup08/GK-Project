using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class advancedClimbing : MonoBehaviour
{
    [Header("references")]
    Rigidbody rigidbody;
    public Camera cam;

    [Header("variables")]
    public float afterHandleJumpForce;
    public float sphereCastR;
    public float hitingLenght;
    public float handleAfterJumpDelay;
    public LayerMask Handler;
    RaycastHit hit;

    private LineMovement lineMovement;
    private Ziplining ziplining;

    public bool canHandle, stopHolding;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lineMovement = GetComponent<LineMovement>();
        ziplining = GetComponent<Ziplining>();

        canHandle = false;
        stopHolding = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (canHandle)
        {
            if (rigidbody.useGravity == true)
            {
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                canHandle = false;
                rigidbody.useGravity = true;
                rigidbody.AddForce(cam.gameObject.transform.forward * afterHandleJumpForce, ForceMode.Impulse);


            }
        }
        else if (!lineMovement.IsMovingOnLine() && !ziplining.IsZiplining())
        {
            if (rigidbody.useGravity == false)
            {
                canHandle = false;
                rigidbody.useGravity = true;
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            Vector3 direction = other.transform.position - transform.position;
            Vector3 currentRotation = transform.rotation.eulerAngles;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float targetYRotation = targetRotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(currentRotation.x, targetYRotation, currentRotation.z);

            canHandle = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            canHandle = false;
        }
    }

}
