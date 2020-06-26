using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour
{
    public GameObject _projectile;
    public Transform _target;

    [SerializeField]
    protected Transform _bodyTrans, _turretTrans, _barrelTrans, _shootPoint;
    protected Transform _transform;

    protected NavMeshAgent _navMeshAgent;
    protected LineRenderer _lineRenderer;

    private Vector3 _barrelLookAtTarget;
    protected float _vertDisplacement = 5;

    public delegate void OnRotateEvent(Quaternion rotation);
    public static event OnRotateEvent onRotateEvent;

    protected virtual void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    protected virtual void Update()
    {
        if (onRotateEvent != null) onRotateEvent(_transform.rotation);

        //AimAt(_target.position);

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            GameObject newProjectile = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
            Rigidbody projRigidbody = newProjectile.GetComponent<Rigidbody>();
            projRigidbody.velocity = CalculateProjectileVelocity();
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public Vector3 CalculateProjectileVelocity() // Thanks, Sabastian
    {
        float gravity = Physics.gravity.y;
        float displacementY = _target.position.y - _shootPoint.position.y;
        Vector3 displancementXZ = new Vector3(_target.position.x - _shootPoint.position.x, 0, _target.position.z - _shootPoint.position.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * _vertDisplacement);
        Vector3 velocityZX = displancementXZ / (Mathf.Sqrt(-2 * _vertDisplacement / gravity) + Mathf.Sqrt(2 * (displacementY - _vertDisplacement) / gravity));
        return velocityZX + velocityY;
    }

    public void AimAt(Vector3 position)
    {
        Vector3 velocity = CalculateProjectileVelocity();

        _turretTrans.LookAt(_target);
        _turretTrans.rotation = Quaternion.Euler(0, _turretTrans.rotation.eulerAngles.y, 0);

        _barrelLookAtTarget = _shootPoint.position + (velocity * 0.05f);
        _barrelTrans.LookAt(_barrelLookAtTarget);
        _barrelTrans.rotation = Quaternion.Euler(_barrelTrans.rotation.eulerAngles.x, _turretTrans.rotation.eulerAngles.y, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        if (_navMeshAgent != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _navMeshAgent.path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(_navMeshAgent.path.corners[i], _navMeshAgent.path.corners[i + 1]);
                Gizmos.DrawSphere(_navMeshAgent.path.corners[i + 1], 0.05f);
            }
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_barrelTrans.position, _barrelLookAtTarget);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_turretTrans.position, _turretTrans.position + (_turretTrans.forward * 2));
    }
}
