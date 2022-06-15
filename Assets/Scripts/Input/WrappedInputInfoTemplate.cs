using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class WrappedInputInfoTemplate<TRaw, TWrap> 
    where TRaw : InputInfoTemplate<TRaw, TWrap>
    where TWrap : WrappedInputInfoTemplate<TRaw, TWrap>
{
    protected TRaw InputInfo { get; }
    public double LastPress => InputInfo.LastPress;
    public bool IsPressed => InputInfo.IsPressed;
    public bool AwaitsProcess => InputInfo.AwaitsProcess;

    public WrappedInputInfoTemplate(TRaw inputInfo) => InputInfo = inputInfo;

    public bool TryProcess() => InputInfo.TryProcess();
}
