using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KeyInput : DigitalInputTemplate<KeyInput, WrappedKeyInput>
{
    public KeyCode Key { get; set; }

    public override WrappedKeyInput Wrap() => new(this);

    public void Update()
    {
        if (Input.GetKeyDown(Key)) Press();
        if (Input.GetKeyUp(Key)) Release();
    }
}
