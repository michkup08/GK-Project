using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Lift class handles the movement of the lift in the game.
/// </summary>
public class Lift : MonoBehaviour
{
    /// <summary>
    /// The speed at which the lift moves.
    /// </summary>
    [SerializeField]
    private float speed = 0.008f;

    /// <summary>
    /// The upper position of the lift platform.
    /// </summary>
    private GameObject platformUpPost;

    /// <summary>
    /// The lower position of the lift platform.
    /// </summary>
    private GameObject platformDownPost;

    /// <summary>
    /// The Renderer component of the first chain of the lift.
    /// </summary>
    private Renderer chain1Renderer;

    /// <summary>
    /// The Renderer component of the second chain of the lift.
    /// </summary>
    private Renderer chain2Renderer;

    /// <summary>
    /// The Rigidbody component of the lift platform.
    /// </summary>
    private Rigidbody platform;

    /// <summary>
    /// A flag indicating whether the lift platform is up.
    /// </summary>
    private bool isPlatformUp = true;

    /// <summary>
    /// A flag indicating whether the lift platform is moving.
    /// </summary>
    private bool isPlatformMoving = false;

    /// <summary>
    /// Initialization of the lift platform, chains, and positions at the start of the game.
    /// </summary>
    void Start()
    {
        platformUpPost = transform.Find("LiftPlatformUpPos").gameObject;
        platformDownPost = transform.Find("LiftPlatformDownPos").gameObject;

        platform = transform.Find("LiftPlatform").GetComponent<Rigidbody>();

        chain1Renderer = transform.Find("LiftPillar1/LiftChainPillar1").GetComponent<Renderer>();
        chain2Renderer = transform.Find("LiftPillar2/LiftChainPillar2").GetComponent<Renderer>();
    }

    /// <summary>
    /// Handles the movement of the lift platform and chains.
    /// </summary>
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

    /// <summary>
    /// Updates the texture offset of the lift chains.
    /// </summary>
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

    /// <summary>
    /// Activates the lift, causing the platform to move up or down depending on its current position.
    /// </summary>
    public void ActivateLift()
    {
        if (!isPlatformMoving)
        {
            isPlatformMoving = true;
        }
    }

    /// <summary>
    /// Moves the lift platform up.
    /// </summary>
    public void MoveLiftUp()
    {
        if (!isPlatformMoving && !isPlatformUp)
        {
            isPlatformMoving = true;
        }
    }

    /// <summary>
    /// Moves the lift platform down.
    /// </summary>
    public void MoveLiftDown()
    {
        if (!isPlatformMoving && isPlatformUp)
        {
            isPlatformMoving = true;
        }
    }
}