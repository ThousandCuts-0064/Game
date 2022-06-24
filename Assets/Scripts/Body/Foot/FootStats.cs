using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class FootStats : Stats
{
    public HashSet<Collider> GroundColliders { get; } = new();
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float JumpHeight { get; set; }
    public bool IsOnGround => GroundColliders.Count != 0;
}