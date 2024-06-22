using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kickEnemy : MonoBehaviour
{
    public Animator animator;  
    public GameObject enemy;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }

    void OnTriggerEnter(Collider other)
    {       
        if (other.tag == "Enemy")
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            int stateHash = stateInfo.shortNameHash;
            Debug.Log("kick enemy");
   
            if (stateHash == Animator.StringToHash("Mma Kick"))
            {
                Destroy(other.gameObject);
            }
        }
    }




}
