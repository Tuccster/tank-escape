using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tank : MonoBehaviour
{
    [SerializeField]
    protected Transform _bodyTrans, _turretTrans, _barrelTrans;
    protected Transform _transform;

    protected NavMeshAgent _navMeshAgent;

    public delegate void OnRotateEvent(Quaternion rotation);
    public static event OnRotateEvent onRotateEvent;

    protected virtual void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (onRotateEvent != null) onRotateEvent(_transform.rotation);
    }

    public void MoveToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
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
    }
}
