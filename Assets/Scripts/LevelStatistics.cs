using TMPro;
using UnityEngine;

/// <summary>
/// The LevelStatistics class handles the tracking and display of collectible items in a level.
/// </summary>
public class LevelStatistics : MonoBehaviour
{
    /// <value><c>collectedCount</c> is the count of collected items.</value>
    public int collectedCount = 0;

    /// <value><c>totalCollectibleCount</c> is the total count of collectible items in the level.</value>
    [SerializeField]
    public int totalCollectibleCount = 0;

    /// <value><c>canvasText</c> is the TextMeshPro component where the statistics are displayed.</value>
    [SerializeField]
    private TMP_Text canvasText;

    /// <value><c>messages</c> is an array of strings that form the display message.</value>
    private string[] messages =
    {
        "Level Statistics",
        ""
    };

    /// <summary>
    /// Initialization of the totalCollectibleCount.
    /// </summary>
    void Start()
    {
        totalCollectibleCount = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    /// <summary>
    /// The LateUpdate method is called once per frame, after all Update methods have been called. It updates the display message.
    /// </summary>
    void LateUpdate()
    {
        messages[1] = "Collected items: " + collectedCount + " / " + totalCollectibleCount;
        canvasText.text = "";
        foreach (string message in messages)
        {
            canvasText.text += message + "\n";
        }
    }
}