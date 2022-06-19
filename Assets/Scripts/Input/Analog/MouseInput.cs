using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MouseInput : AnalogInputTemplate<MouseInput, WrappedMouseInput>
{
    private string _axis;
    public MouseAxis MouseAxis 
    {
        get => Enum.Parse<MouseAxis>(_axis);
        set => _axis = value.ToString();
    }
    public float Sensitivity { get; set; } = 1;

    public override WrappedMouseInput Wrap() => new(this);

    public void Update() => Update(Input.GetAxisRaw(_axis) * Sensitivity);
}
