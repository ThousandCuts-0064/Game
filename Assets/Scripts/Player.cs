using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Body _body;
    private Camera _camera;
    private PlayerDigitalDevice _digitalInput;
    private PlayerAnalogDevice _analogInput;
    private Controller _controller;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _body = new(this);
        _analogInput = new();
        _digitalInput = new();
        _controller = new(this)
        {
            Analog = _analogInput,
            Digital = _digitalInput
        };
    }

    private void Start()
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var col1 in colliders)
            foreach (var col2 in colliders)
                Physics.IgnoreCollision(col1, col2, true);
    }

    private void Update()
    {
        _analogInput.Update();
        _digitalInput.Update();
    }

    private void FixedUpdate()
    {
        _controller.FixedUpdate();
    }

    public class Controller : global::Controller
    {
        private readonly Player _player;
        private Vector3 _rotation;
        private Transform Transform => _player.transform;
        private Rigidbody Rigidbody => _player._rigidbody;
        private Body Body => _player._body;

        public Controller(Player player) => _player = player;

        public override void FixedUpdate()
        {
            //Mouse
            Vector3 addRot = new(-Analog.Z.Process(), Analog.X.Process(), Analog.Y.Process());
            Rigidbody.MoveRotation(Quaternion.Euler(0, Rigidbody.rotation.eulerAngles.y + addRot.y, 0));
            _rotation.Set(_rotation.x + addRot.x, 0, _rotation.z + addRot.z);
            _rotation.x = Math.Clamp(_rotation.x, -90, 90);
            _rotation.z = Math.Clamp(_rotation.z, -90, 90);
            Body.Neck.transform.localRotation = Quaternion.Euler(_rotation);

            //Keyboard
            Vector3 moveDir = new();
            moveDir += Transform.forward * CalcDir(Digital.Forth, Digital.Back);
            moveDir += Transform.right * CalcDir(Digital.Right, Digital.Left);
            moveDir.Normalize();
            moveDir *= (Body.Feet[0].Stats.Speed + Body.Feet[1].Stats.Speed) / 2;
            Rigidbody.MovePosition(Rigidbody.position + moveDir * Time.fixedDeltaTime);

            for (int i = 0; i < Body.Feet.Count; i++)
                TryLegJump(Rigidbody, Digital.Up, Body.Feet[i].Stats);

            //Local funcs
            static float CalcDir(IWrappedDigitalInput positive, IWrappedDigitalInput negative) =>
                (positive.IsPressed, negative.IsPressed, positive.LastPress > negative.LastPress) switch
                {
                    (true, false, _) => 1,
                    (false, true, _) => -1,
                    (true, true, true) => 1,
                    (true, true, false) => -1,
                    _ => 0
                };

            static void TryLegJump(Rigidbody rigidbody, IWrappedDigitalInput input, FootStats foot)
            {
                if (input.IsPressed && foot.TryJump())
                {
                    rigidbody.AddForce(Vector3.up * foot.JumpHeight, ForceMode.Impulse);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Player))]
    public class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying) return;

            Player player = (Player)target;

            DisplayTable(player._body.Feet);

            //Display(player._body.Feet, foot => foot.Stats,
            //    (nameof(FootStats.Health), stats => EditorGUILayout.FloatField(stats.Health)),
            //    (nameof(FootStats.IsOnGround), stats => EditorGUILayout.Toggle(stats.IsOnGround)),
            //    (nameof(FootStats.CanJump), stats => EditorGUILayout.Toggle(stats.CanJump)));

            Repaint();

            static void Display<TSource, TResult>(IEnumerable<TSource> arr, Func<TSource, TResult> selector, params (string name, Action<TResult> action)[] rows)
            {
                foreach (var row in rows)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(row.name, GUILayout.MinWidth(0));
                    foreach (var item in arr) row.action(selector(item));
                    EditorGUILayout.EndHorizontal();
                }
            }

            static void DisplayObject<T>(T obj)
            {
                foreach (var field in FindAllFields(typeof(T)))
                {
                    if (field.FieldType.IsClass)
                    {
                        EditorGUILayout.BeginFoldoutHeaderGroup(true, BackingName(field.Name));
                        DisplayObject(field.FieldType);
                        EditorGUILayout.EndFoldoutHeaderGroup();
                        continue;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(BackingName(field.Name), GUILayout.MinWidth(0));
                    var value = field.GetValue(obj);
                    switch (value)
                    {
                        case float f:
                            EditorGUILayout.FloatField(f);
                            break;

                        case bool b:
                            EditorGUILayout.Toggle(b);
                            break;

                        default:
                            EditorGUILayout.LabelField(value.ToString(), GUILayout.MinWidth(0));
                            break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            static void DisplayTable<T>(IEnumerable<T> objects)
            {
                foreach (var field in FindAllFields(typeof(T)))
                {
                    if (field.FieldType.IsClass)
                    {
                        EditorGUILayout.BeginFoldoutHeaderGroup(true, BackingName(field.Name));
                        DisplayObject(field.FieldType);
                        EditorGUILayout.EndFoldoutHeaderGroup();
                        continue;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(BackingName(field.Name), GUILayout.MinWidth(0));
                    foreach (var obj in objects)
                    {
                        var value = field.GetValue(obj);
                        switch (value)
                        {
                            case float f:
                                EditorGUILayout.FloatField(f);
                                break;

                            case bool b:
                                EditorGUILayout.Toggle(b);
                                break;

                            default:
                                EditorGUILayout.LabelField(value.ToString(), GUILayout.MinWidth(0));
                                break;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            static string BackingName(string str) => str[0] == '<' ? str[1..str.IndexOf('>')] : str;

            static IEnumerable<FieldInfo> FindAllFields(Type type)
            {
                string @namespace = type.Namespace;
                var fieldInfos = Enumerable.Empty<FieldInfo>();

                var currType = type;
                while (currType.Namespace == @namespace)
                {
                    fieldInfos = fieldInfos.Concat(currType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Where(field => field.IsDefined(typeof(SerializeField))));
                    currType = currType.BaseType;
                }

                return fieldInfos;
            }
        }

        public override bool RequiresConstantRepaint() => true;
    }
#endif
}
