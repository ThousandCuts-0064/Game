using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Body
{
    public Torso Torso { get; }
    public Neck Neck { get; }
    public Head Head { get; }
    public ReadOnlyCollection<Foot> Feet { get; }

    public Body(MonoBehaviour owner)
    {
        Torso = owner.GetComponentInChildren<Torso>();
        Head = owner.GetComponentInChildren<Head>();
        Neck = owner.GetComponentInChildren<Neck>();
        Feet = Array.AsReadOnly(owner.GetComponentsInChildren<Foot>().OrderBy(f => f.Index).ToArray());
    }
}
