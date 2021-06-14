using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : AbstractUnitController
{
    public float firingAngle = 5.0f;
    public float avoidDistance = 30.0f;
    protected RadarSystem _RadarSystem;
    public Color radarColor = Color.red;
    protected int bounty = 0;
    public int bountyLow = 0;
    public int bountyHigh = 0;

    public override void Init()
    {
        gameObject.layer = layer;
        thruster = GetComponent<Thruster>();
        thruster.SetThrustV(1f);// Go forward
        _RadarSystem = GameObject.Find("RadarSystem").GetComponent<RadarSystem>();
        _RadarSystem.AddRadarBlip(gameObject, radarColor, 0.5f);
        InitWeapons();
        bounty = Random.Range(bountyLow,bountyHigh);

    }

    public override void UpdateStep()
    {
        // Rotate
        if (target && target.gameObject.activeSelf)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist > avoidDistance)
            {
                // Avoids impact
                thruster.SetTargetDirection(target.position - transform.position);
            }

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
    }

    public virtual void InitWeapons()
    {
        if (autoDetectWeapons)
        {
            weapons = GetComponentsInChildren<Weapon>();
        }

        for (int i = 0; i < weapons.Length; i++)
        {

            weapons[i].origin = 1;
            // Bit shift the index of the layer (8) to get a bit mask. This would be 0000000100000000, with 1 starting all the way on the right and moving 8 steps to the left.
            // This number is the same as 256. 1<<9 would be 512. 1<<10 would be 1024.
            // For multiple layers  (1<<8) | (1<<10);
            if (layer == 8)
            {
                weapons[i].layerMask = 1 << 9;// Hit only this
            }
            else
            {
                weapons[i].layerMask = 1 << 8;// Hit only this
            }

        }

    }

    public override void IsDestroyed()
    {
        GameSingleton.Instance.creditsCurrent += bounty;
        GameSingleton.Instance.BountiesCollected += bounty;
        _RadarSystem.RemoveRadarBlip(gameObject);
    }
}
