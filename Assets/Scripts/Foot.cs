using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Foot
{
    public Dictionary<Collision, float> GroundCollisions { get; } = new();
    public float Speed { get; set; }
    public float JumpHeight { get; set; }
    public bool IsOnGround => GroundCollisions.Count != 0;
}
