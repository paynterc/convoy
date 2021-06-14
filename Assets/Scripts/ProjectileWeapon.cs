using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : BurstWeapon
{
    public GameObject projectile;
    public float bulletSpeed = 200f;
    public float bulletLife = 5.0f;
    public bool createPool = false;
    public float createForward = 5.0f;// Instantiate bullets a certain amount forward to avoid self-collision
    protected Pool pool;
    protected  GameObject bullet;
    protected int poolSize = 20;

    public override void Init()
    {
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
        bullet.SetActive(true);
        if (bulletSpeed>0)
        {

            if (origin == 0)
            {
                //bullet.GetComponent<Rigidbody>().velocity = playercamera.transform.forward * bulletSpeed;

                Vector3 aimSpot = playercamera.transform.position;
                //You will want to play around with the 50 to make it feel accurate.
                aimSpot += playercamera.transform.forward * range;
                transform.LookAt(aimSpot);

            }

            bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        }

        if (audioSource)
        {
            audioSource.PlayOneShot(weaponSound);

        }

    }


}
