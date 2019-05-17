﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyThruster thruster;
    // The target marker.
    public Transform target;

    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 9;
        thruster = GetComponent<EnemyThruster>();
        target = GameObject.Find("PlayerShip").transform;
        thruster.SetThrustV(1f);// Go forward
        weapon = GetComponent<Weapon>();
        weapon.origin = 1;
        // Bit shift the index of the layer (8) to get a bit mask. This would be 0000000100000000, with 1 starting all the way on the right and moving 8 steps to the left.
        // This number is the same as 256. 1<<9 would be 512. 1<<10 would be 1024.
        // For multiple layers  (1<<8) | (1<<10);
        weapon.hitLayer = 1 << 8;// Ignore all but 8
    }

    // Update is called once per frame
    void Update()
    {

        // Rotate
        if (target)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if(dist>30f)
            {
                thruster.SetTargetDirection(target.position - transform.position);
            }

            float angle = Vector3.Angle(target.position - transform.position, transform.forward);
            if (angle < 5.0f)
            {
                FireWeapon();
            }
        }


    }


    public void FireWeapon()
    {
        weapon.Fire();// Fire a burst
    }
}
