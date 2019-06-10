using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulerHull : Hull
{
    public Object cargoPrefab;

    public override void Explode()
    {

        EventManager.TriggerEvent("haulerDestroyed");

        Instantiate(cargoPrefab, transform.position, Quaternion.identity);
        explosion.Explode(transform.position);
        Destroy(gameObject);
    }


}

