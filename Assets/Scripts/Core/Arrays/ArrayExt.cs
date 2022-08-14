using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ArrayExt
{
    public static bool TryGetValue<T>(this IReadOnlyList<T> list, int index, out T obj) where T : class
    {
        obj = default;
        if (index >= list.Count || index < 0)
            return false;

        obj = list[index];
        return obj != null;
    }

    public static bool TryGetValue<T>(this T[,] arr, int index0, int index1, out T obj) where T : class =>
        TryGetValue(arr, arr.GetLength(0), arr.GetLength(1), index0, index1, out obj);

    public static bool TryGetValue<T>(this T[,] arr, int length0, int length1, int index0, int index1, out T obj) where T : class
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1
            || index0 < 0 || index1 < 0)
            return false;

        obj = arr[index0, index1];
        return obj != null;
    }

    public static bool TryGetValue<T>(this T[,,] arr, int index0, int index1, int index2, out T obj) where T : class =>
        TryGetValue(arr, arr.GetLength(0), arr.GetLength(1), arr.GetLength(2), index0, index1, index2, out obj);

    public static bool TryGetValue<T>(this T[,,] arr, int length0, int length1, int length2, int index0, int index1, int index2, out T obj) where T : class
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1 || index2 >= length2
            || index0 < 0 || index1 < 0 || index2 < 0)
            return false;

        obj = arr[index0, index1, index2];
        return obj != null;
    }
}
