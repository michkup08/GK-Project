using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The LiftActivation class handles the activation of lifts in the game when the player enters a specific trigger area.
/// </summary>
public class LiftActivation : MonoBehaviour
{
    /// <summary>
    /// This method checks if the player has entered the trigger area and, if so, activates the lift.
    /// Depending on the name of the trigger area, the lift is moved up, down, or activated (moving between positions).
    /// </summary>
    /// <param name="other">The other Collider involved in this collision. (Player)</param>
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