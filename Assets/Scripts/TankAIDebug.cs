using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDebug : TankAI, IRobot
{
    protected override void Start()
    {
        base.Start();

        _navMeshAgent.speed = 2.2f;

        TaskList taskList = CreateTaskList("move_to_player", true);
        //AddTask(taskList, MoveTo(_player.transform.position, 0.25f), 1);
        AddTask(taskList, MoveTo(_player.transform, 1), 1);
        //AddTask(taskList, MoveToRandom(Vector3.zero, 4));

        StartCoroutine(AIBehaviour());
    }

    private IEnumerator AIBehaviour()
    {
        //StartCoroutine(MoveTo(_player.transform.position).GetEnumerator());
        StartTaskList("move_to_player");
        while (true) 
        {
            if (PlayerWithinRadius(8) && PlayerUnobstructed(8))
            {
                AimAt(_player.transform.position);
                ShootProjectile(0.5f);
            }
            else
            {
                AimAt(transform.position + transform.forward);
            }
            yield return waitForEndOfFrame;
        }

    }
}
