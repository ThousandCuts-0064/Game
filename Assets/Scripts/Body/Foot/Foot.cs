using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Foot : BodyPart<FootStats>
{
    [field: SerializeField] public int Index { get; private set; }

    protected override void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
            if (IsUnder(collision, i))
                Stats.GroundColliders.Add(collision.collider);

        base.OnCollisionEnter(collision);
    }

    protected override void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
            if (IsUnder(collision, i))
                Stats.GroundColliders.Add(collision.collider);
            else
                Stats.GroundColliders.Remove(collision.collider);

        base.OnCollisionStay(collision);
    }

    protected override void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
            Stats.GroundColliders.Remove(collision.collider);

        base.OnCollisionExit(collision);
    }

    private bool IsUnder(Collision collision, int index) => collision.GetContact(index).point.y < transform.position.y;

#if UNITY_EDITOR

    [CustomEditor(typeof(Foot))]
    public class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Foot foot = (Foot)target;
            
            EditorGUILayout.Toggle(nameof(foot.Stats.IsOnGround), foot.Stats.IsOnGround);
        }
    }
#endif
}
