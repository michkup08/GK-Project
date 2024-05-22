using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ResetPlayerAtStart class is responsible for resetting the player's state at the start of the game.
/// It is needed for the player to be correctly moving when on moving platforms.
/// </summary>
public class ResetPlayerAtStart : MonoBehaviour
{
    /// <summary>
    /// Activates and deactivates the player object at the start of the game.
    /// </summary>
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        player.SetActive(false);
        player.SetActive(true);
    }
}