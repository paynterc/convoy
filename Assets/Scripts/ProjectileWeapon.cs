using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : BurstWeapon
{
    public GameObject projectile;
    public float bulletSpeed = 200f;
    public float bulletLife = 5.0f;
    public bool createPool = false;
    public float createForward = 10.0f;// Instantiate bullets a certain amount forward to avoid self-collision
    private Pool pool;
    private GameObject bullet;

    public override void Init()
    {
        if (createPool)
        {
            pool = new Pool(projectile,20);
        }
    }

    public override void FireShot()
    {

        fireCooldown = Time.time + fireRate;
        // Projectile

        if (createPool)
        {
            bullet = pool.GetPooledObject();
            bullet.transform.position = transform.position + transform.forward * createForward;
            bullet.transform.rotation = Quaternion.identity;
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
        P.Init();
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;


    }


}
