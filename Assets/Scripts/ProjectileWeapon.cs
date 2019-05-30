using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectile;
    public float bulletSpeed = 200f;
    public float bulletLife = 5.0f;
    public bool createPool = false;


    public override void FireShot()
    {

        fireCooldown = Time.time + fireRate;
        // Projectile

        GameObject bullet = Instantiate(projectile,transform.position + transform.forward*50.0f, Quaternion.identity) as GameObject;
        Projectile P = bullet.GetComponent<Projectile>();
        P.damage = damage;
        P.duration = bulletLife;
        P.pooled = createPool;
        P.speed = bulletSpeed;
        P.Init();
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;


    }


}
