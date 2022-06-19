using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WrappedDigitalInput : WrappedDigitalInputTemplate<DigitalInput, WrappedDigitalInput>
{
    public WrappedDigitalInput(DigitalInput inputInfo) : base(inputInfo) { }
}
