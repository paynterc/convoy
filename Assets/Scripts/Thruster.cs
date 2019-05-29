using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    Rigidbody rb;

    // Movement
    private float thrustHorizontal=0;// 1 or 0
    private float thrustVertical=0;// 1 or 0
    public float thrustSpeedBase = 5.0f;
    public float thrustSpeedCurr;
    public float thrustDragBase = 0.25f;
    public float thrustDragCurr;
    public float thrustBoostBase = 30.0f;
    public float thrustBoostCurr;
    private ForceMode forceModeBase = ForceMode.Force;
    private ForceMode forceModeCurr;
    private bool braking = false;
    // Rotating
    private float rotateH = 0;
    private float rotateV = 0;
    private float rotateZ = 0;
    public float rotateSpeedBase = 1.0f;
    public float rotateSpeedCurr;
    public float rotateDragBase = 2.0f;
    public float rotateDragCurr;

    // Timers
    public float boostRateBase = 2.0f;// Time between boosts
    public float boostRateCurr;
    private float boostCooldownTimer = 0;// Keep track of time to next boost.
    private bool boosting = false;// Boosting or not
    private int boostTimer = 0;// Keep track of time for current boost

    // Used for manual rotation
    public Vector3 targetDir;
    public Quaternion rotateTo;

    // Thruster effects
    public ParticleSystem psLeft;
    public ParticleSystem psRight;
    public ParticleSystem psFront;
    public ParticleSystem psBack;
    private ParticleSystem.MinMaxCurve boostCurve = new ParticleSystem.MinMaxCurve(2.0f, 0.5f);
    private ParticleSystem.MinMaxCurve normalCurve = new ParticleSystem.MinMaxCurve(0.75f, 0.5f);
    private ParticleSystem.MainModule pMain;
    private bool pMainSet = false;

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

        UpdateThrusterEffects();
    }

    public virtual void UpdateThrusterEffects()
    {
        if (psBack && !pMainSet)
        {
            pMainSet = true;
            pMain = psBack.main;
        }

        if (braking)
        {
            SetThrusterEffects(1, 1, 1, 1);
        }
        else
        {
            int left = thrustHorizontal > 0 || rotateH > 0 ? 1 : 0;
            int right = thrustHorizontal < 0 || rotateH < 0  ? 1 : 0;
            int front = thrustVertical  < 0 || rotateV < 0 ? 1 : 0;
            int back = thrustVertical > 0 || rotateV > 0 ? 1 : 0;
            SetThrusterEffects(left, right, front, back);
        }

    }

    // Turn effect on and off. 0 for off, 1 for on
    public virtual void SetThrusterEffects(int left, int right, int front, int back)
    {
        


        if (psLeft)
        {
            if (left == 1){psLeft.Play();}else{psLeft.Stop();}
        }
        if (psRight)
        {
            if (right == 1) { psRight.Play(); } else { psRight.Stop(); }
        }
        if (psFront)
        {
            if (front == 1) { psFront.Play(); } else { psFront.Stop(); }
        }
        if (psBack)
        {

            if (boosting)
            {
                pMain.startSize = boostCurve;
            }
            else
            {
                pMain.startSize = normalCurve;
            }
            if (back == 1) { psBack.Play(); } else { psBack.Stop(); }

        }

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
        SetThrusterEffects(0, 0, 0, 0);

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
        thrustDragCurr = thrustDragBase * 5f;
        rotateDragCurr = rotateDragBase * 5f;
        braking = true;
        //thrustSpeedCurr = 0;
    }

    public virtual void BrakeOff()
    {
        thrustDragCurr = thrustDragBase;
        rotateDragCurr = rotateDragBase;
        //thrustSpeedCurr = thrustSpeedBase;
        braking = false;
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

    public float GetBoostCooldownTime()
    {
        return boostCooldownTimer;
    }
}
