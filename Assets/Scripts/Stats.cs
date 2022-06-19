using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stats
{
    public ReadOnlyCollection<Foot> Feet { get; }

    public Stats(Foot[] feet)
    {
        Feet = new ReadOnlyCollection<Foot>(feet);
    }
}
