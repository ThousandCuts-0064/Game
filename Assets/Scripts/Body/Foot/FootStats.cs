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
    private float _lastJump;
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float JumpHeight { get; set; }
    [field: SerializeField] public float JumpCooldown { get; set; }
    [SerializeResult] public bool IsOnGround { get; set; }
    [SerializeResult] public bool CanJump => Time.time - _lastJump >= JumpCooldown;
}