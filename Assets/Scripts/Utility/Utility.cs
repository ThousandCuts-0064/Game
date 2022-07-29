using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utility
{
    public static T ReplaceClone<T>(this T obj, string str) where T : UnityEngine.Object
    {
        obj.name = obj.name.Replace(Const.CLONE, str);
        return obj;
    }

    public static int CountBits(this int @int)
    {
        int bits = 0;
        while (@int > 0)
        {
            bits += @int & 1;
            @int >>= 1;
        }
        return bits;
    }

    public static ReadOnlySpan<int> SeparateBits(this int @int, Span<int> span)
    {
        int i = 0;
        while (@int > 0)
        {
            span[i] = @int & 1;
            @int >>= 1;
            i++;
        }
        return span.Slice(0, i);
    }

    public static ReadOnlySpan<int> SeparateCountBits(this int @int, Span<int> span, out int bits)
    {
        int i = 0;
        bits = 0;
        while (@int > 0)
        {
            int bit = @int & 1;
            span[i] = bit;
            bits += bit;
            @int >>= 1;
            i++;
        }
        return span.Slice(0, i);
    }
}
