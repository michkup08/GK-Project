using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Camera3P : MonoBehaviour
{
    [Header("ObjectsRef")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Camera cam;

    [Header("Variables")]
    public float rotationSpeed;
    public Transform lookDir;

    [SerializeField]
    float horizontalI;
    [SerializeField]
    float verticalI;
    public bool aim;
    public Image aimImage;

    public bool disableRotation;

    public CinemachineFreeLook defaultCinemachine, aimCinemachine;

    void Start()
    {
        aim = false;
        aimImage.enabled = aim;
        defaultCinemachine.Priority = 1;
        aimCinemachine.Priority = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            aim = !aim;


            //defaultCinemachine.SetActive(!aim);
            //aimCinemachine.SetActive(aim);
            if (aim)
            {
                defaultCinemachine.Priority = 0;
                aimCinemachine.Priority = 1;
            }
            else
            {
                defaultCinemachine.Priority = 1;
                aimCinemachine.Priority = 0;
            }
            aimImage.enabled = aim;
        }


        if (!aim)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            horizontalI = Input.GetAxis("Horizontal");
            verticalI = Input.GetAxis("Vertical");

            Vector3 inputDir = (orientation.forward * verticalI) + (orientation.right * horizontalI);
            if (inputDir.magnitude != 0 && !disableRotation)
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
