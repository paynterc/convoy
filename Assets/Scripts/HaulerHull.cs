using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulerHull : Hull
{
    public override void Explode()
    {
        EventManager.TriggerEvent("haulerDestroyed");
        explosion.Explode(transform.position);
        Destroy(gameObject);
    }
}
