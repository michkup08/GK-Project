using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStatistics : MonoBehaviour
{
    public int collectedCount = 0;
    [SerializeField]
    public int totalCollectibleCount = 0;

    [SerializeField]
    private TMP_Text canvasText;

    string[] messages =
    {
        "Level Statistics",
        ""
    };

    void Start()
    {
        totalCollectibleCount = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

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
