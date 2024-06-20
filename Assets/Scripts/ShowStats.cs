using TMPro;
using UnityEngine;

public class ShowStats : MonoBehaviour
{
    public TMP_Text levelInfo;
    public int level, maxPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            levelInfo.gameObject.SetActive(true);
            int currentLevel = SaveSystem.Load().currentLevel;
            if (currentLevel > level)
            {
                int points = SaveSystem.Load().points[level - 1];
                levelInfo.text = "Level " + level + " completed with " + points + "/" + maxPoints;
            }
            else
            {
                levelInfo.text = "Level " + level + " incomplete";
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            levelInfo.gameObject.SetActive(false);
        }
    }
}
