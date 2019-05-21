using UnityEngine;
using System.Collections;

public class Hull : MonoBehaviour
{

    public float hullBase = 10f;
    protected float hullCurr;
    protected Explosion explosion;
    // Use this for initialization
    void Start()
    {
        hullCurr = hullBase;
        explosion = GameObject.Find("Explosion").GetComponent<Explosion>();
    }


    public virtual void ApplyDamage(float D)
    {
        hullCurr -= D;
        if (hullCurr <= 0)
        {
            Explode();
        }

    }

    public virtual void Explode()
    {
        explosion.Explode(transform.position);
        Destroy(gameObject);
    }
}
