using System;
using System.Collections.Generic;
using System.Linq;
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
            Neck neck = Body.Neck;
            neck.Rigidbody.MoveRotation(Quaternion.Euler(_rotation));
            //neck.Rigidbody.MovePosition(neck.transform.TransformPoint(neck.transform.localPosition));
            //neck.Rigidbody.MovePosition(neck.transform.TransformPoint(new(0,1,0)));

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
                if (foot.IsOnGround && input.IsPressed)
                {
                    rigidbody.AddForce(Vector3.up * foot.JumpHeight, ForceMode.Impulse);
                    foot.GroundColliders.Clear();
                }
            }
        }
    }
}
