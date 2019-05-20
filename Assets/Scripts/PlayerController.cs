using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerThruster thruster;
    private Camera playercamera;
    private Weapon weapon;

    private float fovMax;
    public float fovMin=20f;
    public float fovInc=120f;
    public float fov;
    private bool zooming = false;
    private int zoomDir = 1;// 1 or -1

    // Time slow
    private float timeMax = 1.0f;
    public float timeMin = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 8;
        thruster = GetComponent<PlayerThruster>();
        weapon = GameObject.Find("PlayerWeapon").GetComponent<Weapon>();
        playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        fovMax = playercamera.fieldOfView;
        fov = fovMax;
        Cursor.lockState = CursorLockMode.Locked;
        weapon.hitLayer = 1 << 9;

    }

    // Update is called once per frame
    void Update()
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
            weapon.Fire();// Fire a burst

        }

        if (Input.GetButton("Fire2") && !zooming)
        {
            zooming = true;
        }

        if (zooming)
        {
            ToggleZoom();
        }

    }

    public void ToggleZoom()
    {
        Debug.Log("zooming " + fov);
        playercamera.fieldOfView = fov;
        fov += fovInc * Time.deltaTime * zoomDir;
        fov = Mathf.Clamp(fov, fovMin, fovMax);
        if (zoomDir>0)
        {
            Time.timeScale = 1.0f;
        }
        if (fov<=fovMin)
        {
            fov = fovMin;
            zooming = false;
            zoomDir = zoomDir * -1;
            Time.timeScale = timeMin;
        }
        if (fov>=fovMax)
        {
            fov = fovMax;
            zooming = false;
            zoomDir = zoomDir * -1;
        }
    }

}
