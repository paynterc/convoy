using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerThruster thruster;
    private Camera playercamera;
    private Weapon weapon;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 8;
        thruster = GetComponent<PlayerThruster>();
        weapon = GameObject.Find("PlayerWeapon").GetComponent<Weapon>();
        playercamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
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

    }

}
