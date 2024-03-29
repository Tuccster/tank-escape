﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankPlayer : Tank
{
    private Vector3 _movePointForward;
    private Vector3 _movePointBack;

    public float _stoppingDistance;
    public float _turnSpeed = 3;
    public float _movePointDistance = 0.5f;

    public delegate void OnUpdateDebugDataEvent(float navVel, float navAcc);
    public static event OnUpdateDebugDataEvent onUpdateDebugData;

    protected override void Awake()
    {
        base.Awake();
        Registry.AddRegister("player", this);
    }

    protected override void Update()
    {
        base.Update();

        _movePointForward = transform.position + (transform.forward * _movePointDistance);
        _movePointBack = transform.position + (transform.forward * -_movePointDistance);

        if (Input.GetKey(KeyCode.A)) transform.rotation *= Quaternion.Euler(0, -_turnSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D)) transform.rotation *= Quaternion.Euler(0, _turnSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.W)) MoveToPosition(_movePointForward);
        if (Input.GetKey(KeyCode.S)) MoveToPosition(_movePointBack);

        if (onUpdateDebugData != null) 
            onUpdateDebugData(_navMeshAgent.velocity.magnitude, _navMeshAgent.acceleration);

        if (Input.GetKeyDown(KeyCode.F)) ShootProjectile(0.5f);
    }

    public override void Kill()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        transform.position = Vector3.zero;
    }

    protected override void OnDrawGizmos()
    {
        if (!GameRules._drawOnDrawGizmos) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.05f);

        Gizmos.color = Color.cyan;
        if (_navMeshAgent) Gizmos.DrawWireSphere(transform.position, _navMeshAgent.stoppingDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_movePointForward, 0.05f);
        Gizmos.DrawSphere(_movePointBack, 0.05f);

        base.OnDrawGizmos();
    }

    // Should be put in "debug_display" in the registry **KEEP IT SIMPLE**
    void OnGUI()
    {
        if (!GameRules._drawOnGUI) return;

        GUI.Label(new Rect(10, 10, 512, 20), $"_navMeshAgent.velocity.magnitude = {_navMeshAgent.velocity.magnitude}");
        GUI.Label(new Rect(10, 30, 512, 20), $"_navMeshAgent.acceleration = {_navMeshAgent.acceleration}");
    }
}
