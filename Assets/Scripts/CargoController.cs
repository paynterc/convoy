using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoController : AiController
{

    protected bool attached; // Attached to hauler or not
    //Rigidbody rb;

    public override void Init()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        attached = true;
        gameObject.layer = 1;
        thruster = GetComponent<Thruster>();
        thruster.SetThrustV(1f);// Go forward  

    }

    public override void UpdateStep()
    {
        // Rotate
        if (target && target.gameObject.activeSelf)
        {

            float dist = Vector3.Distance(target.position, transform.position);
            if (dist <= avoidDistance)
            {
                // Avoids impact
                thruster.thrustSpeedCurr = target.GetComponent<Thruster>().thrustSpeedBase;
            }
            else
            {
                thruster.thrustSpeedCurr = target.GetComponent<Thruster>().thrustSpeedBase * 2;
            }
            thruster.SetTargetDirection(target.position - transform.position);
        }

        if (target == null)
        {
            thruster.thrustSpeedCurr = 0f;
            attached = false;
        }
    }

}
