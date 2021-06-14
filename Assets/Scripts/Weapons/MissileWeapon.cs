using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : ProjectileWeapon
{
    public string targetTag = "Hauler";
    public string targetTag2 = "Player";

    public override void Init()
    {
        if (origin==0)
        {
            // This is a player missile weapon. Ammo should be the Global missile count
            ammo = GameSingleton.Instance.Missiles;
        }

        if (GameObject.Find("PlayerCamera"))
        {
            playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        }

        if (createPool)
        {
            pool = new Pool(projectile, poolSize);

        }
    }


    public override void FireShot()
    {
        fireCooldown = Time.time + fireRate;
        // Projectile

        if (ammo > 0 || ammo==-1)
        {
            ammo--;
            if (origin==0)
            {

                //Player missile. Update UI
                GameSingleton.Instance.Missiles = ammo;
                EventManager.TriggerEvent("missileFired");
            }
            
            if (createPool)
            {
                bullet = pool.GetPooledObject();
                bullet.transform.position = transform.position + transform.forward * createForward;
                bullet.transform.rotation = transform.rotation;
            }
            else
            {
                bullet = Instantiate(projectile, transform.position + transform.forward * createForward, Quaternion.identity) as GameObject;
            }

            Projectile P = bullet.GetComponent<Projectile>();
            P.damage = damage;
            P.duration = bulletLife;
            P.pooled = createPool;
            P.speed = bulletSpeed;
            P.layerMask = layerMask;
            P.Init();

            // Missile stuff
            MissileController missile = P.GetComponent<MissileController>();
            if (missile != null)
            {
                missile.targetTag = targetTag;
                missile.targetTag2 = targetTag2;
            }

            bullet.SetActive(true);
            if (bulletSpeed > 0)
            {
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            }

        }
        else
        {
            // out of missiles. error sound.
        }



        



    }
}
