using System.Collections.Generic;
using UnityEngine;

public abstract class InputMethod
{
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public int Z { get; protected set; }

    public abstract void Detect();
}
