using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDrifter : AiController
{


    public float retargetRate = 15f;
    private float retargetTimer = 0;// Keep track of time to next boost.
    private bool doRetarget = false;
    public float changeDirRate = 3.0f;
    private float changeDirTimer = 0.0f;
    protected float strafeDir = 1;
    public float reactionTime = 0.5f;
    public float reactiontimer = 0.0f;
    protected bool reacting = false;
    public bool charger = false;// Charge at target on damage



    public override void UpdateStep()
    {

        // Rotate
        if (target && target.gameObject.activeSelf)
        {


            if (!reacting)
            {
                Move();
            }
            else
            {
                Reactions();
            }


            // Always point to target
            thruster.SetTargetDirection(target.position - transform.position);

            float angle = Vector3.Angle(target.position - transform.position, transform.forward);
            if (angle < firingAngle)
            {
                FireWeapon();
            }

        }
        else
        {
            AssignTarget(targetTag);
            if (!target)
            {
                AssignTarget(targetTag2);
            }
        }
        if (target == null)
        {
            thruster.thrustSpeedCurr = 0f;
        }
        Retarget();
    }

    public virtual void Move()
    {
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

    public virtual void Reactions()
    {
        ReactDamage();
    }

    private void StartDirectionTimer()
    {
        changeDirTimer = Time.time+changeDirRate;
    }

    public virtual void ChangeDirRandom()
    {
        if (Time.time>changeDirTimer)
        {
            StartDirectionTimer();
            strafeDir = ChooseDirection();

        }

    }

    public override void TakingDamage()
    {
        base.TakingDamage();
        if (!reacting)
        {
            StartReactDamage();
        }

    }

    public virtual void StartReactDamage()
    {
        reacting = true;
        reactiontimer = Time.time + reactionTime;
    }

    public virtual void ReactDamage()
    {
        if (reacting && Time.time > reactiontimer)
        {
            reacting = false;
            AssignTarget("Player");
            doRetarget = true;
            retargetTimer = Time.time + retargetRate;
            if (ChooseDirection()==1)
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

    public virtual float ChooseDirection()
    {

        //return Random.Range(-1.0f,1.0f);
        int roll = Random.Range(1, 3);
        return roll == 1 ? -1 : 1;
    }

    public virtual void Retarget()
    {
        if (doRetarget && Time.time > retargetTimer)
        {
            doRetarget = false;
            // Go back to cargo
            AssignTarget("Hauler");
        }
    }
}
