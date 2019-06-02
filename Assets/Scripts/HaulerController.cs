using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulerController : MonoBehaviour
{

    private Thruster thruster;
    public Transform target;
    private LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 8;// Ignore layer
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
                thruster.SetThrustSpeed(thruster.thrustSpeedBase);
                thruster.SetThrustV(1f);// Go
                thruster.BrakeOff();
            }
            else
            {
                thruster.SetThrustV(1f);
                thruster.SetThrustSpeed(thruster.thrustSpeedBase * 0.5f);// SLOW
                thruster.BrakeOff();
                levelController.ProcessArrival(gameObject,target.gameObject);

            }

        }
        else
        {
            EventManager.TriggerEvent("haulerLost");
        }
    }

    public void SetTarget(Transform T)
    {
        target = T;
    }


}
