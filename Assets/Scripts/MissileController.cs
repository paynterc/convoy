using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : Projectile
{
    public Transform target;
    private Vector3 targetDir;

    public string targetTag = "Hauler";
    public string targetTag2 = "Player";
    public float detonationRange = 5.0f;
    public float rotateSpeedBase = 1.0f;
    public float rotateSpeedCurr;

    public override void Init()
    {
        base.Init();
        rotateSpeedCurr = rotateSpeedBase;
        rb = gameObject.GetComponent<Rigidbody>();
        AssignTarget(targetTag);
        if (!target)
        {
            AssignTarget(targetTag);
        }
    }
    public override void UpdateActions()
    {
        //rb.AddRelativeForce(transform.forward * speed);
        if (Time.time > deadline)
        {
            DestroyMe();
        }
        else
        {

            // Rotate
            if (target && target.gameObject.activeSelf)
            {

                targetDir = target.position - transform.position;
                float dist = Vector3.Distance(target.position, transform.position);
                RotateWithLook();

            }
            else
            {
                
                if (!target)
                {
                    AssignTarget(targetTag);
                }
            }
            //rb.AddForce(new Vector3(0, 0, 1) * 10);
            rb.velocity = transform.forward * speed;

        }
    }

    public virtual void AssignTarget(string findTag)
    {
        //GameObject o = FindClosestTag(findTag);
        GameObject o = FindClosestInFront(findTag,45,200);
        if (o != null)
        {
            target = o.transform;
        }
        else
        {
            target = null;
        }
    }

    public virtual GameObject FindClosestTag(string mytag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(mytag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    protected GameObject FindClosestInFront(string mytag, float maxAngle, float maxDistance)
    {
        GameObject targetGO = null;
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag(mytag);

        float minDotProduct = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
        float targetDistanceSqrd = maxDistance * maxDistance;

        foreach (GameObject go in enemies)
        {
            Vector3 relativePosition = go.transform.position - transform.position;

            float distanceSqrd = relativePosition.sqrMagnitude;
            if ((distanceSqrd < targetDistanceSqrd) && (Vector3.Dot(relativePosition.normalized, transform.forward) > minDotProduct))
            {
                targetDistanceSqrd = distanceSqrd;
                targetGO = go;
            }
        }
        return targetGO;
    }

    public virtual void RotateWithLook()
    {
        // Rotate manually
        float step = rotateSpeedCurr * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);

    }
}
