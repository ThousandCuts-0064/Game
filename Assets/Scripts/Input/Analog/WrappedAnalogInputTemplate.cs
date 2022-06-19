using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WrappedAnalogInputTemplate<TRaw, TWrap> : IWrappedAnalogInput
    where TRaw : AnalogInputTemplate<TRaw, TWrap>
    where TWrap : WrappedAnalogInputTemplate<TRaw, TWrap>
{
    protected TRaw AnalogInput { get; }
    public float UnprocessedAmount => AnalogInput.UnprocessedAmount;

    public WrappedAnalogInputTemplate(TRaw analogInput) => AnalogInput = analogInput;

    public float Process() => AnalogInput.Process();
}
