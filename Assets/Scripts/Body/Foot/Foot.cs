using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Foot : BodyPart<FootStats>
{
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
