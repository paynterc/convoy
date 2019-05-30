using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : AiController
{
    public float rotateSpeedBase = 1f;
    public float rotateSpeedCurr = 1f;
    public bool xClamp = false;
    public float xMin = 0;
    public float xMax = 1;
    public bool yClamp = false;
    public float yMin = 0;
    public float yMax = 1;
    public bool showDir = false;

    public override void Init()
    {
        gameObject.layer = layer;
           
        rotateSpeedCurr = rotateSpeedBase;
        InitWeapons();
    }

    public override void UpdateStep()
    {

        if (target && target.gameObject.activeSelf)
        {
            // Rotate
            RotateWithLock();

            float angle = Vector3.Angle(target.position - transform.position, transform.forward);
            if (angle < firingAngle)
            {
                FireWeapon();
            }
        }
        else
        {
            AssignTarget(targetTag);
        }

    }

    public virtual void RotateWithLock()
    {
;
        if (target)
        {
            // Rotate manually
            Vector3 targetDir = target.position - transform.position;

            float step = rotateSpeedCurr * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            if (yClamp)
            {
                newDir.y = Mathf.Clamp(newDir.y, yMin, yMax);
            }
            if (xClamp)
            {
                newDir.x = Mathf.Clamp(newDir.x, xMin, xMax);
            }
            if (showDir)
            {
                Debug.Log(newDir);
            }
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        //https://www.reddit.com/r/Unity3D/comments/5ib7u9/turret_aiming_that_works_when_parent_is_rotated/
        //this.azimuthTransform.rotation = Quaternion.Euler(0f, Mathf.MoveTowardsAngle(curAz, targetAzimuth, velocity), 0f);
        //this.elevationTransform.localRotation = Quaternion.Euler(Mathf.MoveTowardsAngle(curEl, targetElevation, velocity), 0f, 0f);

    }
}
