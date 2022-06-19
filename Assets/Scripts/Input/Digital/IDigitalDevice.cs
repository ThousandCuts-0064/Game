using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDigitalDevice
{
    public IWrappedDigitalInput Forth { get; }
    public IWrappedDigitalInput Back { get; }
    public IWrappedDigitalInput Left { get; }
    public IWrappedDigitalInput Right { get; }
    public IWrappedDigitalInput Up { get; }
    public IWrappedDigitalInput Down { get; }
}
