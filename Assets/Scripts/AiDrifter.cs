using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDrifter : AiController
{


    public float retargetRate = 15f;
    private float retargetTimer = 0;// Keep track of time to next boost.
    private bool doRetarget = false;

    public override void UpdateStep()
    {
        // Rotate
        if (target && target.gameObject.activeSelf)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist <= 30f)
            {
                // Strafe
                thruster.SetThrustH(ChooseDirection());
                thruster.SetThrustV(0);
            }
            else
            {
                thruster.SetThrustH(0);
                thruster.SetThrustV(1);
            }

            // Always point to target
            thruster.SetTargetDirection(target.position - transform.position);

            float angle = Vector3.Angle(target.position - transform.position, transform.forward);
            if (angle < 5.0f)
            {
                FireWeapon();
            }

        }
        else
        {
            AssignTarget(targetTag);
        }
        if (target == null)
        {
            thruster.thrustSpeedCurr = 0f;
        }
        Retarget();
    }

    public override void TakingDamage()
    {
        base.TakingDamage();
        AssignTarget("Player");
        doRetarget = true;
        retargetTimer = Time.time + retargetRate;
        thruster.SetThrustH(ChooseDirection());
        thruster.SetThrustV(0);
        thruster.StartBoost();
    }

    private float ChooseDirection()
    {

        //return Random.Range(-1.0f,1.0f);
        int roll = Random.Range(1, 2);
        return roll == 1 ? -1 : 1;
    }

    private void Retarget()
    {
        if (doRetarget && Time.time > retargetTimer)
        {
            doRetarget = false;
            // Go back to cargo
            AssignTarget("Cargo");
        }
    }
}
