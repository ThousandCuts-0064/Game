using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Controller
{
    public IDigitalDevice Digital { get; set; }
    public IAnalogDevice Analog { get; set; }

    public abstract void FixedUpdate();
}
