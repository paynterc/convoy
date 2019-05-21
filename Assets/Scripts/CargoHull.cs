using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoHull : Hull
{
    public override void Explode()
    {
        EventManager.TriggerEvent("cargoUpdate");
        explosion.Explode(transform.position);
        Destroy(gameObject);
    }
}
