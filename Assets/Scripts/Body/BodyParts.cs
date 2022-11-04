using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BodyParts
{
    [SerializeResult] public Head Head { get; }
    [SerializeResult] public Neck Neck { get; }
    [SerializeResult] public Torso Torso { get; }
    [SerializeResult] public ReadOnlyCollection<Hand> Hands { get; }
    [SerializeResult] public ReadOnlyCollection<Foot> Feet { get; }

    public BodyParts(MonoBehaviour owner)
    {
        Torso = owner.GetComponentInChildren<Torso>();
        Head = owner.GetComponentInChildren<Head>();
        Neck = owner.GetComponentInChildren<Neck>();
        Hands = Array.AsReadOnly(owner.GetComponentsInChildren<Hand>().OrderBy(f => f.Index).ToArray());
        Feet = Array.AsReadOnly(owner.GetComponentsInChildren<Foot>().OrderBy(f => f.Index).ToArray());
    }
}
