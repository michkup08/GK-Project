using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTags : MonoBehaviour
{
    [SerializeField]
    private List<string> tags;

    public bool HasTag(string tag) => tags.Contains(tag);

    public void AddTag(string tag)
    {
        if (!tags.Contains(tag))
        {
            tags.Add(tag);
        }
    }

    public void RemoveTag(string tag)
    {
        if (tags.Contains(tag))
        {
            tags.Remove(tag);
        }
    }

    public List<string> GetTags() => tags;
}
