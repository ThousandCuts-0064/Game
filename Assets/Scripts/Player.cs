using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private readonly PlayerInput _controller;

    private void Update()
    {
        _controller.Detect();
    }

    private void FixedUpdate()
    {
        
    }

    private class Controller
    {

    }
}
