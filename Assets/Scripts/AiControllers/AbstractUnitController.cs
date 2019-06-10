using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractUnitController : MonoBehaviour
{

    public Thruster thruster; 
    public Weapon[] weapons;
    public bool autoDetectWeapons = true;
    public Transform target;
    public int layer = 9;// layer of this object. 8 for allies. 9 for enemies
    public string targetTag = "Hauler";
    public string targetTag2 = "Player";


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStep();
    }

    public virtual void Init()
    {
        gameObject.layer = layer;

    }

    public virtual void UpdateStep() 
    {
    
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

    public virtual void FireWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i])
            {
                weapons[i].Fire();// Fire a burst
            }

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

    public virtual void TakingDamage()
    {
        // Apply logic when taking damage
    }

    public virtual void IsDestroyed()
    {

    }
}