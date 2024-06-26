using UnityEngine;

/// <summary>
/// This class handles resetting the player to the last checkpoint upon collision or trigger with specific objects.
/// </summary>
public class GoToLastCheckpoint : MonoBehaviour
{
    /// <value> Reference to the checkpoint manager. </value>
    public checkPointsMenager menager;
    /// <value> Reference to the player game object. </value>
    public GameObject player;
    /// <value> Reference to the Animator component. </value>
    public Animator animator;

    /// <summary>
    /// Handles the behavior when another collider enters the trigger.
    /// </summary>
    /// <param name="other">The Collider that triggered the event.</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("reset");

        if (other.CompareTag("Player"))
        {
            player.transform.position = menager.getPosition();
            Debug.Log("colission");
        }
    }

    /// <summary>
    /// Handles the behavior when a collision occurs.
    /// </summary>
    /// <param name="collision">The Collision that triggered the event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("reset");

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!animator)
            {
                player.transform.position = menager.getPosition();
                Debug.Log("colission");
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                int stateHash = stateInfo.shortNameHash;
                if (stateHash == Animator.StringToHash("Standing Melee Attack Horizontal"))
                {
                    player.transform.position = menager.getPosition();
                    Debug.Log("colission");
                }
            }
        }
    }
}
