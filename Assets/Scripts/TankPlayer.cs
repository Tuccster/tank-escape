using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankPlayer : Tank
{
    private Vector3 _movePointForward;
    private Vector3 _movePointBack;
    private Vector3 _stoppingDistanceTarget;

    private float _topSpeed;

    public bool _solution;

    public float _stoppingDistance;
    public float _turnSpeed = 3;
    public float _accelerationCutoffSpeed;
    public float _movePointDistance = 0.5f;
    private float _baseAcceleration;

    protected override void Awake()
    {
        base.Awake();
        Registry.AddRegister("player", this);

        _baseAcceleration = _navMeshAgent.acceleration;

        //MoveToPosition(transform.forward * 30);
    }

    private void Start()
    {

    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log(_movePointForward);

        _movePointForward = _transform.position + (_transform.forward * _movePointDistance);
        _movePointBack = _transform.position + (_transform.forward * -_movePointDistance);

        if (Input.GetKey(KeyCode.A)) transform.rotation *= Quaternion.Euler(0, -_turnSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D)) transform.rotation *= Quaternion.Euler(0, _turnSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.W)) MoveToPosition(_movePointForward);
        if (Input.GetKey(KeyCode.S)) MoveToPosition(_movePointBack);

        if (_navMeshAgent.velocity.magnitude > _topSpeed)
            _topSpeed = _navMeshAgent.velocity.magnitude;

        if (_solution)
        {
            _stoppingDistanceTarget = transform.position + (transform.forward * -0.25f);
            if (Vector3.Distance(_navMeshAgent.path.corners[_navMeshAgent.path.corners.Length - 1], _stoppingDistanceTarget) <= _stoppingDistance)
                _navMeshAgent.velocity = Vector3.zero;
        }

        /* RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.55f))
        {
            _navMeshAgent.stoppingDistance = 0.0f;
        }
        else
        {
            _navMeshAgent.stoppingDistance = 0.25f;
        } */

        // Used for speed graph
        speeds.Add(_navMeshAgent.velocity.magnitude);

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _movePointDistance + 1))
        //{
        //    _movePointDistance = hit.distance;
        //}

        //This solution works fine but only if we are able to get up to speed in time for the corner//
        /* 
        if (Maths.IsApproximately(_navMeshAgent.velocity.magnitude, _accelerationCutoffSpeed, 0.1f))
            _navMeshAgent.acceleration = _extendedAcceleration;
        else
            _navMeshAgent.acceleration = _baseAcceleration; 
        */ 
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.05f);

        /*
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_stoppingDistanceTarget, 0.05f);
        Gizmos.DrawWireSphere(_stoppingDistanceTarget, _stoppingDistance);
        */

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_transform.position, _navMeshAgent.stoppingDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_movePointForward, 0.05f);
        Gizmos.DrawSphere(_movePointBack, 0.05f);
        // Color doens't seem to work *big sad*
        //UnityEditor.Handles.color = Color.magenta;
        //UnityEditor.Handles.Label(_movePointForward, "_movePoint");

        base.OnDrawGizmos();
    }

    // Used for speed graph
    private List<float> speeds = new List<float>();

    // Should be put in "debug_display" in the registry **KEEP IT SIMPLE**
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 512, 20), $"_navMeshAgent.velocity.magnitude = {_navMeshAgent.velocity.magnitude}");
        GUI.Label(new Rect(10, 30, 512, 20), $"_navMeshAgent.acceleration = {_navMeshAgent.acceleration}");
        GUI.Label(new Rect(10, 50, 512, 20), $"_topSpeed = {_topSpeed}");

        // Graph for visualizing speed
        for (int i = 0; i < speeds.Count; i++)
            GUI.Box(new Rect(i + 10, Screen.height - (speeds[i] * 100), 1, 1), "/");

        if (speeds.Count > Screen.width)
            speeds.Clear();
    }
}
