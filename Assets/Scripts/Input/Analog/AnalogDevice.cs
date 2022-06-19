using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnalogDevice<TRaw, TWrap> : IAnalogDevice
    where TRaw : AnalogInputTemplate<TRaw, TWrap>, new()
    where TWrap : WrappedAnalogInputTemplate<TRaw, TWrap>
{
    protected TRaw RawX { get; } = new();
    protected TRaw RawY { get; } = new();
    protected TRaw RawZ { get; } = new();
    public TWrap X { get; }
    public TWrap Y { get; }
    public TWrap Z { get; }
    IWrappedAnalogInput IAnalogDevice.X => X;
    IWrappedAnalogInput IAnalogDevice.Y => Y;
    IWrappedAnalogInput IAnalogDevice.Z => Z;

    public AnalogDevice()
    {
        X = RawX.Wrap();
        Y = RawY.Wrap();
        Z = RawZ.Wrap();
    }
}
