using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[Serializable]
public class Torso : BodyPart<TorsoStats>
{
    public Rigidbody Rigidbody { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody>();
    }
}
