using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    private bool _drawDebugData = false;

    private void Awake()
    {
        Registry.AddRegister<DebugDisplay>("debug_display", this);
    }

    private void Start()
    {
        //TankPlayer.onUpdateDebugData += OnGUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            _drawDebugData = !_drawDebugData;
    }

    // private void OnGUI(float navVel, float navAcc)
    // {
    //     GUI.Label(new Rect(10, 10, 512, 20), $"_navMeshAgent.velocity.magnitude = {navVel}");
    //     GUI.Label(new Rect(10, 30, 512, 20), $"_navMeshAgent.acceleration = {navAcc}");
    // }

    private void OnDrawGizmos()
    {
        //TankPlayerGUI(1.3f, 3.2f);
    }

    //
    private void TankPlayerGUI(float navVel, float navAcc)
    {
        GUI.Label(new Rect(10, 10, 512, 20), $"_navMeshAgent.velocity.magnitude = {navVel}");
        GUI.Label(new Rect(10, 30, 512, 20), $"_navMeshAgent.acceleration = {navAcc}");
    }
}
