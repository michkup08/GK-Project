using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftActivation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Lift lift;
            if (transform.name == "LiftPlatformActivationPos")
            {
                lift = transform.parent.parent.GetComponent<Lift>();
            }
            else
            {
                lift = transform.parent.GetComponent<Lift>();
            }


            if (lift != null)
            {
                if (transform.name == "LiftUpActivationPos")
                {
                    lift.MoveLiftUp();
                }
                else if (transform.name == "LiftDownActivationPos")
                {
                    lift.MoveLiftDown();
                }
                else
                {
                    lift.ActivateLift();
                }
            }
        }
    }
}
