using System.Collections.Generic;
using UnityEngine;

public abstract class InputMethod<TRaw, TWrap> 
    where TRaw : InputInfoTemplate<TRaw, TWrap>, new() 
    where TWrap : WrappedInputInfoTemplate<TRaw, TWrap>
{
    protected TRaw RawForth { get; } = new();
    protected TRaw RawBack { get; } = new();
    protected TRaw RawLeft { get; } = new();
    protected TRaw RawRight { get; } = new();
    protected TRaw RawUp { get; } = new();
    protected TRaw RawDown { get; } = new();
    public TWrap Forth { get; }
    public TWrap Back { get; } 
    public TWrap Left { get; } 
    public TWrap Right { get; }
    public TWrap Up { get; }
    public TWrap Down { get; } 

    public InputMethod()
    {
        Forth = RawForth.Wrap();
        Back = RawBack.Wrap();
        Left = RawLeft.Wrap();
        Right = RawRight.Wrap();
        Up = RawRight.Wrap();
        Down = RawDown.Wrap();
    }
}   