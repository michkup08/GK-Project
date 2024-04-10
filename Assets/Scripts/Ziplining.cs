using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ziplining : MonoBehaviour
{
    [SerializeField]
    private float doubleSpaceKeyPressTimeMax = 0.5f;
    private bool spaceKeyPressed = false;

    [Header("Speed")]
    [SerializeField]
    private float speed = 3.0f;

    private Rigidbody playerRigidbody;
    private float doubleSpaceKeyPressTime = 0.0f;
    private bool isZiplining = false;

    Vector3 minPoint;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("zippableLine"))
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.GetContact(0);
                isZiplining = (Vector3.Dot(contact.normal, Vector3.up) > 0.7);
                playerRigidbody.useGravity = false;
                playerRigidbody.drag = 0;
                isZiplining = true;
                minPoint = collision.gameObject.GetComponent<MeshRenderer>().bounds.min;
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

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (spaceKeyPressed)
            {
                if (doubleSpaceKeyPressTime <= doubleSpaceKeyPressTimeMax)
                {
                    playerRigidbody.useGravity = true;
                    isZiplining = false;
                }
            }
            spaceKeyPressed = true;
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
    }
}
