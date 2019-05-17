using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrusterOld : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 targetDir;
    public float moveHorizontal = 0;
    public float moveVertical = 1;
    public float speed = 1.0f;
    public float turnSpeed = 1.0f;
    public float defaultSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = defaultSpeed;
        turnSpeed = 2.0f;
        rb.angularDrag = 2f;
        rb.drag = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);

        // Thrust
        rb.AddRelativeForce(new Vector3(moveHorizontal, 0.0f, moveVertical) * speed, ForceMode.Force);
    }

    public void SetSpeed(float setSpeed)
    {
        speed = setSpeed;
    }

}
