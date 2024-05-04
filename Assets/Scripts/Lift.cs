using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.008f;

    private GameObject platformUpPost;
    private GameObject platformDownPost;

    private Renderer chain1Renderer;
    private Renderer chain2Renderer;

    private Rigidbody platform;

    private bool isPlatformUp = true;
    private bool isPlatformMoving = false;

    void Start()
    {
        platformUpPost = transform.Find("LiftPlatformUpPos").gameObject;
        platformDownPost = transform.Find("LiftPlatformDownPos").gameObject;

        platform = transform.Find("LiftPlatform").GetComponent<Rigidbody>();

        chain1Renderer = transform.Find("LiftPillar1/LiftChainPillar1").GetComponent<Renderer>();
        chain2Renderer = transform.Find("LiftPillar2/LiftChainPillar2").GetComponent<Renderer>();
    }

    void Update()
    {
        if (isPlatformMoving)
        {
            if (isPlatformUp)
            {
                if (Vector3.Distance(platform.transform.position, platformDownPost.transform.position) < 0.1f)
                {
                    isPlatformUp = false;
                    isPlatformMoving = false;
                }
                else
                {
                    platform.MovePosition(Vector3.MoveTowards(platform.transform.position, platformDownPost.transform.position, speed));
                    UpdateTextureOffset();
                }
            }
            else
            {
                if (Vector3.Distance(platform.transform.position, platformUpPost.transform.position) < 0.1f)
                {
                    isPlatformUp = true;
                    isPlatformMoving = false;
                }
                else
                {
                    platform.MovePosition(Vector3.MoveTowards(platform.transform.position, platformUpPost.transform.position, speed));
                    UpdateTextureOffset();
                }
            }
        }
    }

    private void UpdateTextureOffset()
    {
        float oldYOffset = chain1Renderer.material.mainTextureOffset.y;
        if (oldYOffset >= 1000)
        {
            oldYOffset = 0;
        }
        Vector2 newOffset = new Vector2(0, oldYOffset + 0.5f);

        chain1Renderer.material.mainTextureOffset = newOffset;
        chain2Renderer.material.mainTextureOffset = newOffset;
    }

    public void ActivateLift()
    {
        if (!isPlatformMoving)
        {
            isPlatformMoving = true;
        }
    }

    public void MoveLiftUp()
    {
        if (!isPlatformMoving && !isPlatformUp)
        {
            isPlatformMoving = true;
        }
    }

    public void MoveLiftDown()
    {
        if (!isPlatformMoving && isPlatformUp)
        {
            isPlatformMoving = true;
        }
    }
}
