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
    public Collider Trigger { get; private set; }
    [field: SerializeField] public T Stats { get; private set; }

    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerStay;
    public event Action<Collider> TriggerExit;

    protected virtual void Awake()
    {
        if (!TryGetComponent(out Collider coll)) return;
        
        Collider = coll;
        switch (Collider)
        {
            case CapsuleCollider:
                var capsule = gameObject.AddComponent<CapsuleCollider>();
                float ratio = capsule.height / capsule.radius;
                capsule.radius += 2 * Physics.defaultContactOffset;
                capsule.height += ratio * 2 * Physics.defaultContactOffset;
                Trigger = capsule;
                break;

            case SphereCollider:
                var sphere = gameObject.AddComponent<SphereCollider>();
                sphere.radius += 2 * Physics.defaultContactOffset;
                Trigger = sphere;
                break;

            default: throw new NotImplementedException();
        }
        Trigger.isTrigger = true;
        //Stats = new(); Serialized
    }

    protected virtual void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);
    protected virtual void OnTriggerStay(Collider other) => TriggerStay?.Invoke(other);
    protected virtual void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);
}
