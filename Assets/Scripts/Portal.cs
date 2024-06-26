using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents a portal that loads a new scene when triggered by a collider.
/// </summary>
public class Portal : MonoBehaviour
{
    /// <value>The name of the scene to load when triggered.</value>
    public string sceneName;

    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// Loads the scene specified by <see cref="sceneName"/>.
    /// </summary>
    /// <param name="other">The Collider other that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneName);
    }
}