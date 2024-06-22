using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kickEnemy : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator enemyAnimator;
    public GameObject enemy;
 
    private bool getKicked;

    // Start is called before the first frame update
    void Start()
    {
        getKicked = false;
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    void OnTriggerEnter(Collider other)
    {       
        if (other.tag == "Enemy")
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
                    enemyMovement.GetKicked();
                }
            }
        }
    }

}
