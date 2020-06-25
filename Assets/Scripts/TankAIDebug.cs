using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDebug : Tank
{
    private Camera _camera;

    private void Start()
    {
        _camera = Registry.GetRegister<Camera>("camera");
    }

    protected override void Awake()
    {
        
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MoveToPosition(_camera.ViewportToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2)));
        }
    }
}
