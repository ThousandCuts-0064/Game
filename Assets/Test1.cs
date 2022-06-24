using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        var rb = transform.root.GetComponent<Rigidbody>();
        rb.AddForceAtPosition(-rb.velocity, transform.position, ForceMode.Impulse);
    }
}
