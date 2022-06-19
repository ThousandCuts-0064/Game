using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FootBehaviour : BodyBehaviour
{
    [field: SerializeField] public int Index { get; private set; }
}
