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
        AddTask(taskList, MoveTo(_player.transform, 3), 1);

        StartCoroutine(AIBehaviour());
    }

    private IEnumerator AIBehaviour()
    {
        StartTaskList("move_to_player");
        while (true) 
        {
            if (PlayerWithinRadius(100) && PlayerUnobstructed(100))
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
