using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLastCheckpoint : MonoBehaviour
{
    public checkPointsMenager menager;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("reset");

        if(other.tag=="Player")
        {
            player.transform.position = menager.getPosition();
        }
        
        
    }
}
