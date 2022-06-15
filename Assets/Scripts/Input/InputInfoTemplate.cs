using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class InputInfoTemplate<TRaw, TWrap> 
    where TRaw : InputInfoTemplate<TRaw, TWrap>
    where TWrap : WrappedInputInfoTemplate<TRaw, TWrap>
{
    public double LastPress { get; private set; }
    public bool IsPressed { get; private set; }
    public bool AwaitsProcess { get; private set; }

    public abstract TWrap Wrap();

    public void Press()
    {
        LastPress = Time.time;
        if (!IsPressed) AwaitsProcess = true;
        IsPressed = true;
    }

    public void Release()
    {
        IsPressed = false;
    }

    public bool TryProcess()
    {
        try { return AwaitsProcess; }
        finally { AwaitsProcess = false; }
    }
}
