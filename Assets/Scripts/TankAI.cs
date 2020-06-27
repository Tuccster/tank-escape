using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : Tank
{
    //private IEnumerator _currentTask;
    private List<IEnumerator> _taskList = new List<IEnumerator>();
    private bool _taskListRunning = false;
    private bool _currentTaskRunning = false;

    public void AddTask(IEnumerator task)
    {
        if (_taskListRunning)
        {
            Debug.LogError("Cannot add tasks while task list is running.");
            return;
        }
        _taskList.Add(task);
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
                yield return new WaitForFixedUpdate();
            }
        }
        _taskListRunning = false;
    }

    // TASKS //

    public IEnumerator MoveTo(Vector3 position)
    {
        MoveToPosition(position);
        while(_navMeshAgent.remainingDistance < 0.25f)
        {
            yield return new WaitForFixedUpdate();
        }
        _currentTaskRunning = false;
    }
}
