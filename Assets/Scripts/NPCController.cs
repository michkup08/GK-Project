using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private string npcName;
    [SerializeField]
    private TextAsset fileWithDialogue;
    [SerializeField]
    private float lineDisplayTimeSec = 2.0f;
    [SerializeField]
    private TMP_Text canvasText;

    private int currentLine = 0;
    private List<string> dialogue = new List<string>();
    private bool dialogueActive = false;
    private bool playerInRange = false;

    private float currentLineDisplayTime = 0.0f;

    void Start()
    {
        string[] lines = fileWithDialogue.text.Split('\n');
        foreach (string line in lines)
        {
            dialogue.Add(line);
        }
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = true;
            canvasText.gameObject.SetActive(true);
            canvasText.text = "Press E to talk to " + npcName;
        }
    }

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
