using UnityEngine;
using System.Collections;

public class Hull : MonoBehaviour
{

    public float hullBase = 10f;
    private float hullCurr;

    // Use this for initialization
    void Start()
    {
        hullCurr = hullBase;
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
        gameObject.SetActive(false);
    }
}
