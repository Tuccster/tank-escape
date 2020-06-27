using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDebug : TankAI
{
    protected override void Start()
    {
        base.Start();

        AddTask(MoveTo(new Vector3(0, 0, 20)));
        AddTask(MoveTo(new Vector3(0, 0, -20)));
        AddTask(MoveTo(new Vector3(0, 0, 20)));

        //AddTask(MoveToRandom(Vector3.zero, 8), 1000);

        StartTaskList();
    }
}
