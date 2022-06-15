using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InputInfo : InputInfoTemplate<InputInfo, WrappedInputInfo>
{
    public override WrappedInputInfo Wrap() => new(this);
}
