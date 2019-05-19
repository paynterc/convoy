using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 50.0F;
    public float power = 1500.0F;
    public GameObject exploder;
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {

        ps = exploder.GetComponent<ParticleSystem>();
        ps.Stop();


    }

    public void Explode(Vector3 explosionPos)
    {

        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius);
                exploder.transform.position = explosionPos;
                ps.Play();
            }

        }
    }

}
