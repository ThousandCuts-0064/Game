using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static T ReplaceClone<T>(this T obj, string str) where T : UnityEngine.Object
    {
        obj.name = obj.name.Replace(Const.CLONE, str);
        return obj;
    }

    public static T SetLocalPosition<T>(this T obj, Vector3 pos) where T : Component
    {
        obj.transform.localPosition = pos;
        return obj;
    }

    public static ReadOnlySpan<char> SeparateWords(this string str, Span<char> span)
    {
        int spaces = 0;
        int i = 0;
        while (!char.IsUpper(str[i])) i++;
        for (; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
            {
                span[i + spaces] = ' ';
                spaces++;
            }
            span[i + spaces] = str[i];
        }
        return span[..(str.Length + spaces)];
    }

}
