using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera3P : MonoBehaviour
{
    [Header("ObjectsRef")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;

    [Header("Virables")]
    public float rotationSpeed;
    public Transform lookDir;

    [SerializeField]
    float horizontalI;
    [SerializeField]
    float verticalI;
    public bool aim;
    public Image aimImage;

    public GameObject defaultCinemachine, aimCinemachine;

    void Start()
    {
        aim = false;
        aimImage.enabled = aim;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            aim = !aim;
            defaultCinemachine.SetActive(!aim);
            aimCinemachine.SetActive(aim);
            aimImage.enabled = aim;
        }


        if (!aim)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            horizontalI = Input.GetAxis("Horizontal");
            verticalI = Input.GetAxis("Vertical");

            Vector3 inputDir = orientation.forward * verticalI + orientation.right * horizontalI;
            if (inputDir.magnitude != 0)
            {
                playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            Vector3 lookAtDir = (lookDir.position - new Vector3(transform.position.x, lookDir.position.y, transform.position.z)).normalized;
            orientation.forward = lookAtDir;
            playerObject.forward = lookAtDir;
        }
    }

}
