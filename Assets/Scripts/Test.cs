using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.F)) return;

        GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0, 0, 0.1f));
    }
}
