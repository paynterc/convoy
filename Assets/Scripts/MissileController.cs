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

                Debug.Log("TARGET GOOD");
                targetDir = target.position - transform.position;
                float dist = Vector3.Distance(target.position, transform.position);
                RotateWithLook();

            }
            else
            {
                AssignTarget(targetTag);
                if (!target)
                {
                    AssignTarget(targetTag2);
                }
            }
            //rb.AddForce(new Vector3(0, 0, 1) * 10);
            rb.velocity = transform.forward * speed;

        }
    }

    public virtual void AssignTarget(string findTag)
    {
        GameObject o = FindClosestTag(findTag);
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

    public virtual void RotateWithLook()
    {
        // Rotate manually
        Debug.Log("ROTATE GOOD");
        float step = rotateSpeedCurr * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);

    }
}
