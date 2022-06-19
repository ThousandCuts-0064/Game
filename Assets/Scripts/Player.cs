using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private BodyBehaviours _body;
    private Camera _camera;
    private PlayerDigitalDevice _digitalInput;
    private PlayerAnalogDevice _analogInput;
    private Controller _controller;
    private Stats _stats;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _body = new(this);
        _analogInput = new();
        _digitalInput = new();
        _stats = new(
            new Foot[]
            {
                new()
                {
                    Speed = 2,
                    JumpHeight = 3,
                },
                new()
                {
                    Speed = 2,
                    JumpHeight = 3,
                },
            });
        _controller = new(this)
        {
            Analog = _analogInput,
            Digital = _digitalInput
        };

        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Start()
    {
        Physics.IgnoreCollision(_body.Torso.Collider, _body.Feet[0].Collider);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            var contact = collision.GetContact(i);
            for (int y = 0; y < _body.Feet.Count; y++)
            {
                if (CheckFoot(contact, y)) break;
            }
        }

        bool CheckFoot(ContactPoint contact, int index)
        {
            Collider footCol = _body.Feet[index].Collider;
            if (contact.thisCollider != footCol) return false;

            if (contact.point.y < footCol.transform.position.y)
                _stats.Feet[index].GroundCollisions[collision] = Time.time;
            return true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            var contact = collision.GetContact(i);
            for (int y = 0; y < _body.Feet.Count; y++)
            {
                if (CheckFoot(contact, y)) break;
            }
        }

        bool CheckFoot(ContactPoint contact, int index)
        {
            Collider footCol = _body.Feet[index].Collider;
            if (contact.thisCollider != footCol) return false;

            if (contact.point.y < footCol.transform.position.y)
                _stats.Feet[index].GroundCollisions[collision] = Time.time;
            else
                _stats.Feet[index].GroundCollisions.Remove(collision);
            return true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < _body.Feet.Count; i++)
        {
            if (CheckFoot(i)) break;
        }

        bool CheckFoot(int index)
        {
            var collisions = _stats.Feet[index].GroundCollisions;
            Collision[] keys = new Collision[collisions.Count];
            float[] values = new float[collisions.Count];
            collisions.Keys.CopyTo(keys, 0);
            collisions.Values.CopyTo(values, 0);

            for (int i = 0; i < keys.Length; i++)
            {
                if (Time.time - values[i] > Time.fixedDeltaTime)
                {
                    collisions.Remove(keys[i]);
                    return true;
                }
            }
            return false;
        }
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

    private class Controller : global::Controller
    {
        private readonly Player _player;
        private Vector3 _rotation;
        private Transform Transform => _player.transform;
        private Rigidbody Rigidbody => _player._rigidbody;
        private BodyBehaviours Body => _player._body;
        private Stats Stats => _player._stats;

        public Controller(Player player) => _player = player;

        public override void FixedUpdate()
        {
            //Mouse
            Vector3 addRot = new(-Analog.Z.Process(), Analog.X.Process(), Analog.Y.Process());
            Transform.Rotate(0, addRot.y, 0);
            _rotation.Set(_rotation.x + addRot.x, 0, _rotation.z + addRot.z);
            _rotation.x = Math.Clamp(_rotation.x, -90, 90);
            _rotation.z = Math.Clamp(_rotation.z, -90, 90);
            Body.Neck.localEulerAngles = _rotation;

            //Keyboard
            Vector3 moveDir = new();
            moveDir += Transform.forward * CalcDir(Digital.Forth, Digital.Back);
            moveDir += Transform.right * CalcDir(Digital.Right, Digital.Left);
            moveDir.Normalize();
            moveDir *= (Stats.Feet[0].Speed + Stats.Feet[1].Speed) / 2;
            Rigidbody.MovePosition(Rigidbody.position + moveDir * Time.fixedDeltaTime);

            for (int i = 0; i < Stats.Feet.Count; i++)
                TryLegJump(Rigidbody, Digital.Up, Stats.Feet[i]);

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

            static void TryLegJump(Rigidbody rigidbody, IWrappedDigitalInput input, Foot foot)
            {
                if (foot.IsOnGround && input.IsPressed)
                {
                    rigidbody.AddForce(Vector3.up * foot.JumpHeight, ForceMode.Impulse);
                    foot.GroundCollisions.Clear();
                }
            }
        }
    }
}
