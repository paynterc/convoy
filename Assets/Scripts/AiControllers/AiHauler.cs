using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHauler : AiController
{
    private LevelController levelController;

    public override void Init()
    {
        base.Init();

        levelController = GameObject.Find("LevelControl").GetComponent<LevelController>();
    }

    public override void UpdateStep()
    {
        // Rotate
        if (target)
        {

            float dist = Vector3.Distance(target.position, transform.position);
            if (dist > 30f)
            {
                thruster.SetTargetDirection(target.position - transform.position);
                thruster.SetThrustSpeed(thruster.thrustSpeedBase);
                thruster.SetThrustV(1f);// Go
                thruster.BrakeOff();
            }
            else
            {
                thruster.SetThrustV(1f);
                thruster.SetThrustSpeed(thruster.thrustSpeedBase * 0.5f);// SLOW
                thruster.BrakeOff();
                levelController.ProcessArrival(gameObject, target.gameObject);

            }

        }
        else
        {
            EventManager.TriggerEvent("haulerLost");
        }
    }

    public void SetTarget(Transform T)
    {
        target = T;
    }

}
