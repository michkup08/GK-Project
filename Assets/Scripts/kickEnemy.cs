using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages kicking enemies when they collide with a trigger.
/// </summary>
public class kickEnemy : MonoBehaviour
{
    /// <value> Reference to the Animator of the player. </value>
    public Animator playerAnimator;
    /// <value> Flag indicating if the enemy has been kicked </value>
    private bool getKicked;

    // Start is called before the first frame update
    void Start()
    {
        getKicked = false;
    }

    /// <summary>
    /// Handles the behavior when another collider enters the trigger.
    /// </summary>
    /// <param name="other">The Collider that triggered the event.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            int stateHash = stateInfo.shortNameHash;
            Debug.Log("kick enemy");

            if (stateHash == Animator.StringToHash("Mma Kick"))
            {
                getKicked = true;

                EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.GetKicked(); // Call the GetKicked method of the EnemyMovement script
                }
            }
        }
    }
}
