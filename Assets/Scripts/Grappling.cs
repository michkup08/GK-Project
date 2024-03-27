using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    private Transform gunTip;
    private bool grappling = false;

    [Header("Grappling Settings")]
    [SerializeField]
    private float maxGrapplingDistance;

    [Header("ObjectsRef")]
    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private Transform playerObject;

    [Header("SpringJoint settings")]
    [SerializeField]
    private float spring = 10.0f;
    [SerializeField]
    private float damper = 7f;
    [SerializeField]
    private float massScale = 4.5f;

    private MultipleTags tags;
    private string checkTag = "grappable";

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gunTip = transform.Find("GunTip");
        lineRenderer.positionCount = 0;
    }

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

    private void LateUpdate()
    {
        if (grappling)
        {
            DrawLine();
        }
    }

    private void DrawLine()
    {
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

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

    private void StopGrappling()
    {
        grappling = false;
        lineRenderer.positionCount = 0;
        Destroy(playerObject.GetComponent<SpringJoint>());
    }

}
