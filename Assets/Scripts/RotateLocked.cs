using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLocked : MonoBehaviour
{


    public float rotateSpeedCurr = 1f;
    // The target marker.
    public Transform target;
    public bool xClamp = false;
    public float xMin=0;
    public float xMax=1;
    public bool yClamp = false;
    public float yMin=0;
    public float yMax=1;
    public bool showDir=false;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RotateWithLock();
    }

    public virtual void RotateWithLock()
    {
        Debug.Log(transform.localEulerAngles);
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
            if (showDir) {
                Debug.Log(newDir);
            }
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        //https://www.reddit.com/r/Unity3D/comments/5ib7u9/turret_aiming_that_works_when_parent_is_rotated/
        //this.azimuthTransform.rotation = Quaternion.Euler(0f, Mathf.MoveTowardsAngle(curAz, targetAzimuth, velocity), 0f);
        //this.elevationTransform.localRotation = Quaternion.Euler(Mathf.MoveTowardsAngle(curEl, targetElevation, velocity), 0f, 0f);

    }

    public float ClampAngle(float angle, float min, float max){
 
        if (angle<90 || angle>270){       // if angle in the critic region...
            if (angle>180) angle -= 360;  // convert all angles to -180..+180
            if (max>180) max -= 360;
            if (min>180) min -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);
        if (angle<0) angle += 360;  // if angle negative, convert to 0..360

        return angle;
    }

    public void ReportSpot(Transform target)
    {

        Vector3 direction = transform.InverseTransformDirection((transform.position - target.position).normalized);

        float azimuth  = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Debug.Log("Enemy " + target.gameObject.name + "at azimuth " + (Mathf.RoundToInt(azimuth) + 180) + "!");
    }
}
