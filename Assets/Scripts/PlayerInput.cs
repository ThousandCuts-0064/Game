using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : InputMethod
{
    public KeyCode Forth { get; set; } = KeyCode.W;
    public KeyCode Back { get; set; } = KeyCode.S;
    public KeyCode Left { get; set; } = KeyCode.A;
    public KeyCode Right { get; set; } = KeyCode.D;
    public KeyCode Up { get; set; } = KeyCode.Space;
    public KeyCode Down { get; set; } = KeyCode.LeftControl;

    public override void Detect()
    {
        X += PressedOrReleased(Left) - PressedOrReleased(Right);
        Y += PressedOrReleased(Up) - PressedOrReleased(Down);
        Z += PressedOrReleased(Forth) - PressedOrReleased(Back);

        static int PressedOrReleased(KeyCode key)
        {
            int result = 0;
            if (Input.GetKeyDown(key)) result++;
            if (Input.GetKeyUp(key)) result--;
            return result;
        }
    }
}
