using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WrappedKeyInput : WrappedDigitalInputTemplate<KeyInput, WrappedKeyInput>
{
    public KeyCode Key
    {
        get => InputInfo.Key;
        set => InputInfo.Key = value;
    }

    public WrappedKeyInput(KeyInput inputInfo) : base(inputInfo) { }
}
