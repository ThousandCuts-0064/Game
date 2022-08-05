using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Const
{
    public const string CLONE = "(Clone)";
    public const int BYTE_BITS = 8;
    public const int SHORT_BITS = BYTE_BITS * 2;
    public const int INT_BITS = BYTE_BITS * 2;
    public const int LONG_BITS = INT_BITS * 2;
    public const int FLOAT_FRACTION_BITS = 23;
    public const int DOUBLE_FRACTION_BITS = 52;
    public const int DIRECTIONS_ALL = (int)Direction.All;
    public const int DIRECTIONS_COUNT = DIRECTIONS_ALL + 1;
}
