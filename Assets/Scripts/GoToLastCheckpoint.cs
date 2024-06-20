using UnityEngine;

public class GoToLastCheckpoint : MonoBehaviour
{
    public checkPointsMenager menager;
    public GameObject player;
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("reset");

        if (other.tag == "Player")
        {
            player.transform.position = menager.getPosition();
            Debug.Log("colission");
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("reset");

        if (collision.gameObject.tag == "Player")
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
