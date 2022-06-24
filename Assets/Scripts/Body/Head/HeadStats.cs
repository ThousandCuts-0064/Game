using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class HeadStats : Stats
{
    [field: SerializeField] public float Vission { get; private set; }
}
