using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Value
{
    public static int FloatPosInfBits { get; } = BitConverter.SingleToInt32Bits(float.PositiveInfinity);
    public static long DoublePosInfBits { get; } = BitConverter.DoubleToInt64Bits(double.PositiveInfinity);
}
