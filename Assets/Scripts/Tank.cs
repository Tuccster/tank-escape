using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Tank : MonoBehaviour, IKillable
{
    public GameObject _projectile;

    [SerializeField]
    protected Transform _bodyTrans, _turretTrans, _barrelTrans, _shootPoint;

    protected NavMeshAgent _navMeshAgent;
    protected LineRenderer _lineRenderer;

    private Vector3 _barrelLookAtTarget;
    private float _vertDisplacement = 0.0005f;
    private float _vertDisplacementMax = 1;

    private Vector3 _targetPosition;
    public float _aimDistMin, _aimDistMax;

    private WaitForSeconds _fireRateWaitForSeconds;
    private Coroutine _cooldownCoroutine;
    private bool _fireCooldown = false;

    public delegate void OnRotateEvent(Quaternion rotation);
    public event OnRotateEvent onRotateEvent;

    protected virtual void Awake()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _lineRenderer = gameObject.GetComponent<LineRenderer>();

        if (_aimDistMin == 0 || _aimDistMax == 0)
            Debug.LogWarning("_aimDistMin or _aimDistMax is 0");
    }

    protected virtual void Update()
    {
        if (onRotateEvent != null) onRotateEvent(transform.rotation);
    }

    public void MoveToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public void AimAt(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        direction.Normalize();
        float percentage = Vector3.Distance(transform.position + (direction * _aimDistMin), position) / _aimDistMax;
        _vertDisplacement = Mathf.Clamp(percentage * _vertDisplacementMax, 0, _vertDisplacementMax);

        _targetPosition = position;
        Vector3 velocity = Maths.CalculateProjectileVelocity(_shootPoint.position, _targetPosition, _vertDisplacement);

        _turretTrans.LookAt(_targetPosition);
        _turretTrans.rotation = Quaternion.Euler(0, _turretTrans.rotation.eulerAngles.y, 0);

        _barrelLookAtTarget = _shootPoint.position + (velocity * 0.01f);
        _barrelTrans.LookAt(_barrelLookAtTarget);
        _barrelTrans.rotation = Quaternion.Euler(_barrelTrans.rotation.eulerAngles.x, _turretTrans.rotation.eulerAngles.y, 0);
    }

    public void ShootProjectile(float fireRate)
    {
        if (!_fireCooldown)
        {
            _fireCooldown = true;
            StartCoroutine(ShootProjectileCooldown(fireRate));
        }
    }

    private IEnumerator ShootProjectileCooldown(float fireRate)
    {
        float distToPos = Vector3.Distance(transform.position, _targetPosition);
        if (distToPos < _aimDistMin || distToPos > _aimDistMax) yield break;

        GameObject newProjectile = Instantiate(_projectile, _shootPoint.position, Quaternion.identity);
        Rigidbody projRigidbody = newProjectile.GetComponent<Rigidbody>();
        projRigidbody.velocity = Maths.CalculateProjectileVelocity(_shootPoint.position, _targetPosition, _vertDisplacement);

        yield return new WaitForSeconds(fireRate);
        _fireCooldown = false;
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
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
        Gizmos.DrawWireSphere(transform.position, _aimDistMin);
        Gizmos.DrawWireSphere(transform.position, _aimDistMax);
    }
}
