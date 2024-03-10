using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3P : MonoBehaviour
{
    [Header("ObjectsRef")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    Rigidbody rigidbody;

    [Header("Virables")]
    public float rotationSpeed;

    [SerializeField]
    float horizontalI;
    [SerializeField]
    float verticalI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        horizontalI = Input.GetAxis("Horizontal");
        verticalI = Input.GetAxis("Vertical");

        Vector3 inputDir = orientation.forward * verticalI + orientation.right * horizontalI;
        if(inputDir.magnitude != 0)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
