using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWrappedDigitalInput
{
    public double LastPress { get; }
    public bool IsPressed { get; }
    public bool AwaitsPressProcess { get; }

    public bool TryProcessPress();
    public bool TryProcessRelease();

}
