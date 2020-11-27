using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePoint
{
    public string indexName;
    public Vector2 screenPosition;
    public SortedList distances;

    public ReferencePoint(string iname, Vector2 spos)
    {
        distances = new SortedList();
    }

    public void AddReferenceDistance(string otherName, float distance)
    {
        distances.Add(otherName, distance);
    }

}
