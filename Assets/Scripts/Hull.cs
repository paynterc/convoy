using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Hull : MonoBehaviour
{

    public float hullBase = 10f;
    protected float hullCurr;
    protected Explosion explosion;
    private GameObject hitObject;
    private ParticleSystem hitPs;// particle effect for hits
    protected AbstractUnitController myController;
    private bool exploded = false;
    protected AudioScript audioScript;
    // Use this for initialization
    void Start()
    {
        hullCurr = hullBase;
        explosion = GameObject.Find("Explosion").GetComponent<Explosion>();
        hitObject = GameObject.Find("HitPulse");
        hitPs = hitObject.GetComponent<ParticleSystem>();
        myController = gameObject.GetComponent<AbstractUnitController>();
        audioScript = GameObject.Find("AudioController").GetComponent<AudioScript>();

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
        myController.IsDestroyed();
        audioScript.shipExplode();
        Destroy(gameObject);
    }

    public float GetHullCurrent()
    {
        return hullCurr;
    }

    public float GetHullPercentage()
    {
        return hullCurr/hullBase;
    }
}
