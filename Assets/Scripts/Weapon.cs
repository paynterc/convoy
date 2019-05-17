using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float burstRate=1f;// Time between bursts
    public int burstMagnitude=3;// Bullets per burst
    private int burstCount = 0;// Current burst count
    private float burstCooldown = 0f;// Time of next burst
    private bool bursting=false;// Currently firing a burst;
    private int burstIdx = 0;// Burst index. For debug purposes, keep track of the current burst by giving it a number.

    public float fireRate = 0.10f;// Time between shots. Should be less than burstRate / burstMagnitude.
    public float fireDuration = 0.05f; // Amount of time that shot will be visible. Should be less than fireRate
    private float fireCooldown = 0f;// Time of next shot
    private float fireTimer = 0f;// Keep track of amount of time the shot is visible

    public float damage=1;
    public float range = 100f;

    private RaycastHit hit;
    private LineRenderer line;
    private Camera playercamera;

    public int origin = 0;// 0 for player, 1 for other. This will determine where the raycast begins: player camera or transform.position.
    private bool hitscan;
    public LayerMask hitLayer = 1 << 9;
    // Start is called before the first frame update
    void Start()
    {
        // Normal burst: burstRate =1f; fireRate = .10f; fireDuration = .05f
        // Constant fire: burstRate =.10f; fireRate = .10f; fireDuration = .05f

        playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        MakeLine();
    }

    // Update is called once per frame
    void Update()
    {



        if (bursting)
        {
            if (burstCount< burstMagnitude)
            {
                if (Time.time > fireCooldown)
                {
                    FireShot();
                    burstCount++;
                }

            }
            else
            {
                bursting = false;
                burstCooldown = Time.time + burstRate;
            }

        }

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

    // Fire a burst
    public void Fire()
    {
        if (!bursting && Time.time > burstCooldown)
        {
            StartBurst();
        }


    }

    public void StartBurst()
    {
        burstIdx++;
        burstCount = 0;
        bursting = true;
    }

    public void FireShot()
    {

        fireCooldown = Time.time + fireRate;
        fireTimer = Time.time + fireDuration;
        // Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)
        // Raycast from gun. Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range)

        if (origin==0)
        {
            // Raycast from camera
            Debug.Log("Fire Hit Layer " + hitLayer);
            Vector3 rayOrigin = playercamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
            hitscan = Physics.Raycast(rayOrigin, playercamera.transform.forward, out hit, range, hitLayer);
        }
        else
        {
            Debug.Log("Fire Hit Layer " + hitLayer);
            hitscan = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, hitLayer);
        }

        if (hitscan)
        {
            Debug.Log(burstIdx + "," +burstCount + ": Did Hit. Hit Layer: " + hitLayer);
            hit.transform.GetComponent<Hull>().ApplyDamage(damage);

        }
        else
        {

            //Debug.Log(burstIdx + "," + burstCount + ": Did not Hit");
        }

    }

    public void MakeLine()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.startColor = Color.red;
        line.endColor = Color.blue;
        line.material = new Material(Shader.Find("Transparent/Diffuse"));
        line.startWidth = .5f;
        line.endWidth = .25f;
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.one);
        line.enabled = false;
    }

}
