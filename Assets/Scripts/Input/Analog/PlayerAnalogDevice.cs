using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerAnalogDevice : AnalogDevice<MouseInput, WrappedMouseInput>
{
    public PlayerAnalogDevice()
    {
        X.MouseAxis = MouseAxis.MouseX;
        Y.MouseAxis = MouseAxis.MouseScrollWheel;
        Y.Sensitivity = 50;
        Z.MouseAxis = MouseAxis.MouseY;
    }

    public void Update()
    {
        RawX.Update();
        RawY.Update();
        RawZ.Update();
    }
}
