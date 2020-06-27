using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    private Tank _player;
    private Camera _camera;

    private Vector3 _mouseWorldPos;

    private void Start()
    {
        _player = Registry.GetRegister<Tank>("player");
        _camera = Registry.GetRegister<Camera>("camera");
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            _mouseWorldPos = raycastHit.point;
        }
        _player.AimAt(_mouseWorldPos);
    }

    private void OnDrawGizmos()
    {
        if (!GameRules._drawOnDrawGizmos) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_mouseWorldPos, 0.1f);
    }
}
