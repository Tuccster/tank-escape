using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : Tank
{
    private List<IEnumerable> _taskList = new List<IEnumerable>();
    private bool _repeatTaskList = true;
    private bool _taskListRunning = false;
    private bool _currentTaskRunning = false;
    private TankPlayer _player;
    private WaitForEndOfFrame waitForEndOfFrame;

    protected virtual void Start()
    {
        _player = Registry.GetRegister<TankPlayer>("player");
        waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public void AddTask(IEnumerable task, int amount = 1)
    {
        if (_taskListRunning)
        {
            Debug.LogError("Cannot add tasks while task list is running.");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            _taskList.Add(task);
        }
    }

    public void StartTaskList(bool repeat)
    {
        if (_taskList.Count == 0)
        {
            Debug.LogWarning("Attempted to start task list while empty.");
            return;
        }
        _repeatTaskList = repeat;
        StartCoroutine(StartTaskListCoroutine());
    }

    private IEnumerator StartTaskListCoroutine()
    {
        _taskListRunning = true;
        bool repeat = true;
        while(repeat)
        {
            for (int i = 0; i < _taskList.Count; i++)
            {
                _currentTaskRunning = true;
                StartCoroutine(_taskList[i].GetEnumerator());
                while(_currentTaskRunning)
                    yield return waitForEndOfFrame;
            }
            repeat = _repeatTaskList;
        }
        _taskListRunning = false;
    }

    public IEnumerable MoveTo(Vector3 position)
    {
        MoveToPosition(position);
        yield return waitForEndOfFrame; // Needed so that remaining distance can be calculated

        while(Maths.RemainingDistance(_navMeshAgent.path.corners) >= 0.25f)
            yield return waitForEndOfFrame;
        _currentTaskRunning = false;
    }

    public IEnumerable MoveToRandom(Vector3 position, float radius)
    {
        Vector3 direction = position + (Random.insideUnitSphere * radius);
        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, radius, 1);
        MoveToPosition(hit.position);
        yield return waitForEndOfFrame; // Needed so that remaining distance can be calculated

        while(Maths.RemainingDistance(_navMeshAgent.path.corners) >= 0.25f)
            yield return waitForEndOfFrame;
        _currentTaskRunning = false;
    }
}
