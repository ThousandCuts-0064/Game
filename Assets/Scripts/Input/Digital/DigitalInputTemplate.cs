using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class DigitalInputTemplate<TRaw, TWrap>
    where TRaw : DigitalInputTemplate<TRaw, TWrap>
    where TWrap : WrappedDigitalInputTemplate<TRaw, TWrap>
{
    public double LastPress { get; private set; }
    public double LastRelease { get; private set; }
    public bool IsPressed { get; private set; }
    public bool AwaitsPressProcess { get; private set; }
    public bool AwaitsReleaseProcess { get; private set; }

    public abstract TWrap Wrap();

    public void Press()
    {
        LastPress = Time.time;
        AwaitsPressProcess = true;
        IsPressed = true;
    }

    public void Release()
    {
        LastRelease = Time.time;
        AwaitsReleaseProcess = true;
        IsPressed = false;
    }

    public bool TryProcessPress()
    {
        try { return AwaitsPressProcess; }
        finally { AwaitsPressProcess = false; }
    }

    public bool TryProcessRelease()
    {
        try { return AwaitsReleaseProcess; }
        finally { AwaitsReleaseProcess = false; }
    }
}
