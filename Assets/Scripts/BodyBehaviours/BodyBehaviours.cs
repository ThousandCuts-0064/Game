using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BodyBehaviours
{
    public TorsoBehaviour Torso { get; }
    public Transform Neck { get; }
    public HeadBehaviour Head { get; }
    public ReadOnlyCollection<FootBehaviour> Feet { get; }

    public BodyBehaviours(MonoBehaviour owner)
    {
        Torso = owner.GetComponentInChildren<TorsoBehaviour>();
        Head = owner.GetComponentInChildren<HeadBehaviour>();
        Neck = Head.transform.parent;
        Feet = Array.AsReadOnly(owner.GetComponentsInChildren<FootBehaviour>().OrderBy(f => f.Index).ToArray());
    }
}
