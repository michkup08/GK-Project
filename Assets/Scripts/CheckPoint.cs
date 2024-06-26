using UnityEngine;

/// <summary>
/// This class handles checkpoint functionality, saving the player's position when they reach a checkpoint.
/// </summary>
public class CheckPoint : MonoBehaviour
{
    /// <value> Reference to the checkPointsManager that manages checkpoints. </value>
    public checkPointsMenager menager;

    /// <summary>
    /// Called when another collider enters the checkpoint trigger.
    /// </summary>
    /// <param name="other">The Collider that triggered the checkpoint.</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Check point saved");
        menager.setPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        Destroy(gameObject);
    }
}
