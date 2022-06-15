using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KeyInputInfo : InputInfoTemplate<KeyInputInfo, WrappedKeyInputInfo>
{
    public KeyCode Key { get; set; }

    public override WrappedKeyInputInfo Wrap() => new(this);

    public void Update()
    {
        if (Input.GetKey(Key)) Press();
        else Release();
    }
}
