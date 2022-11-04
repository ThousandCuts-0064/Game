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
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public T Stats { get; private set; }

    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerStay;
    public event Action<Collider> TriggerExit;

    protected virtual void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);
    protected virtual void OnTriggerStay(Collider other) => TriggerStay?.Invoke(other);
    protected virtual void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);
}
