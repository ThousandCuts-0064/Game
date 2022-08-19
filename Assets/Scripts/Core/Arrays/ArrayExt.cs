using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ArrayExt
{
    public static bool TryGetClass<T>(this IReadOnlyList<T> list, int index, out T obj) where T : class
    {
        obj = default;
        if (index >= list.Count || index < 0)
            return false;

        obj = list[index];
        return obj != null;
    }

    public static bool TryGetClass<T>(this IReadOnlyList<T> list, int index, IEqualityComparer<T> comparer, out T obj) where T : class
    {
        obj = default;
        if (index >= list.Count || index < 0)
            return false;

        obj = list[index];
        return !comparer.Equals(obj, null);
    }

    public static bool TryGetStruct<T>(this IReadOnlyList<T> list, int index, out T obj) where T : struct
    {
        obj = default;
        if (index >= list.Count || index < 0)
            return false;

        obj = list[index];
        return true;
    }

    public static bool TryGetClass<T>(this T[,] arr, int index0, int index1, out T obj) where T : class =>
        TryGetClass(arr, arr.GetLength(0), arr.GetLength(1), index0, index1, out obj);

    public static bool TryGetClass<T>(this T[,] arr, int index0, int index1, IEqualityComparer<T> comparer, out T obj) where T : class =>
        TryGetClass(arr, arr.GetLength(0), arr.GetLength(1), index0, index1, comparer, out obj);
    public static bool TryGetStruct<T>(this T[,] arr, int index0, int index1, out T obj) where T : struct =>
        TryGetStruct(arr, arr.GetLength(0), arr.GetLength(1), index0, index1, out obj);

    public static bool TryGetClass<T>(this T[,] arr, int length0, int length1, int index0, int index1, out T obj) where T : class
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1
            || index0 < 0 || index1 < 0)
            return false;

        obj = arr[index0, index1];
        return obj != null;
    }

    public static bool TryGetClass<T>(this T[,] arr, int length0, int length1, int index0, int index1, IEqualityComparer<T> comparer, out T obj) where T : class
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1
            || index0 < 0 || index1 < 0)
            return false;

        obj = arr[index0, index1];
        return !comparer.Equals(obj, null);
    }

    public static bool TryGetStruct<T>(this T[,] arr, int length0, int length1, int index0, int index1, out T obj) where T : struct
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1
            || index0 < 0 || index1 < 0)
            return false;

        obj = arr[index0, index1];
        return true;
    }

    public static bool TryGetClass<T>(this T[,,] arr, int index0, int index1, int index2, out T obj) where T : class =>
        TryGetClass(arr, arr.GetLength(0), arr.GetLength(1), arr.GetLength(2), index0, index1, index2, out obj);

    public static bool TryGetClass<T>(this T[,,] arr, int length0, int length1, int length2, int index0, int index1, int index2, out T obj) where T : class
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1 || index2 >= length2
            || index0 < 0 || index1 < 0 || index2 < 0)
            return false;

        obj = arr[index0, index1, index2];
        return obj != null;
    }

    public static bool TryGetClass<T>(this T[,,] arr, int length0, int length1, int length2, int index0, int index1, int index2, 
        IEqualityComparer<T> comparer, out T obj) where T : class
    {
        obj = default;
        if (index0 >= length0 || index1 >= length1 || index2 >= length2
            || index0 < 0 || index1 < 0 || index2 < 0)
            return false;

        obj = arr[index0, index1, index2];
        return !comparer.Equals(obj, null);
    }
}
