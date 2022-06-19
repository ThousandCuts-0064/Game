using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WrappedMouseInput : WrappedAnalogInputTemplate<MouseInput, WrappedMouseInput>
{
    public MouseAxis MouseAxis
    {
        get => AnalogInput.MouseAxis;
        set => AnalogInput.MouseAxis = value;
    }
    public float Sensitivity
    {
        get => AnalogInput.Sensitivity;
        set => AnalogInput.Sensitivity = value;
    }

    public WrappedMouseInput(MouseInput mouseInput) : base(mouseInput) { }
}