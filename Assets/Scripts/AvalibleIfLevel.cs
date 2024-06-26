using UnityEngine;

/// <summary>
/// This class controls the activation of a GameObject based on the player's current level.
/// </summary>
public class AvalibleIfLevel : MonoBehaviour
{
    /// <value> The level required to activate the GameObject. </value>
    public int levelToActive;

    /// <summary>
    /// Initializes the GameObject and sets its active state based on the player's current level.
    /// </summary>
    void Start()
    {
        if (SaveSystem.Load().currentLevel < levelToActive)
        {
            gameObject.SetActive(false);
        }
    }
}