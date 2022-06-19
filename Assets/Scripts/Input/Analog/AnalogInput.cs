using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnalogInput : AnalogInputTemplate<AnalogInput, WrappedAnalogInput>
{
    public override WrappedAnalogInput Wrap() => new(this);
}
