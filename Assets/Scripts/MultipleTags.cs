using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The MultipleTags class allows a GameObject to have multiple tags.
/// </summary>
public class MultipleTags : MonoBehaviour
{
    /// <value><c>tags</c> is a list of tags assigned to the GameObject.</value>
    [SerializeField]
    private List<string> tags;

    /// <summary>
    /// Checks if a specific tag is assigned to the GameObject.
    /// </summary>
    /// <param name="tag">The tag to check.</param>
    /// <returns>True if the tag is assigned to the GameObject, false otherwise.</returns>
    public bool HasTag(string tag) => tags.Contains(tag);

    /// <summary>
    /// Adds a tag to the GameObject.
    /// </summary>
    /// <param name="tag">The tag to add.</param>
    public void AddTag(string tag)
    {
        if (!tags.Contains(tag))
        {
            tags.Add(tag);
        }
    }

    /// <summary>
    /// Removes a tag from the GameObject.
    /// </summary>
    /// <param name="tag">The tag to remove.</param>
    public void RemoveTag(string tag)
    {
        if (tags.Contains(tag))
        {
            tags.Remove(tag);
        }
    }

    /// <summary>
    /// Returns the list of tags assigned to the GameObject.
    /// </summary>
    /// <returns>The list of tags.</returns>
    public List<string> GetTags() => tags;
}