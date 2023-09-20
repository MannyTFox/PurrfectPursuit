using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreTags : MonoBehaviour
{
    [SerializeField] List<string> extraTags = new List<string>();

    public string GetTag(int index)
    {
        if (index < extraTags.Count)
        {
            return extraTags[index];
        }
        else
        {
            return null;
        }
    }

    public bool HasTag(string tagName)
    {
        for (int i = 0; i < extraTags.Count; i++)
        {
            if(extraTags[i] == tagName)
            {
                return true;
            }
        }

        return false;
    }

    public void ChangeTag(int tagIndex, string newTag)
    {
        extraTags[tagIndex] = newTag;
    }
}
