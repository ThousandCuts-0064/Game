using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(5 * Mathf.Sqrt(2) * Vector3.up, ForceMode.VelocityChange);
    }
}
