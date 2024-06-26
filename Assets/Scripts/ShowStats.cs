using TMPro;
using UnityEngine;

/// <summary>
/// Displays level completion information when a player enters a trigger collider.
/// </summary>
public class ShowStats : MonoBehaviour
{
    /// <summary>
    /// The TextMeshPro Text component used to display level information.
    /// </summary>
    public TMP_Text levelInfo;

    /// <value>
    /// The level number this trigger represents.
    /// </value>
    public int level;

    /// <value>
    /// The maximum points achievable for this level.
    /// </value>
    public int maxPoints;

    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// Displays level completion status if the entering collider is tagged as "Player".
    /// </summary>
    /// <param name="other">The Collider that entered the trigger.</param>
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

    /// <summary>
    /// Called when another collider exits the trigger collider attached to this object.
    /// Hides the level information display if the exiting collider is tagged as "Player".
    /// </summary>
    /// <param name="other">The Collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            levelInfo.gameObject.SetActive(false);
        }
    }
}
