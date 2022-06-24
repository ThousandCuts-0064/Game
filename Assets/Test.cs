using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Vector3 _rotation;

    private void Awake()
    {
        _rotation = transform.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        _rotation.x += 1;
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(_rotation));
    }
}
