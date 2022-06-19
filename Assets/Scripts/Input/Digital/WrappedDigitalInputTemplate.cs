using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class WrappedDigitalInputTemplate<TRaw, TWrap> : IWrappedDigitalInput
    where TRaw : DigitalInputTemplate<TRaw, TWrap>
    where TWrap : WrappedDigitalInputTemplate<TRaw, TWrap>
{
    protected TRaw InputInfo { get; }
    public double LastPress => InputInfo.LastPress;
    public double LastRelease => InputInfo.LastRelease;
    public bool IsPressed => InputInfo.IsPressed;
    public bool AwaitsPressProcess => InputInfo.AwaitsPressProcess;
    public bool AwaitsReleaseProcess => InputInfo.AwaitsReleaseProcess;

    public WrappedDigitalInputTemplate(TRaw inputInfo) => InputInfo = inputInfo;

    public bool TryProcessPress() => InputInfo.TryProcessPress();
    public bool TryProcessRelease() => InputInfo.TryProcessRelease();
}
