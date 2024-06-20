using UnityEngine;

/// <summary>
/// This class represents counting the number of collectibles the player has collected.
/// </summary>
public class Collectible : MonoBehaviour
{
    /// <value><c>collectedCounter</c> the number of collectibles the player has collected.</value>
    public int collectedCounter = 0;

    /// <value><c>levelStatistics</c> reference to LevelStatistics to set collectedCount to show.</value>
    LevelStatistics levelStatistics;

    /// <summary>
    /// This method sets the levelStatistics reference when the game starts.
    /// </summary>
    void Start()
    {
        levelStatistics = GetComponent<LevelStatistics>();
    }

    /// <summary>
    /// This method is called when the Collider other enters the trigger.
    /// If the other object is a collectible, it is destroyed, and, CollectedCounter is incremented,
    /// and the collectedCount in LevelStatistics is updated.
    /// </summary>
    /// <param name="other">Object that player collides with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            collectedCounter++;
            levelStatistics.collectedCount = collectedCounter;
        }
    }
}
