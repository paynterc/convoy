using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask layerMask = 1 << 8;// hit the player
    public float forcePower = 0.0f;// how much force to apply on impact
    public float damage;
    public bool destroyOnImpact = true;
    public bool pooled = true;// Is this a member of a bullet pool. If so, set inactive so it can be reused. Otherwise destroy after use.
    public float duration = 5.0f;// Life of the object
    public float speed = 20f;
    protected float deadline = 0.0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.layer = 10;// Bullet layer. Bullets should not collide with eachother.
        Init();
    }

    public virtual void Init()
    {

        deadline = Time.time + duration;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActions();
    }

    public virtual void UpdateActions()
    {
        //rb.AddRelativeForce(transform.forward * speed);
        if (Time.time>deadline)
        {
            DestroyMe();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyCollision(collision);
    }

    public virtual void ApplyCollision(Collision collision)
    {
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            Hull hull = collision.transform.GetComponent<Hull>();
            if (hull != null)
            {
                collision.transform.GetComponent<Hull>().ApplyDamage(damage, collision.transform.position);
            }
        }
        if (destroyOnImpact)
        {
            DestroyMe();
        }

    }

    public virtual void DestroyMe()
    {
        if (pooled)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
