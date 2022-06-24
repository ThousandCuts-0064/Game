using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class BodyPart<T> : MonoBehaviour, IHaveStats<T> where T : Stats, new()
{
    public Collider Collider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public T Stats { get; private set; }

    public event Action<Collision> CollisionEnter;
    public event Action<Collision> CollisionStay;
    public event Action<Collision> CollisionExit;

    protected virtual void Awake()
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        //Stats = new();
    }

    protected virtual void OnCollisionEnter(Collision collision) => CollisionEnter?.Invoke(collision);
    protected virtual void OnCollisionStay(Collision collision) => CollisionStay?.Invoke(collision);
    protected virtual void OnCollisionExit(Collision collision) => CollisionExit?.Invoke(collision);
}
