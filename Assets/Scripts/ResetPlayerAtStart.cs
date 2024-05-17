using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerAtStart : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        player.SetActive(false);
        player.SetActive(true);        
    }
}
