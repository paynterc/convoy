using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestExplosion : MonoBehaviour
{
    private Explosion explosion;

    // Start is called before the first frame update
    void Start()
    {
        explosion = GetComponent<Explosion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MyFire();// Fire a burst

        }

        if (Input.GetButtonDown("Fire2"))
        {
            Restart();// Fire a burst

        }
    }


    public void MyFire()
    {
        explosion.Explode(transform.position);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
