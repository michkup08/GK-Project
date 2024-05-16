using System.Collections.Generic;
using TMPro;

using UnityEngine;

/// <summary>
/// The ScreenHints class handles the display of on-screen messages.
/// </summary>
public class ScreenHints : MonoBehaviour
{
    /// <value><c>messages</c> is an array of strings that form the display message.</value>
    private string[] messages;

    /// <value><c>timeToDisplay</c> is the time in seconds that the message is displayed.</value>
    public double timeToDisplay = 3.0;

    /// <value><c>timeDisplaying</c> is the time in seconds that the current message has been displayed.</value>
    private double timeDisplaying = 0.0;

    /// <value><c>messageShown</c> indicates whether the message has been shown.</value>
    private bool messageShown = false;

    /// <value><c>isDisplaying</c> indicates whether any message is currently being displayed.</value>
    private bool isDisplaying = false;

    /// <value><c>loadedMessages</c> is a set of the names of the messages that have been loaded.</value>
    private HashSet<string> loadedMessages = new HashSet<string>();

    /// <value><c>canvasText</c> is the TextMeshPro component where the messages are displayed.</value>
    [SerializeField]
    private TMP_Text canvasText;

    /// <summary>
    /// Handling the display and removal of messages.
    /// </summary>
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

    /// <summary>
    /// The LoadMessage method loads a message to be displayed.
    /// </summary>
    /// <param name="messages">The message to display.</param>
    /// <param name="name">The name of the message.</param>
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