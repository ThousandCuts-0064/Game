using System.Collections.Generic;
using UnityEngine;

public abstract class DigitalDevice<TRaw, TWrap> : IDigitalDevice
    where TRaw : DigitalInputTemplate<TRaw, TWrap>, new() 
    where TWrap : WrappedDigitalInputTemplate<TRaw, TWrap>
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
    IWrappedDigitalInput IDigitalDevice.Forth => Forth;
    IWrappedDigitalInput IDigitalDevice.Back => Back;
    IWrappedDigitalInput IDigitalDevice.Left => Left;
    IWrappedDigitalInput IDigitalDevice.Right => Right;
    IWrappedDigitalInput IDigitalDevice.Up => Up;
    IWrappedDigitalInput IDigitalDevice.Down => Down;

    public DigitalDevice()
    {
        Forth = RawForth.Wrap();
        Back = RawBack.Wrap();
        Left = RawLeft.Wrap();
        Right = RawRight.Wrap();
        Up = RawUp.Wrap();
        Down = RawDown.Wrap();
    }
}   