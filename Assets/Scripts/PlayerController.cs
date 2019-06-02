using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : AbstractUnitController
{

    private Camera playercamera;

    // Zooming and Time Slow
    private float fovMax;
    public float fovMin=20f;
    public float fovInc=120f;
    public float fov;
    private bool zooming = false;
    private int zoomDir = -1;// 1 or -1
    public float zoomDuration = 2f;// Time that zoom lasts
    public float zoomRate = 3f;// Time between zooms
    private float zoomDurationTimer = 0f;
    private float zoomCooldownTimer = 0f;
    private bool zoomed;

    // Time slow
    private float timeMax = 1.0f;
    public float timeMin = 0.5f;



    public override void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.layer = 8;
        thruster = GetComponent<PlayerThruster>();
        playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        fovMax = playercamera.fieldOfView;
        fov = fovMax;
        weapons = GetComponentsInChildren<Weapon>();
        InitWeapons();
    }

    public override void UpdateStep()
    {
        thruster.SetThrust(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), Input.GetAxis("Roll"));

        // Manual Rotation
        //Vector3 tgtPosition = playercamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        //thruster.targetDir = tgtPosition - transform.position;


        if (Input.GetButtonDown("Boost"))
        {
            thruster.StartBoost();
        }

        if (Input.GetButton("Fire1"))
        {

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].Fire();// Fire a burst
            }
        }

        // -1 = zoom in, 1 = zoom out
        if (Input.GetButton("Fire2") && !zooming)
        {
            if (!zoomed)
            {
                if (Time.time > zoomCooldownTimer)
                {
                    zooming = true;
                }
            }
            else
            {
                zooming = true;// Zoom out
            }

        }

        if (Input.GetButton("Brake"))
        {
            thruster.BrakeOn();
        }
        else
        {
            thruster.BrakeOff();
        }

        if (!zooming && zoomed && Time.time > zoomDurationTimer)
        {
            zooming = true;// Zoom out
        }

        if (zooming)
        {
            ToggleZoom();
        }
    }

    private void InitWeapons()
    {

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].layerMask = 1 << 9;
        }

    }

    public void ToggleZoom()
    {

        playercamera.fieldOfView = fov;
        fov += fovInc * Time.deltaTime * zoomDir;
        fov = Mathf.Clamp(fov, fovMin, fovMax);
        if (zoomDir>0)
        {
            Time.timeScale = 1.0f;
        }
        if (fov<=fovMin)
        {

            zooming = false;
            zoomed = true;
            zoomDir = zoomDir * -1;
            fov = fovMin;
            Time.timeScale = timeMin;
            zoomDurationTimer = Time.time + zoomDuration;


        }
        if (fov>=fovMax)
        {

            zooming = false;
            zoomed = false;
            zoomDir = zoomDir * -1;
            fov = fovMax;
            zoomCooldownTimer = Time.time + zoomRate;
            zoomDurationTimer = 0;

        }
    }


    public float GetZoomTimer()
    {
        return zoomDurationTimer;
   
    }

    public float GetZoomCooldownTimer()
    {
        return zoomCooldownTimer;

    }

    public override void TakingDamage()
    {
        // Apply logic when taking damage
        EventManager.TriggerEvent("playerDamage");
    }

}
