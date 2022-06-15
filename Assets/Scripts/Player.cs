using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput _input;

    private void Awake()
    {
        _input = new();
    }

    private void Update()
    {
        _input.Update();
    }

    private void FixedUpdate()
    {
        
    }

    private class Controller
    {

    }
}
