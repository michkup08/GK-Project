using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenHints : MonoBehaviour
{
    private string[] messages;

    public double timeToDisplay = 3.0;
    private double timeDisplaying = 0.0;

    private bool messageShown = false;
    private bool isDisplaying = false;

    private HashSet<string> loadedMessages = new HashSet<string>();

    [SerializeField]
    private TMP_Text canvasText;

    void Update()
    {
        if (isDisplaying)
        {
            if (timeDisplaying >= timeToDisplay)
            {
                isDisplaying = false;
                timeDisplaying = 0.0;
                canvasText.gameObject.SetActive(false);
            }
            else
            {
                timeDisplaying += Time.deltaTime;
            }

            if (!messageShown)
            {
                messageShown = true;
                canvasText.text = "";
                foreach (string message in messages)
                {
                    canvasText.text += message + "\n";
                }
                canvasText.gameObject.SetActive(true);
            }
        }
    }

    public void LoadMessage(string[] messages, string name)
    {
        if (isDisplaying)
            return;

        if (loadedMessages.Contains(name))
            return;

        this.messages = messages;
        loadedMessages.Add(name);
        isDisplaying = true;
        messageShown = false;
    }
}
