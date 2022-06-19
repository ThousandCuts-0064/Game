using System.Collections.Generic;
using UnityEngine;

public class PlayerDigitalDevice : DigitalDevice<KeyInput, WrappedKeyInput>
{
    public PlayerDigitalDevice()
    {
        Forth.Key = KeyCode.W;
        Back.Key = KeyCode.S;
        Left.Key = KeyCode.A;
        Right.Key = KeyCode.D;
        Up.Key = KeyCode.Space;
        Down.Key = KeyCode.LeftControl;
    }

    public void Update()
    {
        RawForth.Update();
        RawBack.Update();
        RawLeft.Update();
        RawRight.Update();
        RawUp.Update();
        RawDown.Update();
    }
}
