using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class handles enemy movement and behavior, including interaction with the player and responding to being kicked.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    /// <value> Reference to the NavMeshAgent component for navigation. </value>
    public NavMeshAgent agent;
    /// <value> Indicates if the player is in range of the enemy. </value>
    private bool playerInRange;
    /// <value> Indicates if the enemy is fighting the player. </value>
    private bool fighting;
    /// <value> Reference to the player's transform. </value>
    public Transform playerTransform;
    /// <value> Indicates if the enemy is standing. </value>
    private bool stand;

    /// <value> Indicates if the enemy has been kicked. </value>
    private bool isKicked;

    /// <value> Reference to the Animator component for handling animations. </value>
    public Animator animator;

    /// <summary>
    /// Updates the enemy's behavior and animations each frame.
    /// </summary>
    private void Update()
    {
        if (isKicked)
        {
            agent.SetDestination(transform.position);
            return;
        }

        if (playerInRange)
        {
            animator.SetBool("isInRange", true);
        }
        else
        {
            animator.SetBool("isInRange", false);
        }

        if (fighting)
        {
            animator.SetBool("fight", true);
        }
        else
        {
            animator.SetBool("fight", false);
        }
    }

    /// <summary>
    /// Handles the behavior when the player stays within the enemy's trigger range.
    /// </summary>
    /// <param name="other">The Collider that triggered the event.</param>
    private void OnTriggerStay(Collider other)
    {
        if (isKicked)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float dist = Vector3.Distance(other.transform.position, transform.position);

            if (dist > 1)
            {
                fighting = false;
                playerInRange = true;
                Debug.Log("Enemy set agro");
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                int stateHash = stateInfo.shortNameHash;
                if (stateHash == Animator.StringToHash("Standing Melee Attack Horizontal"))
                {
                    agent.SetDestination(transform.position);
                }
                else
                {
                    agent.SetDestination(playerTransform.position);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                fighting = true;
            }
        }
    }

    /// <summary>
    /// Handles the behavior when the player exits the enemy's trigger range.
    /// </summary>
    /// <param name="other">The Collider that triggered the event.</param>
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    /// <summary>
    /// Method to be called when the enemy gets kicked, stopping its movement.
    /// </summary>
    public void GetKicked()
    {
        Debug.Log("Enemy got kicked");
        animator.SetBool("isKicked", true);
        isKicked = true;
        agent.SetDestination(transform.position); // Stop moving
    }
}
