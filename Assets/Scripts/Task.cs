using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public IEnumerator _task;

    public Task(IEnumerator task)
    {
        _task = task;
    }

    public void Start()
    {
        
    }
}
