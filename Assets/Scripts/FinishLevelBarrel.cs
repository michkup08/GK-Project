using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the finish level logic when a barrel enters the trigger.
/// </summary>
public class FinishLevelBarrel : MonoBehaviour
{
    /// <summary>
    /// Handles the behavior when another collider enters the trigger.
    /// </summary>
    /// <param name="collider">The Collider that triggered the event.</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Barrel"))
        {
            gameObject.layer = 9;
        }
    }
}
