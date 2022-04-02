using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetRandomInList<T>(this List<T> list) where T : class
    {
        if (list == null || list.Count == 0)
            return null;

        return list[Random.Range(0, list.Count)];
    }
}
