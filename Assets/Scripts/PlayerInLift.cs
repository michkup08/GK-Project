using UnityEngine;

/// <summary>
/// Manages the player's animation state when inside a lift, based on his movement.
/// </summary>
public class PlayerInLift : MonoBehaviour
{
    /// <value><c>playerAnimator</c> is used to control the player's animations.</value>
    [SerializeField]
    private Animator playerAnimator;

    /// <value><c>player</c> refers to the player's Rigidbody component, used to check his movement.</value>
    [SerializeField]
    private Rigidbody player;

    /// <summary>
    /// Called when the player continuously collides with the lift.
    /// It checks the player's movement and updates the animation state accordingly.
    /// </summary>
    /// <param name="collision">The Collision data associated with this collision.</param>
    private void OnCollisionStay(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // If the player is moving, set 'inLift' to false, else to true
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

    /// <summary>
    /// Called when the player exits the collision with the lift.
    /// It resets the player's animation state to indicate they are not in the lift.
    /// </summary>
    /// <param name="collision">The Collision data associated with this collision.</param>
    private void OnCollisionExit(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Reset the 'inLift' animation state to false
            playerAnimator.SetBool("inLift", false);
        }
    }
}