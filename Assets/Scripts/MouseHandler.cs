using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    Tank _player;
    Camera _camera;

    private void Start()
    {
        _player = Registry.GetRegister<Tank>("player");
        _camera = Registry.GetRegister<Camera>("camera");
    }

    private void Update()
    {
        _player.AimAt(_camera.ScreenToWorldPoint(Input.mousePosition));
    }
}
