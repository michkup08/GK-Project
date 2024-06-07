using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public Animator animator;
    public checkPointsMenager checkpointsmenager;
    public PlayerMovement playerMovement;
    public NPCController npcController;
    int lastState;
    float oldVelocity;
    bool letTalk = false;
    public AudioSource source;

    public AudioClip walking;
    public AudioClip runing;
    public AudioClip jumping;
    public AudioClip hanging;
    public AudioClip death;
    public AudioClip talk;

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

        

        if (stateHash != lastState || ((oldVelocity> walkRunLimit && playerMovement.velocity< walkRunLimit) || (oldVelocity < walkRunLimit && playerMovement.velocity > walkRunLimit) && stateHash == Animator.StringToHash("Move Blend Tree")))
        {
            oldVelocity = playerMovement.velocity;
            lastState = stateHash;
            if(stateHash == Animator.StringToHash("Jumping Up"))
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
                if(oldVelocity < walkRunLimit)
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

        if (checkpointsmenager)
        {
            if (checkpointsmenager.trigger)
            {
                checkpointsmenager.trigger = false;
                source.clip = death;
                source.PlayOneShot(death);
            }
        }

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
    }
}
