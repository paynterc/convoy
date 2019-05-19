using UnityEngine;
using System.Collections;

public class Hull : MonoBehaviour
{

    public float hullBase = 10f;
    private float hullCurr;
    private Explosion explosion;
    // Use this for initialization
    void Start()
    {
        hullCurr = hullBase;
        explosion = GameObject.Find("Explosion").GetComponent<Explosion>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyDamage(float D)
    {
        hullCurr -= D;
        if (hullCurr <= 0)
        {
            Explode();
        }

    }

    public void Explode()
    {
        explosion.Explode(transform.position);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
