using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0.10f;// Time between shots. Should be less than burstRate / burstMagnitude.
    public float fireDuration = 0.05f; // Amount of time that shot will be visible. Should be less than fireRate
    protected float fireCooldown = 0f;// Time of next shot
    protected float fireTimer = 0f;// Keep track of amount of time the shot is visible

    public float damage=1;
    public float range = 100f;

    protected RaycastHit hit;
    protected LineRenderer line;
    protected Camera playercamera;

    public int origin = 0;// 0 for player, 1 for other. This will determine where the raycast begins: player camera or transform.position.
    protected bool hitscan;
    // layerMask = 1 << 8. This would cast rays only against colliders in layer 8.
    // But instead, if we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask. layerMask = ~layerMask;
    public LayerMask layerMask = 1 << 8;
    public Material lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // Use with burst, charge or sustain weapons
        UpdateFire();

        DrawShot();
    }

    public virtual void Init()
    {
        if (GameObject.Find("PlayerCamera"))
        {
            playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        }
        MakeLine();
    }

    // Fire. CALLED FROM CONTROLLER
    public virtual void Fire()
    {
        if (Time.time > fireCooldown)
        {
            FireShot();
        }

    }

    public virtual void UpdateFire()
    {
        // Fill this in with timers or whatever for charged shots or burst shots.

    }

    public virtual void DrawShot()
    {
        if (line)
        {
            // Draw laser line
            if (Time.time < fireTimer)
            {
                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position + (transform.forward * range));
            }
            else
            {
                line.enabled = false;
            }
        }

    }

    public virtual void FireShot()
    {

        fireCooldown = Time.time + fireRate;
        fireTimer = Time.time + fireDuration;
        // Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)
        // Raycast from gun. Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range)

        if (origin==0)
        {
            // Raycast from camera

            Vector3 rayOrigin = playercamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
            hitscan = Physics.Raycast(rayOrigin, playercamera.transform.forward, out hit, range, layerMask);
        }
        else
        {

            hitscan = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask);
        }

        if (hitscan)
        {
            Hull hull = hit.transform.GetComponent<Hull>();
            if (hull != null)
            {
                hit.transform.GetComponent<Hull>().ApplyDamage(damage,hit.point);
            }

        }
        else
        {

            //Debug.Log("Shot Did not Hit");
        }



    }

    public virtual void MakeLine()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startColor = Color.blue;
        line.endColor = Color.white;
        //line.material = new Material(Shader.Find("Transparent/Diffuse"));
        line.material = lineMaterial;
        line.startWidth = .5f;
        line.endWidth = .25f;
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.one);
        line.enabled = false;
    }

}
