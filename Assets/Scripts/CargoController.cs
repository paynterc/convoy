using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoController : MonoBehaviour
{


    protected Hull hull;
    protected bool attached; // Attached to hauler or not
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        hull = GetComponent<Hull>();
        gameObject.layer = 8;

    }

    // Update is called once per frame
    void Update()
    {
        CheckHull();
    }

    // Check status of hull.
    protected void CheckHull()
    {
    
    }

}
