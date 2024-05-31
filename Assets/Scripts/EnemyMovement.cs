using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    private bool playerInRange;
    private bool fighting;
    public Transform playerTransform;

    private Animator animator;

    // Start is called before the first frame update
    private void start(){
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float dist = Vector3.Distance(other.transform.position, transform.position);
           // animator.SetBool("isInRange", false);
            if(dist>2)
            {
                fighting = false;
                playerInRange = true;
                Debug.Log("Enemy set agro");
                agent.SetDestination(playerTransform.position + (transform.position.normalized*1.5f));
            }
            else 
            {
                fighting = true;
                
            }
        }
    }
}
