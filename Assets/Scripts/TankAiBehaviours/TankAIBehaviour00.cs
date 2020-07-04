
// This AI is essentially just a turret.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIBehaviour00 : TankAI, IRobot
{
    [Header("AI")]
    public float _fireRate;
    public float _detectRadius;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(AIBehaviour());
    }

    public IEnumerator AIBehaviour()
    {
        while(true)
        {
            if (PlayerWithinRadius(_detectRadius) && PlayerUnobstructed(_detectRadius))
            {
                AimAt(_player.transform.position);
                ShootProjectile(_fireRate);
            }
            else
            {
                AimAt(transform.position + transform.forward);
            }
            yield return waitForEndOfFrame;
        }
    }
}
