using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public checkPointsMenager menager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Check point saved");
        menager.setPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        Destroy(gameObject);
    }
}
