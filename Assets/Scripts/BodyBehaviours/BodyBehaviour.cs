using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BodyBehaviour : MonoBehaviour
{
    public Collider Collider { get; private set; }

    protected virtual void Awake()
    {
        Collider = GetComponent<Collider>();
    }
}
