using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The NPCController class handles the interaction between the player and the NPCs in the game.
/// </summary>
public class NPCController : MonoBehaviour
{
    [SerializeField]
    /// <value><c>npcName</c> is the name of the NPC.</value>
    private string npcName;

    [SerializeField]
    /// <value><c>fileWithDialogue</c> is the file containing the dialogue for the NPC.</value>
    private TextAsset fileWithDialogue;

    [SerializeField]
    /// <value><c>lineDisplayTimeSec</c> is the time in seconds each line of dialogue is displayed.</value>
    private float lineDisplayTimeSec = 2.0f;

    [SerializeField]
    /// <value><c>canvasText</c> is the TMP_Text object to display the dialogue on the game's canvas.</value>
    private TMP_Text canvasText;

    /// <value><c>animator</c> is the Animator component of the NPC. It is used to control the NPC's animations.</value>
    public Animator animator;

    /// <value><c>currentLine</c> is the index of the current line of dialogue being displayed.</value>
    private int currentLine = 0;

    /// <value><c>dialogue</c> is the list of dialogue lines for the NPC.</value>
    private List<string> dialogue = new List<string>();

    /// <value><c>dialogueActive</c> is a flag indicating whether the dialogue is currently active.</value>
    public bool dialogueActive = false;

    /// <value><c>playerInRange</c> is a flag indicating whether the player is in range to interact with the NPC.</value>
    private bool playerInRange = false;

    /// <value><c>currentLineDisplayTime</c> is the current display time of the line of dialogue.</value>
    private float currentLineDisplayTime = 0.0f;

    public SoundEffectManager soundEffectManager;

    /// <summary>
    /// It initializes the NPC's Animator component and dialogue lines at the start of the game.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();

        string[] lines = fileWithDialogue.text.Split('\n');
        foreach (string line in lines)
        {
            dialogue.Add(line);
        }
    }

    /// <summary>
    /// It handles the player's interaction with the NPC and the display of the dialogue.
    /// </summary>
    void Update()
    {
        if (playerInRange && currentLine == 0 && Input.GetKeyDown(KeyCode.E))
        {
            dialogueActive = true;
            canvasText.text = $"{npcName}: {dialogue[currentLine]}";
            canvasText.gameObject.SetActive(true);
        }

        if (dialogueActive)
        {
            if (currentLine < dialogue.Count)
            {
                if (animator)
                {
                    animator.SetBool("isTalking", true);
                }
                if (currentLineDisplayTime >= lineDisplayTimeSec)
                {
                    currentLineDisplayTime = 0.0f;
                    canvasText.text = $"{npcName}: {dialogue[currentLine]}";
                    currentLine++;
                }
                else
                {
                    currentLineDisplayTime += Time.deltaTime;
                }
            }
            else if (currentLine == dialogue.Count && currentLineDisplayTime >= lineDisplayTimeSec)
            {
                if (animator)
                {
                    animator.SetBool("isTalking", false);
                }
                dialogueActive = false;
                currentLine = 0;
                canvasText.gameObject.SetActive(false);
            }
            else
            {
                currentLineDisplayTime += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// It checks if the player has entered the NPC's interaction range and, if so, displays the interaction prompt.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(soundEffectManager)
            {
                soundEffectManager.npcController = this;
            }
            
            playerInRange = true;
            canvasText.gameObject.SetActive(true);
            canvasText.text = "Press E to talk to " + npcName;
        }
    }

    /// <summary>
    /// It checks if the player has left the NPC's interaction range and, if so, hides the dialogue and interaction prompt.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = false;
            currentLine = 0;
            dialogueActive = false;
            canvasText.gameObject.SetActive(false);
            canvasText.text = "";
        }
    }
}