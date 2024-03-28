using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float movingSpeed;

    private GameObject platformParent;
    private Transform pointA;
    private Transform pointB;

    bool isMovingToA = true;

    void Start()
    {
        platformParent = transform.parent.gameObject;
        pointA = platformParent.GetComponentInChildren<Transform>().Find("CheckPointA");
        pointB = platformParent.GetComponentInChildren<Transform>().Find("CheckPointB");
    }

    void Update()
    {
        if (isMovingToA)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, movingSpeed * Time.deltaTime);
            if (transform.position == pointA.position)
            {
                isMovingToA = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, movingSpeed * Time.deltaTime);
            if (transform.position == pointB.position)
            {
                isMovingToA = true;
            }
        }
    }
}
