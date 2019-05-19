using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulerController : MonoBehaviour
{

    private Thruster thruster;
    // The target marker.
    public Transform target;
    private LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 1;// Ignore layer
        thruster = GetComponent<Thruster>();
        thruster.SetThrustV(1f);// Go forward
        levelController = GameObject.Find("LevelControl").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate
        if (target)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist > 30f)
            {
                thruster.SetTargetDirection(target.position - transform.position);
                thruster.SetThrustV(1f);// Go
                thruster.BrakeOff();
            }
            else
            {
                thruster.SetThrustV(0f);// Stop
                thruster.BrakeOn();
                levelController.ProcessArrival(gameObject);
            }

        }
    }

    public void SetTarget(Transform T)
    {
        target = T;
    }


}
