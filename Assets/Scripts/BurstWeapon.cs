using UnityEngine;
using System.Collections;

public class BurstWeapon : Weapon
{
    // Normal burst: burstRate =1f; fireRate = .10f; fireDuration = .05f
    // Constant fire: burstRate =.10f; fireRate = .10f; fireDuration = .05f
    public float burstRate = 1f;// Time between bursts
    public int burstMagnitude = 3;// Bullets per burst
    protected int burstCount = 0;// Current burst count
    protected float burstCooldown = 0f;// Time of next burst
    protected bool bursting = false;// Currently firing a burst;
    protected int burstIdx = 0;// Burst index. For debug purposes, keep track of the current burst by giving it a number.

    // Called in the Update phase
    public override void UpdateFire()
    {
        if (bursting)
        {
            if (burstCount < burstMagnitude)
            {
                if (Time.time > fireCooldown)
                {
                    FireShot();
                    burstCount++;
                }

            }
            else
            {
                bursting = false;
                burstCooldown = Time.time + burstRate;
            }

        }

    }


    // Fire a burst
    public override void Fire()
    {
        if (!bursting && Time.time > burstCooldown)
        {
            StartBurst();
        }

    }

    public void StartBurst()
    {
        burstIdx++;
        burstCount = 0;
        bursting = true;
    }


}
