using UnityEngine;
using System.Collections;

public class Hull : MonoBehaviour
{

    public float hullBase = 10f;
    protected float hullCurr;
    protected Explosion explosion;
    private GameObject hitObject;
    private ParticleSystem hitPs;// particle effect for hits
    private AbstractUnitController myController;
    private bool exploded = false;
    // Use this for initialization
    void Start()
    {
        hullCurr = hullBase;
        explosion = GameObject.Find("Explosion").GetComponent<Explosion>();
        hitObject = GameObject.Find("HitPulse");
        hitPs = hitObject.GetComponent<ParticleSystem>();
        myController = gameObject.GetComponent<AbstractUnitController>();
    }


    public virtual void ApplyDamage(float D, Vector3 hitpoint)
    {
        hullCurr -= D;
        if (hullCurr <= 0)
        {
            if (!exploded)
            {
                exploded = true;
                Explode();
            }

        }
        else
        {
            hitObject.transform.position = hitpoint;
            hitPs.Play();
        }
        if (myController)
        {
            myController.TakingDamage();
        }

    }

    public virtual void Explode()
    {
        explosion.Explode(transform.position);
        Destroy(gameObject);
    }

    public float GetHullCurrent()
    {
        return hullCurr;
    }
}
