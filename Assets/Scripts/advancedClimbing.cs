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

    bool canHandle, stopHolding;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        canHandle = false;
        stopHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHandler();

        if(canHandle)
        {
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            if(Input.GetKeyDown(KeyCode.Space) )
            {
                stopHolding = true;
                canHandle = false;
                rigidbody.useGravity = true;
                rigidbody.AddForce(cam.gameObject.transform.forward * afterHandleJumpForce, ForceMode.Impulse);
                
                Invoke(nameof(resetHoldingAbility), handleAfterJumpDelay);
            }
        }

        
        
    }

    void resetHoldingAbility()
    {
        stopHolding = false;
    }

    private void CheckHandler()
    {
        if(Physics.SphereCast(transform.position, sphereCastR, transform.forward, out hit, hitingLenght, Handler ) && !stopHolding)
        {
            canHandle = true;
        }
        else 
        {
            canHandle = false;
        }
    }

}
