using UnityEngine;

/// <summary>
/// This class manages checkpoints and handles the player's position after death.
/// </summary>
public class checkPointsMenager : MonoBehaviour
{
    /// <value> Position to respawn the player after death. </value>
    Vector3 AfterDeadPosition;
    /// <value> Reference to the player's Transform. </value>
    public Transform Player;
    /// <value> Indicates if a checkpoint has been triggered. </value>
    public bool trigger = false;

    /// <summary>
    /// Initializes the AfterDeadPosition to the player's initial position.
    /// </summary>
    void Start()
    {
        AfterDeadPosition = Player.position;
    }

    /// <summary>
    /// Gets the current respawn position.
    /// </summary>
    /// <returns>The position to respawn the player after death.</returns>
    public Vector3 getPosition()
    {
        trigger = true;
        return AfterDeadPosition;
    }

    /// <summary>
    /// Sets a new respawn position.
    /// </summary>
    /// <param name="position">The new position to respawn the player.</param>
    public void setPosition(Vector3 position)
    {
        AfterDeadPosition = position;
    }

    /// <summary>
    /// Updates the manager each frame.
    /// </summary>
    private void Update()
    {

    }
}
