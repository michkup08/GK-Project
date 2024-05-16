using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The Grappling class handles the grappling mechanics in the game.
/// </summary>
public class Grappling : MonoBehaviour
{
    /// <value><c>lineRenderer</c> the line renderer for the grappling line.</value>
    private LineRenderer lineRenderer;

    /// <value><c>grapplePoint</c> the point where the grappling hook is attached.</value>
    private Vector3 grapplePoint;

    /// <value><c>gunTip</c> the tip of the gun where the grappling hook is shot from.</value>
    private Transform gunTip;

    /// <value><c>grappling</c> whether the player is currently grappling.</value>
    private bool grappling = false;

    /// <value><c>maxGrapplingDistance</c> the maximum distance the grappling hook can reach.</value>
    [SerializeField]
    private float maxGrapplingDistance;

    /// <value><c>playerCamera</c> the player's camera.</value>
    [SerializeField]
    private Transform playerCamera;

    /// <value><c>playerObject</c> the player's object.</value>
    [SerializeField]
    private Transform playerObject;

    /// <value><c>spring</c> the spring value for the spring joint.</value>
    [SerializeField]
    private float spring = 10.0f;

    /// <value><c>damper</c> the damper value for the spring joint.</value>
    [SerializeField]
    private float damper = 7f;

    /// <value><c>massScale</c> the mass scale value for the spring joint.</value>
    [SerializeField]
    private float massScale = 4.5f;

    /// <value><c>tags</c> the reference to the MultipleTags object.</value>
    private MultipleTags tags;

    /// <value><c>checkTag</c> the tag to check for grappable objects.</value>
    private string checkTag = "grappable";

    /// <summary>
    /// Initializes the grappling mechanics. Sets the line renderer and gun tip.
    /// </summary>
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gunTip = transform.Find("GunTip");
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// Handles the player's input for starting and stopping the grappling.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrappling();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrappling();
        }
    }

    /// <summary>
    /// Draws the grappling line if the player is grappling.
    /// </summary>
    private void LateUpdate()
    {
        if (grappling)
        {
            DrawLine();
        }
    }

    /// <summary>
    /// Draws the grappling line from the gun tip to the grapple point.
    /// </summary>
    private void DrawLine()
    {
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    /// <summary>
    /// Starts the grappling if the player's aim hits a grappable object.
    /// </summary>
    private void StartGrappling()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitPoint, maxGrapplingDistance, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            hitPoint.transform.TryGetComponent<MultipleTags>(out var multipleTags);
            if (multipleTags != null)
                if (multipleTags.HasTag(checkTag))
                {
                    grapplePoint = hitPoint.point;
                    grappling = true;
                    lineRenderer.positionCount = 2;

                    SpringJoint springJoint = playerObject.AddComponent<SpringJoint>();
                    springJoint.autoConfigureConnectedAnchor = false;
                    springJoint.connectedAnchor = grapplePoint;
                    springJoint.maxDistance = Vector3.Distance(playerObject.position, grapplePoint) * 0.8f;
                    springJoint.minDistance = Vector3.Distance(playerObject.position, grapplePoint) * 0.1f;
                    springJoint.spring = spring;
                    springJoint.damper = damper;
                    springJoint.massScale = massScale;
                }
        }
    }

    /// <summary>
    /// Stops the grappling and removes the grappling line.
    /// </summary>
    private void StopGrappling()
    {
        grappling = false;
        lineRenderer.positionCount = 0;
        Destroy(playerObject.GetComponent<SpringJoint>());
    }
}
