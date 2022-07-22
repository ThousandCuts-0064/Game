using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum Direction
{
    None =  0,
    Forth = 1,
    Back =  2 >> 0,
    Right = 2 >> 1,
    Left =  2 >> 2,
    Top =   2 >> 3,
    Down =  2 >> 4
}
