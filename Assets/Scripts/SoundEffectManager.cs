using UnityEngine;

/// <summary>
/// Manages sound effects based on various game events and states.
/// </summary>
public class SoundEffectManager : MonoBehaviour
{
    /// <value>
    /// The Animator component controlling character animations.
    /// </value>
    public Animator animator;

    /// <value>
    /// Reference to the checkpoint manager script.
    /// </value>
    public checkPointsMenager checkpointsmenager;

    /// <value>
    /// Reference to the player movement script.
    /// </value>
    public PlayerMovement playerMovement;

    /// <value>
    /// Reference to the NPC controller script.
    /// </value>
    public NPCController npcController;

    /// <value>
    /// Reference to the script handling going to the last checkpoint on mine.
    /// </value>
    public GoToLastCheckpointOnMine goToLastCheckpointOnMine;

    /// <value>
    /// Stores the last animator state hash.
    /// </value>
    int lastState;

    /// <value>
    /// Stores the previous velocity of the player.
    /// </value>
    float oldVelocity;

    /// <value>
    /// Flag indicating if the NPC is currently talking.
    /// </value>
    bool letTalk = false;

    /// <value>
    /// The AudioSource component used to play sound effects.
    /// </value>
    public AudioSource source;

    /// <value>
    /// Sound effect for walking.
    /// </value>
    public AudioClip walking;

    /// <value>
    /// Sound effect for running.
    /// </value>
    public AudioClip runing;

    /// <value>
    /// Sound effect for jumping.
    /// </value>
    public AudioClip jumping;

    /// <value>
    /// Sound effect for hanging.
    /// </value>
    public AudioClip hanging;

    /// <value>
    /// Sound effect for death.
    /// </value>
    public AudioClip death;

    /// <value>
    /// Sound effect for NPC dialogue.
    /// </value>
    public AudioClip talk;

    /// <value>
    /// Sound effect for explosion.
    /// </value>
    public AudioClip explosion;

    /// <value>
    /// The velocity threshold distinguishing between walking and running.
    /// </value>
    public float walkRunLimit = 10;

    // Start is called before the first frame update
    void Start()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        lastState = stateInfo.shortNameHash;
        oldVelocity = playerMovement.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        int stateHash = stateInfo.shortNameHash;

        // Check if the animator state or player velocity has changed
        if (stateHash != lastState || (oldVelocity > walkRunLimit && playerMovement.velocity < walkRunLimit) || (oldVelocity < walkRunLimit && playerMovement.velocity > walkRunLimit && stateHash == Animator.StringToHash("Move Blend Tree")))
        {
            oldVelocity = playerMovement.velocity;
            lastState = stateHash;

            // Handle specific sound effects based on animator states
            if (stateHash == Animator.StringToHash("Jumping Up"))
            {
                source.clip = jumping;
                source.PlayOneShot(jumping);
            }

            if (stateHash == Animator.StringToHash("Hanging Idle 1"))
            {
                source.clip = hanging;
                source.PlayOneShot(hanging);
            }

            if (stateHash == Animator.StringToHash("Move Blend Tree"))
            {
                if (oldVelocity < walkRunLimit)
                {
                    source.clip = walking;
                    source.Play();
                }
                else
                {
                    source.clip = runing;
                    source.Play();
                }
            }

            if (stateHash == Animator.StringToHash("Idle"))
            {
                source.clip = null;
            }
        }

        // Checkpoint manager handling
        if (checkpointsmenager)
        {
            if (checkpointsmenager.trigger)
            {
                checkpointsmenager.trigger = false;
                source.clip = death;
                source.PlayOneShot(death);
            }
        }

        // NPC dialogue handling
        if (npcController)
        {
            if (npcController.dialogueActive)
            {
                source.clip = talk;
                if (!letTalk)
                {
                    letTalk = true;
                    source.Play();
                }
            }
            else
            {
                letTalk = false;
            }
        }

        // Handling explosion sound effect
        if (goToLastCheckpointOnMine)
        {
            if (goToLastCheckpointOnMine.exploding)
            {
                goToLastCheckpointOnMine.exploding = false;
                source.PlayOneShot(explosion);
            }
        }
    }
}
