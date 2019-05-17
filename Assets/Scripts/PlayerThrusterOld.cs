using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrusterOld : MonoBehaviour
{
    Rigidbody rb;

    public Quaternion rotateTo;
    public ForceMode force = ForceMode.Force;

    // Movement
    public float moveHorizontal=0;
    public float moveVertical=0;
    public float speed = 5.0f;

    // Rotating
    public float rotateHorizontal = 0;
    public float rotateVertical = 0;
    public float rotateZ = 0;
    public float rotationSpeed = 1.0f;
    public bool rotationBrake = false;

    // Defaults
    public float defaultSpeed = 5.0f;
    public float lungeSpeed = 20.0f;
    public float defaultDrag = 0.25f;
    public float defaultAngularDrag = 2f;

    // Timers

    private float lungeRate = 2.0f;
    private float lungeCooldown = 0;
    private bool lunging = false;
    private int lungeTimer = 0;

    public Vector3 targetDir;

    // Start is called before the first frame update
    void Start()
    {
        speed = defaultSpeed;

        rb = GetComponent<Rigidbody>();
        rb.angularDrag = defaultAngularDrag;
        rb.drag = defaultDrag;
    }

    // Update is called once per frame
    void Update()
    {

        // Rotation with physics
        rb.AddRelativeTorque(rotationSpeed * rotateVertical, rotationSpeed * rotateHorizontal, rotationSpeed * rotateZ);

        // Rotate manually
        //float step = rotationSpeed * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        // Move our position a step closer to the target.
        //transform.rotation = Quaternion.LookRotation(newDir);
        //rb.AddRelativeTorque(0, 0, rotationSpeed * rotateZ);

        // Thrust
        rb.AddRelativeForce(new Vector3(moveHorizontal, 0.0f, moveVertical) * speed, force);

        //  Lunging for 1 frame
        if (lunging)
        {
            lungeTimer++;
            if (lungeTimer>0)
            {
                EndLunge();
            }
        }

        rb.angularDrag = defaultAngularDrag;
        rb.drag = defaultDrag;

    }

    public void Lunge()
    {
        if (Time.time > lungeCooldown && !lunging)
        {
            speed = lungeSpeed;
            force = ForceMode.Impulse;
            lungeCooldown = Time.time + lungeRate;
            lunging = true;
            lungeTimer = 0;
        }

    }

    private void EndLunge()
    {
        // Code to execute after the delay
        speed = defaultSpeed;
        force = ForceMode.Force;
        lunging = false;
        lungeTimer = 0;
    }
}
