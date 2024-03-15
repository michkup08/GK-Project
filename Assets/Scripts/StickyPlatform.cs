using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject stickedObject;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == stickedObject)
        {
            stickedObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == stickedObject)
        {
            stickedObject.transform.SetParent(null);
        }
    }
}
