using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WrappedKeyInputInfo : WrappedInputInfoTemplate<KeyInputInfo, WrappedKeyInputInfo>
{
    public KeyCode Key
    {
        get => InputInfo.Key;
        set => InputInfo.Key = value;
    }

    public WrappedKeyInputInfo(KeyInputInfo inputInfo) : base(inputInfo) { }
}
