using UnityEngine;
using System.Collections;

/// <summary>
/// This class manages respawning the player and a barrel at the last checkpoint upon collision with specific objects.
/// </summary>
public class GoToLastCheckpointOnMine : MonoBehaviour
{
    /// <value> Reference to the checkpoint manager. </value>
    public checkPointsMenager menager;
    /// <value> Reference to the player game object. </value>
    public GameObject player;
    /// <value> Reference to the barrel game object. </value>
    public GameObject barrel;
    /// <value> Reference to the PlayerMovement script attached to the player. </value>
    public PlayerMovement playerMovement;
    /// <value> Reference to the Animator component for handling animations. </value>
    public Animator animator;
    /// <value> Reference to the explosion game object for visual effects. </value>
    public GameObject explosion;
    /// <value> Indicates if the explosion animation is currently active. </value>
    public bool exploding = false;
    /// <value> Hash for the "isExploding" parameter in the Animator component. </value>
    private int isExplodingHash;

    /// <summary>
    /// Initializes the hash for the "isExploding" Animator parameter.
    /// </summary>
    void Start()
    {
        isExplodingHash = Animator.StringToHash("isExploding");
    }

    /// <summary>
    /// Handles the behavior when another collider enters the trigger.
    /// </summary>
    /// <param name="other">The Collider that triggered the event.</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("reset");

        if (other.CompareTag("Player"))
        {
            StartCoroutine(RespawnPlayerWithDelay());
        }
        if (other.CompareTag("Barrel"))
        {
            StartCoroutine(RespawnBarrelWithDelay());
        }
    }

    /// <summary>
    /// Handles the behavior when a collision occurs.
    /// </summary>
    /// <param name="collision">The Collision that triggered the event.</param>
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("reset");

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RespawnPlayerWithDelay());
        }
        if (collision.gameObject.CompareTag("Barrel"))
        {
            StartCoroutine(RespawnBarrelWithDelay());
        }
    }

    /// <summary>
    /// Coroutine to respawn the player with a delay after an explosion.
    /// </summary>
    private IEnumerator RespawnPlayerWithDelay()
    {
        exploding = true;
        playerMovement.enabled = false;
        player.transform.rotation = Quaternion.identity;
        explosion.transform.position = player.transform.position;
        explosion.transform.rotation = player.transform.rotation;
        explosion.SetActive(true);
        playerMovement.dead = true;
        animator.SetBool(isExplodingHash, true);
        yield return new WaitForSeconds(3f);

        playerMovement.velocity = 0f;
        player.transform.position = menager.getPosition();
        Debug.Log("colission");
        explosion.SetActive(false);
        animator.SetBool(isExplodingHash, false);
        playerMovement.dead = false;
        playerMovement.enabled = true;
    }

    /// <summary>
    /// Coroutine to respawn the barrel with a delay after an explosion.
    /// </summary>
    private IEnumerator RespawnBarrelWithDelay()
    {
        exploding = true;
        explosion.transform.position = barrel.transform.position;
        explosion.transform.rotation = barrel.transform.rotation;
        explosion.SetActive(true);
        barrel.SetActive(false);
        yield return new WaitForSeconds(3f);
        explosion.SetActive(false);
        barrel.SetActive(true);
        barrel.gameObject.transform.position = new Vector3(56.5f, 6, 122);
        barrel.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }
}
