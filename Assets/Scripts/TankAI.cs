using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : Tank
{
    public Dictionary<string, TaskList> _taskListsDict = new Dictionary<string, TaskList>();
    public TaskList _currentTaskList = null;

    public class TaskList
    {
        public List<IEnumerable> taskList = new List<IEnumerable>();
        public bool taskListRunning = false;
        public bool currentTaskRunning = false;
        public bool repeat = false;

        public TaskList(bool repeatTaskList)
        {
            repeat = repeatTaskList;
        }
    }

    protected TankPlayer _player;
    protected WaitForEndOfFrame waitForEndOfFrame;

    protected virtual void Start()
    {
        _player = Registry.GetRegister<TankPlayer>("player");
        waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public TaskList CreateTaskList(string key, bool repeat)
    {
        if (_taskListsDict.ContainsKey(key)) return null;
        _taskListsDict.Add(key, new TaskList(repeat));
        return _taskListsDict[key];
    }

    public void AddTask(string taskListKey, IEnumerable task, int amount = 1)
    {
        if (!_taskListsDict.ContainsKey(taskListKey)) return;

        if (_taskListsDict[taskListKey].taskListRunning)
        {
            Debug.LogError("Cannot add tasks while task list is running.");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            _taskListsDict[taskListKey].taskList.Add(task);
        }
    }

    public void AddTask(TaskList taskList, IEnumerable task, int amount = 1)
    {
        if (taskList.taskListRunning)
        {
            Debug.LogError("Cannot add tasks while task list is running.");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            taskList.taskList.Add(task);
        }
    }

    public void StartTaskList(string taskListKey)
    {
        if (_taskListsDict.ContainsKey(taskListKey))
            _currentTaskList = _taskListsDict[taskListKey];
        if (_currentTaskList.taskList.Count == 0)
        {
            Debug.LogWarning("Attempted to start task list while empty.");
            return;
        }
        StartCoroutine(StartTaskListCoroutine());
    }

    private IEnumerator StartTaskListCoroutine()
    {
        _currentTaskList.currentTaskRunning = true;
        bool repeat = true;
        while(repeat)
        {
            for (int i = 0; i < _currentTaskList.taskList.Count; i++)
            {
                _currentTaskList.currentTaskRunning = true;
                StartCoroutine(_currentTaskList.taskList[i].GetEnumerator());
                while(_currentTaskList.currentTaskRunning)
                    yield return waitForEndOfFrame;
            }
            repeat = _currentTaskList.repeat;
        }
        _currentTaskList.currentTaskRunning = false;
    }

    // TASKS //

    public IEnumerable MoveTo(Vector3 position, float stopWithin)
    {
        if (Maths.RemainingDistance(_navMeshAgent.path.corners) < stopWithin) yield break;
        MoveToPosition(position);
        yield return waitForEndOfFrame; // Needed so that remaining distance can be calculated

        while(Maths.RemainingDistance(_navMeshAgent.path.corners) >= stopWithin)
            yield return waitForEndOfFrame;
        _currentTaskList.currentTaskRunning = false;
    }

    public IEnumerable MoveTo(Transform trans, float stopWithin)
    {
        yield return waitForEndOfFrame; // Needed so that remaining distance can be calculated

        while(Vector3.Distance(transform.position, trans.position) >= stopWithin)
        {
            yield return waitForEndOfFrame;
            MoveToPosition(trans.position);
        }

        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        _currentTaskList.currentTaskRunning = false;
    }

    public IEnumerable MoveToRandom(Vector3 position, float radius, float stopWithin)
    {
        Vector3 direction = position + (Random.insideUnitSphere * radius);
        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, radius, 1);
        MoveToPosition(hit.position);
        yield return waitForEndOfFrame; // Needed so that remaining distance can be calculated

        while(Maths.RemainingDistance(_navMeshAgent.path.corners) >= stopWithin)
            yield return waitForEndOfFrame;
        _currentTaskList.currentTaskRunning = false;
    }

    // CONDITIONS //

    public bool PlayerWithinRadius(float radius)
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= radius)
            return true;
        return false;
    }

    public bool PlayerUnobstructed(float range)
    {
        if (Physics.Raycast(transform.position, transform.position - _player.transform.position, range))
            return true;
        return false;
    }
}
