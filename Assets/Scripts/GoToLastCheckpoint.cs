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
        
            
        player.transform.position = menager.getTransform().position;
        //player.transform.rotation = menager.getTransform().rotation;
        
    }
}
