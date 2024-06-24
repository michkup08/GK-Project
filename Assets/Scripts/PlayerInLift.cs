using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInLift : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Rigidbody player;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (player.velocity.x != 0 || player.velocity.z != 0)
            {
                playerAnimator.SetBool("inLift", false);
            }
            else
            {
                playerAnimator.SetBool("inLift", true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerAnimator.SetBool("inLift", false);
        }
    }
}
