using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : Tank
{
    //private IEnumerator _currentTask;
    private List<IEnumerator> _taskList = new List<IEnumerator>();
    private bool _taskListRunning = false;
    private bool _currentTaskRunning = false;
    private TankPlayer _player;

    protected virtual void Start()
    {
        _player = Registry.GetRegister<TankPlayer>("player");
    }

    public void AddTask(IEnumerator task, int amount = 1)
    {
        if (_taskListRunning)
        {
            Debug.LogError("Cannot add tasks while task list is running.");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            _taskList.Add(task);
            Debug.Log(task);
        }
    }

    public void StartTaskList()
    {
        if (_taskList.Count == 0)
        {
            Debug.LogWarning("Attempted to start task list while empty.");
            return;
        }
        StartCoroutine(StartTaskListCoroutine());
    }

    private IEnumerator StartTaskListCoroutine()
    {
        _taskListRunning = true;
        for (int i = 0; i < _taskList.Count; i++)
        {
            StartCoroutine(_taskList[i]);
            _currentTaskRunning = true;

            while(_currentTaskRunning)
            {
                yield return new WaitForEndOfFrame();
            }
            StopCoroutine(_taskList[i]);
        }
        _taskListRunning = false;
    }

    // TASKS //

    public IEnumerator MoveTo(Vector3 position)
    {
        MoveToPosition(position);
        yield return new WaitForEndOfFrame(); // Needed so that remaining distance can be calculated

        while(Maths.RemainingDistance(_navMeshAgent.path.corners) >= 0.25f)
        {
            yield return new WaitForEndOfFrame();
        }
        _currentTaskRunning = false;
    }

    public IEnumerator MoveToRandom(Vector3 position, float radius)
    {
        Vector3 direction = position + (Random.insideUnitSphere * radius);
        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, radius, 1);
        MoveToPosition(hit.position);
        yield return new WaitForEndOfFrame(); // Needed so that remaining distance can be calculated

        while(Maths.RemainingDistance(_navMeshAgent.path.corners) >= 0.25f)
        {
            yield return new WaitForEndOfFrame();
        }
        _currentTaskRunning = false;
    }
}
