using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    private bool playerInRange;
    private bool fighting;
    public Transform playerTransform;
    private bool stand;

    private bool isKicked;

    public Animator animator;


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

                    agent.SetDestination(playerTransform.position);// + (transform.position.normalized*0.8f));
                }

            }
            else
            {
                agent.SetDestination(transform.position);
                fighting = true;

            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    // Method to be called when the enemy gets kicked
    public void GetKicked()
    {
        Debug.Log("Enemy got kicked");
        animator.SetBool("isKicked", true);
        isKicked = true;
        agent.SetDestination(transform.position); // Stop moving
    }

}
