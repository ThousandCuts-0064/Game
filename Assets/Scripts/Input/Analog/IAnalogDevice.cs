using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAnalogDevice
{
    public IWrappedAnalogInput X { get; }
    public IWrappedAnalogInput Y { get; }
    public IWrappedAnalogInput Z { get; }
}
