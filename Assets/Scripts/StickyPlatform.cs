using UnityEngine;

/// <summary>
/// The StickyPlatform class handles the interaction between the player and sticky platforms in the game.
/// When the player is on a sticky platform, he become a child of the platform, moving with it.
/// When the player stops colliding with the platform, he are no longer a child of the platform.
/// </summary>
public class StickyPlatform : MonoBehaviour
{
    /// <value><c>objectToStick</c> is the GameObject that will stick to the platform.</value>
    private GameObject objectToStick;

    /// <summary>
    /// It initializes the objectToStick with the player's GameObject.
    /// </summary>
    private void Start()
    {
        objectToStick = GameObject.Find("Player");
    }

    /// <summary>
    /// Checks if the objectToStick is colliding with the platform.
    /// If the colliding object is the objectToStick, it becomes a child of the platform.
    /// </summary>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == objectToStick)
        {
            objectToStick.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// Checks if the objectToStick stopped colliding with the platform.
    /// If the object that stopped colliding is the objectToStick, it is no longer a child of the platform.
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == objectToStick)
        {
            objectToStick.transform.SetParent(null);
        }
    }
}