using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDodger : AiDrifter
{
    public float avoidAngle = 5.0f;
    public float checkTargetTimer = 0.0f;

    // Check to see if the target is aiming at me
    public void TargetAimingAtMe()
    {
        if (Time.time>checkTargetTimer)
        {
            checkTargetTimer = Time.time + reactionTime;
            if (target)
            {
                float angle = Vector3.Angle(transform.position - target.position, target.transform.forward);
                if (angle <= avoidAngle)
                {
                    if (!reacting)
                    {
                        StartDodge();
                    }
                }
            }
        }


    }

    public override void Move()
    {

        TargetAimingAtMe();

        float dist = Vector3.Distance(target.position, transform.position);
        thruster.SetThrustY(0);
        ChangeDirRandom();
        if (dist <= avoidDistance)
        {
            // Strafe
            thruster.SetThrustH(strafeDir);
            thruster.SetThrustV(0);
        }
        else
        {
            thruster.SetThrustH(0);
            thruster.SetThrustV(1);
        }
    }

    public override void Reactions()
    {
        ReactDamage();
        Dodge();
        TargetAimingAtMe();

    }

    public virtual void StartDodge()
    {
        reacting = true;
        reactiontimer = Time.time + reactionTime;
    }

    private void Dodge()
    {
        if (reacting && Time.time > reactiontimer)
        {
            reacting = false;
            if (ChooseDirection() == 1)
            {
                thruster.SetThrustH(ChooseDirection());
            }
            else
            {
                thruster.SetThrustY(ChooseDirection());
            }

            thruster.SetThrustV(charger ? 1 : 0);
            thruster.StartBoost();
        }

    }

}
