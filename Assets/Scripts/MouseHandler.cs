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

    private void FixedUpdate()
    {
        //Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = _camera.nearClipPlane;

        //_player.AimAt(_camera.ScreenToWorldPoint(mousePosition));
        //Debug.Log(_camera.ScreenToWorldPoint(mousePosition));
    }
}
