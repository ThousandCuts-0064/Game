using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class AnalogInputTemplate<TRaw, TWrap>
    where TRaw : AnalogInputTemplate<TRaw, TWrap>
    where TWrap : WrappedAnalogInputTemplate<TRaw, TWrap>
{
    public float UnprocessedAmount { get; private set; }

    public abstract TWrap Wrap();

    public void Update(float amount) => UnprocessedAmount += amount; 

    public float Process()
    {
        try { return UnprocessedAmount; }
        finally { UnprocessedAmount = 0; }
    }
}
