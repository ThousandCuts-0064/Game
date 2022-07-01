using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Foot : BodyPart<FootStats>
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (IsUnder(other))
            Stats.GroundColliders.Add(other);

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (IsUnder(other))
            Stats.GroundColliders.Add(other);
        else
            Stats.GroundColliders.Remove(other);

        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        Stats.GroundColliders.Remove(other);

        base.OnTriggerExit(other);
    }

    private bool IsUnder(Collider other) => Collider.ClosestPoint(other.transform.position).y < transform.position.y;

#if UNITY_EDITOR
    [CustomEditor(typeof(Foot))]
    public class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Foot foot = (Foot)target;

            EditorGUILayout.Toggle(nameof(foot.Stats.IsOnGround), foot.Stats.IsOnGround);
            EditorGUILayout.Toggle(nameof(foot.Stats.CanJump), foot.Stats.CanJump);

            Repaint();
        }

        public override bool RequiresConstantRepaint() => true;
    }
#endif
}
