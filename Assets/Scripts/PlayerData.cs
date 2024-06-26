[System.Serializable]
/// <summary>
/// Serializable class representing player data for saving and loading.
/// </summary>
public class PlayerData
{
    /// <value> Current level of the player. </value>
    public int currentLevel;

    /// <value> Array representing points or scores accumulated by the player. </value>
    public int[] points;

    /// <summary>
    /// Constructor for initializing PlayerData with specific parameters.
    /// </summary>
    /// <param name="level">The current level of the player.</param>
    /// <param name="points">An array of points or scores accumulated by the player.</param>
    public PlayerData(int level, int[] points)
    {
        currentLevel = level;
        this.points = points;
    }
}
