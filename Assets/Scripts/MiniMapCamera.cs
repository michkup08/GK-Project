using UnityEngine;

/// <summary>
/// The MiniMapCamera class handles the positioning and rotation of the minimap camera.
/// </summary>
public class MiniMapCamera : MonoBehaviour
{
    /// <value><c>player</c> is the Transform of the player object that the minimap camera follows.</value>
    [SerializeField]
    private Transform player;

    /// <summary>
    /// Updating of the position and rotation of the minimap camera to follow the player.
    /// </summary>
    void LateUpdate()
    {
        // Set the position of the minimap camera to be above the player
        transform.position = player.position + new Vector3(0, 10, 0);

        // Set the rotation of the minimap camera to match the player's y rotation
        transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
    }
}