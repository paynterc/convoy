using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHull : Hull
{
    public AudioSource audioSource;

    public override void Explode()
    {
        EventManager.TriggerEvent("playerDestroyed");
        explosion.Explode(transform.position);
        myController.IsDestroyed();
        audioScript.shipExplode();
        Destroy(gameObject);
        

    }

}
