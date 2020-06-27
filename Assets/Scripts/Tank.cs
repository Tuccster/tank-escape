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
    protected float _vertDisplacement = 0.25f;

    private Vector3 _targetPosition;
    public float _aimDistMin, _aimDistMax;

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

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0f) _vertDisplacement += 0.25f;
        if (scrollValue < 0f) _vertDisplacement -= 0.25f;
        _vertDisplacement = Mathf.Clamp(_vertDisplacement, 0, 5f);

        if (Input.GetKeyDown(KeyCode.F)) ShootProjectile();
    }

    public void MoveToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public void AimAt(Vector3 position)
    {
        _targetPosition = position;
        Vector3 velocity = Maths.CalculateProjectileVelocity(_shootPoint.position, _targetPosition, _vertDisplacement);

        _turretTrans.LookAt(_targetPosition);
        _turretTrans.rotation = Quaternion.Euler(0, _turretTrans.rotation.eulerAngles.y, 0);

        _barrelLookAtTarget = _shootPoint.position + (velocity * 0.05f);
        _barrelTrans.LookAt(_barrelLookAtTarget);
        _barrelTrans.rotation = Quaternion.Euler(_barrelTrans.rotation.eulerAngles.x, _turretTrans.rotation.eulerAngles.y, 0);
    }

    public void ShootProjectile()
    {
        float distToPos = Vector3.Distance(_transform.position, _targetPosition);
        if (distToPos < _aimDistMin || distToPos > _aimDistMax) return;

        GameObject newProjectile = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
        Rigidbody projRigidbody = newProjectile.GetComponent<Rigidbody>();
        projRigidbody.velocity = Maths.CalculateProjectileVelocity(_shootPoint.position, _targetPosition, _vertDisplacement);
    }

    protected virtual void OnDrawGizmos()
    {
        if (!GameRules._drawOnDrawGizmos) return;

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
        Gizmos.DrawLine(_turretTrans.position, _turretTrans.position + _turretTrans.forward);

        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(_transform.position, _aimDistMin);
        Gizmos.DrawWireSphere(_transform.position, _aimDistMax);
    }
}
