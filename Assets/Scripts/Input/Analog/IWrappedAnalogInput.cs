using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWrappedAnalogInput
{
    public float UnprocessedAmount { get; }

    public float Process();
}
