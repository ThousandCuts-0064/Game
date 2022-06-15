using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WrappedInputInfo : WrappedInputInfoTemplate<InputInfo, WrappedInputInfo>
{
    public WrappedInputInfo(InputInfo inputInfo) : base(inputInfo) { }
}
