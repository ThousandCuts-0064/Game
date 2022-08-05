using System;

[Flags]
public enum Direction
{
    None =  0,
    Forth = 1 << 0,
    Back =  1 << 1,
    Right = 1 << 2,
    Left =  1 << 3,
    Up =    1 << 4,
    Down =  1 << 5,
    All =   (1 << 6) - 1,
}
