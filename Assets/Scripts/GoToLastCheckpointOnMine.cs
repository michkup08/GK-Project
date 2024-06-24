using UnityEngine;
using System.Collections;

public class GoToLastCheckpointOnMine : MonoBehaviour
{
    public checkPointsMenager menager;
    public GameObject player;
    public PlayerMovement playerMovement;
    public Animator animator;
    public GameObject explosion;
    int isExplodingHash;

    void Start()
    {
        isExplodingHash = Animator.StringToHash("isExploding");
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("reset");

        if (other.tag == "Player")
        {
            StartCoroutine(RespawnPlayerWithDelay());
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("reset");

        if (collision.gameObject.tag == "Player")
        {
            //if (!animator)
            //{
            //    if (playerMovement != null)
            //    {
            //        playerMovement.enabled = false;
            //    }
            //    explosion.transform.position = player.transform.position;
            //    explosion.transform.rotation = player.transform.rotation;
            //    explosion.SetActive(true);
            //    animator.SetBool(isExplodingHash, true);
            //    StartCoroutine(RespawnPlayerWithDelay());
            //}
            //else
            //{
            //    if (playerMovement != null)
            //    {
            //        playerMovement.enabled = false;
            //    }
            //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //    int stateHash = stateInfo.shortNameHash;
            //    if (stateHash == Animator.StringToHash("Standing Melee Attack Horizontal"))
            //    {
            //        explosion.transform.position = player.transform.position;
            //        explosion.transform.rotation = player.transform.rotation;
            //        explosion.SetActive(true);
            //        animator.SetBool(isExplodingHash, true);
            //        StartCoroutine(RespawnPlayerWithDelay());
            //    }
            //}
            //if (playerMovement != null)
            //{
            //    playerMovement.dead = true;
            //}
            playerMovement.enabled = false; //kur w   nic nie dziala na zablokowanie tej rotacji
            player.transform.rotation = Quaternion.identity;
            explosion.transform.position = player.transform.position;
            explosion.transform.rotation = player.transform.rotation;
            explosion.SetActive(true);
            playerMovement.dead = true;
            animator.SetBool(isExplodingHash, true);
            StartCoroutine(RespawnPlayerWithDelay());
        }
    }

    private IEnumerator RespawnPlayerWithDelay()
    {
        yield return new WaitForSeconds(3f);

        playerMovement.velocity = 0f;
        player.transform.position = menager.getPosition();
        Debug.Log("colission");
        explosion.SetActive(false);
        animator.SetBool(isExplodingHash, false);
        
        playerMovement.dead = false;
        
        playerMovement.enabled = true;

    }

}
