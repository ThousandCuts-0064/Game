using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WrappedAnalogInput : WrappedAnalogInputTemplate<AnalogInput, WrappedAnalogInput>
{
    public WrappedAnalogInput(AnalogInput analogInput) : base(analogInput) { }
}
