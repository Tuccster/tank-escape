using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDebug : TankAI
{
    protected override void Start()
    {
        base.Start();

        AddTask(MoveToRandom(Vector3.zero, 16));

        StartTaskList(true);
    }
}
