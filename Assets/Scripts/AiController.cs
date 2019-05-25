using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : AbstractUnitController
{
    public override void Init()
    {
        gameObject.layer = layer;

        thruster = GetComponent<Thruster>();
        thruster.SetThrustV(1f);// Go forward        

        weapon = GetComponent<Weapon>();
        weapon.origin = 1;
        // Bit shift the index of the layer (8) to get a bit mask. This would be 0000000100000000, with 1 starting all the way on the right and moving 8 steps to the left.
        // This number is the same as 256. 1<<9 would be 512. 1<<10 would be 1024.
        // For multiple layers  (1<<8) | (1<<10);
        if (layer == 8)
        {
            weapon.layerMask = 1 << 9;// Hit only this
        }
        else
        {
            weapon.layerMask = 1 << 8;// Hit only this
        }
    }

    public override void UpdateStep()
    {
        // Rotate
        if (target && target.gameObject.activeSelf)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist > 30f)
            {
                // Avoids impact
                thruster.SetTargetDirection(target.position - transform.position);
            }

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
    }
}
