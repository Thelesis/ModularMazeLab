using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenstions
{
    
    private static System.Random rng = new System.Random();

    /// <summary>
    /// Shuffle content of list
    /// Solution based on: https://stackoverflow.com/questions/273313/randomize-a-listt
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
