using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelBarrel : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Barrel"))
        {
            gameObject.layer = 9;
        }
    }
}
