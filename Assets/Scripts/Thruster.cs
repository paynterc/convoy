using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    Rigidbody rb;

    // Movement
    private float thrustHorizontal=0;// 1 or 0
    private float thrustVertical=0;// 1 or 0
    private float thrustSpeedBase = 5.0f;
    public float thrustSpeedCurr;
    public float thrustDragBase = 0.25f;
    private float thrustDragCurr;
    private float thrustBoostBase = 30.0f;
    public float thrustBoostCurr;
    private float thrustBoostFrames = 1;
    private ForceMode forceModeBase = ForceMode.Force;
    private ForceMode forceModeCurr;

    // Rotating
    private float rotateH = 0;
    private float rotateV = 0;
    private float rotateZ = 0;
    public float rotateSpeedBase = 1.0f;
    private float rotateSpeedCurr;
    public float rotateDragBase = 2.0f;
    private float rotateDragCurr;
    private bool rotationBrake = false;

    // Timers
    private float boostRateBase = 2.0f;// Time between boosts
    private float boostRateCurr;
    private float boostCooldownTimer = 0;// Keep track of time to next boost.
    private bool boosting = false;// Boosting or not
    private int boostTimer = 0;// Keep track of time for current boost

    // Used for manual rotation
    public Vector3 targetDir;
    public Quaternion rotateTo;

    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

    // Update is called once per frame
    void Update()
    {

        Rotate();

        Thrust();

        Boost();

        Drag();

    }

    public virtual void Init()
    {
        thrustSpeedCurr = thrustSpeedBase;
        rotateSpeedCurr = rotateSpeedBase;
        thrustDragCurr = thrustDragBase;
        rotateDragCurr = rotateDragBase;
        boostRateCurr = boostRateBase;
        forceModeCurr = forceModeBase;
        thrustBoostCurr = thrustBoostBase;

        rb = GetComponent<Rigidbody>();
        rb.angularDrag = rotateDragCurr;
        rb.drag = thrustDragCurr;

    }

    public virtual void Rotate()
    {
        RotateWithPhysics();
    }

    public virtual void RotateWithPhysics()
    {
        // Rotation with physics
        rb.AddRelativeTorque(rotateSpeedCurr * rotateV, rotateSpeedCurr * rotateH, rotateSpeedCurr * rotateZ);
    }

    public virtual void RotateWithLook()
    {
        // Rotate manually
        float step = rotateSpeedCurr * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);

    }

    public virtual void Thrust()
    {
        // Thrust
        rb.AddRelativeForce(new Vector3(thrustHorizontal, 0.0f, thrustVertical) * thrustSpeedCurr, forceModeCurr);
    }

    public virtual void Drag()
    {
        rb.angularDrag = rotateDragCurr;
        rb.drag = thrustDragCurr;
    }

    public virtual void BrakeOn()
    {
        thrustDragCurr = thrustDragBase * 10f;
        rotateDragCurr = rotateDragBase * 10f;
        thrustSpeedCurr = 0;
    }

    public virtual void BrakeOff()
    {
        thrustDragCurr = thrustDragBase;
        rotateDragCurr = rotateDragBase;
        thrustSpeedCurr = thrustSpeedBase;
    }

    public virtual void StartBoost()
    {
        if (Time.time > boostCooldownTimer && !boosting)
        {
            thrustSpeedCurr = thrustBoostCurr;
            forceModeCurr = ForceMode.Impulse;
            boostCooldownTimer = Time.time + boostRateCurr;
            boostTimer = 0;
            boosting = true;
        }

    }

    public virtual void Boost()
    {
        //  Boosting for 1 frame
        if (boosting)
        {
            boostTimer++;
            if (boostTimer > 0)
            {
                EndBoost();
            }
        }
    }

    public virtual void EndBoost()
    {
        boosting = false;
        thrustSpeedCurr = thrustSpeedBase;
        forceModeCurr = forceModeBase;
        boostTimer = 0;
    }

    public virtual void SetThrustV(float t)
    {
        thrustVertical = t;
    }

    public virtual void SetThrustH(float t)
    {
        thrustHorizontal = t;
    }

    public virtual void SetRotateV(float r)
    {
        rotateV = r;
    }

    public virtual void SetRotateH(float r)
    {
        rotateH = r;
    }

    public virtual void SetRotateZ(float r)
    {
        rotateZ = r;
    }

    public virtual void SetThrust(float tH, float tV, float rH, float rV, float rZ)
    {
        SetThrustH(tH);
        SetThrustV(tV);
        SetRotateH(rH);
        SetRotateV(rV);
        SetRotateZ(rZ);
    }

    public void SetThrustSpeed(float S)
    {
        thrustSpeedCurr = S;
    }

    public void SetThrustBase(float S)
    {
        thrustSpeedBase = S;
    }

    public virtual void SetTargetDirection(Vector3 d)
    {
        targetDir = d;
    }
}
