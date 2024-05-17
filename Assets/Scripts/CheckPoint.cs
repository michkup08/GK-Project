using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public checkPointsMenager menager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Check point saved");
        menager.setTransform(transform);
        Destroy(gameObject);
    }
}
