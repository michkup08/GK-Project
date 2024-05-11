using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private GameObject objectToStick;

    private void Start()
    {
        objectToStick = GameObject.Find("Player");
    }


    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == objectToStick)
        {
            objectToStick.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == objectToStick)
        {
            objectToStick.transform.SetParent(null);
        }
    }
}
