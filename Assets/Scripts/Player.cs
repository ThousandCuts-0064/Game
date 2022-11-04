using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Camera _camera;
    private PlayerDigitalDevice _digitalInput;
    private PlayerAnalogDevice _analogInput;
    private Controller _controller;
    public BodyParts BodyParts { get; private set; }
    [field: SerializeField]public float LowestPoint { get; private set; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        BodyParts = new(this);
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
        private BodyParts BodyParts => _player.BodyParts;

        public Controller(Player player) => _player = player;

        public override void FixedUpdate()
        {
            //Mouse
            Vector3 addRot = new(-Analog.Z.Process(), Analog.X.Process(), Analog.Y.Process());
            BodyParts.Torso.Rigidbody.MoveRotation(Quaternion.Euler(0, BodyParts.Torso.Rigidbody.rotation.eulerAngles.y + addRot.y, 0));
            _rotation.Set(_rotation.x + addRot.x, 0, _rotation.z + addRot.z);
            _rotation.x = Math.Clamp(_rotation.x, -90, 90);
            _rotation.z = Math.Clamp(_rotation.z, -90, 90);
            BodyParts.Neck.transform.localRotation = Quaternion.Euler(_rotation);

            //Keyboard
            Vector3 moveDir = new();
            moveDir += Transform.forward * CalcDir(Digital.Forth, Digital.Back);
            moveDir += Transform.right * CalcDir(Digital.Right, Digital.Left);
            moveDir.Normalize();
            moveDir *= (BodyParts.Feet[0].Stats.Speed + BodyParts.Feet[1].Stats.Speed) / 2;
            Rigidbody.MovePosition(Rigidbody.position + moveDir * Time.fixedDeltaTime);

            for (int i = 0; i < BodyParts.Feet.Count; i++)
                TryLegJump(Rigidbody, Digital.Up, BodyParts.Feet[i].Stats);

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
                if (input.IsPressed && foot.CanJump)
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
        private static Dictionary<string, bool> _foldouts = new();

        private void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying) return;

            Player player = (Player)target;

            DisplayObject(player.BodyParts);

            Repaint();

            static void DisplayObject(object obj, string recursiveName = null)
            {
                foreach (var member in FindAllSerializableMembers(obj.GetType()))
                {
                    if (!GetReturnType(member).IsClass)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(BackingName(member.Name), GUILayout.MinWidth(0));
                        DisplayValue(member, obj);
                        EditorGUILayout.EndHorizontal();
                        continue;
                    }

                    string fullName = recursiveName + BackingName(member.Name);
                    if (!_foldouts.ContainsKey(fullName)) _foldouts.Add(fullName, false);
                    if (_foldouts[fullName] = EditorGUILayout.BeginToggleGroup(fullName, _foldouts[fullName]))
                    {
                        if (GetValue(member, obj) is IEnumerable<object> returnedValue)
                            DisplayTable(returnedValue, fullName);
                        else
                            DisplayObject(GetValue(member, obj), fullName);
                    }
                    EditorGUILayout.EndToggleGroup();
                }
            }

            static void DisplayTable(IEnumerable<object> objects, string recursiveName = null)
            {
                foreach (var member in FindAllSerializableMembers(objects.First().GetType()))
                {
                    if (!GetReturnType(member).IsClass)
                    {
                        DisplayRow(member, objects);
                        continue;
                    }

                    string fullName = recursiveName + BackingName(member.Name);
                    if (!_foldouts.ContainsKey(fullName)) _foldouts.Add(fullName, false);
                    if (_foldouts[fullName] = EditorGUILayout.BeginToggleGroup(fullName, _foldouts[fullName]))
                    {
                        var classes = objects.Select(obj => GetValue(member, obj)).ToList();
                        var innerMembers = FindAllSerializableMembers(classes.First().GetType());
                        foreach (var innerMember in innerMembers)
                            DisplayRow(innerMember, classes);
                    }
                    EditorGUILayout.EndToggleGroup();
                }

                static void DisplayRow(MemberInfo member, IEnumerable<object> objects)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(BackingName(member.Name), GUILayout.MinWidth(0));
                    foreach (var obj in objects)
                        DisplayValue(member, obj);
                    EditorGUILayout.EndHorizontal();
                }
            }

            static string BackingName(string str) => str[0] == '<' ? str[1..str.IndexOf('>')] : str;

            static object DisplayValue(MemberInfo member, object obj) =>
                GetValue(member, obj) switch
                {
                    float f => EditorGUILayout.FloatField(f, GUILayout.MinWidth(0)),
                    bool b => EditorGUILayout.Toggle(b, GUILayout.MinWidth(0)),
                    object o => new Func<object>(() =>
                    {
                        EditorGUILayout.LabelField(o.ToString(), GUILayout.MinWidth(0));
                        return null;
                    })()
                };

            static object GetValue(MemberInfo member, object obj) =>
                member switch
                {
                    FieldInfo field => field.GetValue(obj),
                    PropertyInfo property => property.GetValue(obj),
                    MethodInfo method => method.Invoke(obj, null),
                    _ => throw new NotSupportedException($"{nameof(member)} is not field, property or method")
                };

            static Type GetReturnType(MemberInfo member) =>
                member switch
                {
                    FieldInfo field => field.FieldType,
                    PropertyInfo property => property.PropertyType,
                    MethodInfo method => method.ReturnType,
                    _ => throw new NotSupportedException($"{nameof(member)} is not field, property or method")
                };

            static IEnumerable<MemberInfo> FindAllSerializableMembers(Type type)
            {
                string @namespace = type.Namespace;
                BindingFlags flags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly;

                var currType = type;
                var fieldInfos = Enumerable.Empty<FieldInfo>();
                var propertyInfos = Enumerable.Empty<PropertyInfo>();
                while (currType.Namespace == @namespace)
                {
                    fieldInfos = currType.GetFields(flags)
                        .Where(field => field.IsDefined(typeof(SerializeField)))
                        .Concat(fieldInfos);

                    propertyInfos = currType.GetProperties(flags)
                        .Where(field => field.IsDefined(typeof(SerializeResultAttribute)))
                        .Concat(propertyInfos);

                    currType = currType.BaseType;
                }

                return Enumerable.Empty<MemberInfo>().Concat(fieldInfos).Concat(propertyInfos);
            }
        }

        public override bool RequiresConstantRepaint() => true;
    }
#endif
}
