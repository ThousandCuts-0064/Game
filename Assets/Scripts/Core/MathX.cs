using System;
using System.Collections.Generic;
using System.Text;

public static class MathX
{
    public static bool IsValidFloat(int bits) =>
    ((bits ^ Value.FloatPosInfBits) & Value.FloatPosInfBits) != 0;

    public static bool IsValidDouble(long bits) =>
        ((bits ^ Value.DoublePosInfBits) & Value.DoublePosInfBits) != 0;

    public static int CountBits(int @int)
    {
        int bits = 0;
        while (@int > 0)
        {
            bits += @int & 1;
            @int >>= 1;
        }
        return bits;
    }

    public static ReadOnlySpan<int> SeparateBits(int @int, Span<int> span)
    {
        int i = 0;
        while (@int > 0)
        {
            span[i] = @int & 1;
            @int >>= 1;
            i++;
        }
        return span[..i];
    }

    public static ReadOnlySpan<int> SeparateCountBits(int @int, Span<int> span, out int bits)
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
        return span[..i];
    }
}
